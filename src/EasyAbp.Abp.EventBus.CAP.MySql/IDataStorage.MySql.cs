// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DotNetCore.CAP.Internal;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Monitoring;
using DotNetCore.CAP.Persistence;
using DotNetCore.CAP.Serialization;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace DotNetCore.CAP.MySql
{
    public class MySqlDataStorage<TDbContext> : DapperRepository<TDbContext>, IDataStorage 
        where TDbContext : IEfCoreDbContext
    {
        private readonly IOptions<EFOptions> _options;
        private readonly IOptions<CapOptions> _capOptions;
        private readonly IStorageInitializer _initializer; 
        public IMonitoringApi MonitoringApi { get; set; }

        public MySqlDataStorage(
            IOptions<EFOptions> options,
            IDbContextProvider<TDbContext> dbContextProvider,
            IOptions<CapOptions> capOptions,
            IStorageInitializer initializer)
            :base(dbContextProvider)
        {
            _options = options;
            _capOptions = capOptions;
            _initializer = initializer;
        }

        public async Task ChangePublishStateAsync(MediumMessage message, StatusName state)
        {
            var connection = this.DbConnection;

            var sql = $"UPDATE `{_initializer.GetPublishedTableName()}` SET `Retries` = @Retries,`ExpiresAt` = @ExpiresAt,`StatusName`=@StatusName WHERE `Id`=@Id;";

            await connection.ExecuteAsync(sql, new
            {
                Id = message.DbId,
                message.Retries,
                message.ExpiresAt,
                StatusName = state.ToString("G")
            }, transaction: DbTransaction);
        }

        public async Task ChangeReceiveStateAsync(MediumMessage message, StatusName state)
        {
            var connection = this.DbConnection;

            var sql = $"UPDATE `{_initializer.GetReceivedTableName()}` SET `Retries` = @Retries,`ExpiresAt` = @ExpiresAt,`StatusName`=@StatusName WHERE `Id`=@Id;";

            await connection.ExecuteAsync(sql, new
            {
                Id = message.DbId,
                message.Retries,
                message.ExpiresAt,
                StatusName = state.ToString("G")
            }, transaction: DbTransaction);
        }

        public MediumMessage StoreMessage(string name, Message content, object dbTransaction = null)
        {
            var sql = $"INSERT INTO `{_initializer.GetPublishedTableName()}`(`Id`,`Version`,`Name`,`Content`,`Retries`,`Added`,`ExpiresAt`,`StatusName`) VALUES(@Id,'{_options.Value.Version}',@Name,@Content,@Retries,@Added,@ExpiresAt,@StatusName);";

            var message = new MediumMessage
            {
                DbId = content.GetId(),
                Origin = content,
                Content = StringSerializer.Serialize(content),
                Added = DateTime.Now,
                ExpiresAt = null,
                Retries = 0
            };

            var po = new
            {
                Id = message.DbId,
                Name = name,
                message.Content,
                message.Retries,
                message.Added,
                message.ExpiresAt,
                StatusName = nameof(StatusName.Scheduled)
            };

            if (dbTransaction == null)
            {
                var connection = this.DbConnection;
                connection.Execute(sql, po, transaction: DbTransaction);
            }
            else
            {
                var dbTrans = dbTransaction as IDbTransaction;
                if (dbTrans == null && dbTransaction is IDbContextTransaction dbContextTrans)
                {
                    dbTrans = dbContextTrans.GetDbTransaction();
                }

                var conn = dbTrans?.Connection;
                conn.Execute(sql, po, dbTrans);
            }

            return message;
        }

        public void StoreReceivedExceptionMessage(string name, string group, string content)
        {
            var sql = $@"INSERT INTO `{_initializer.GetReceivedTableName()}`(`Id`,`Version`,`Name`,`Group`,`Content`,`Retries`,`Added`,`ExpiresAt`,`StatusName`) VALUES(@Id,'{_options.Value.Version}',@Name,@Group,@Content,@Retries,@Added,@ExpiresAt,@StatusName);";
           
            var connection = this.DbConnection;
            connection.Execute(sql, new
            {
                Id = SnowflakeId.Default().NextId().ToString(),
                Group = @group,
                Name = name,
                Content = content,
                Retries = _capOptions.Value.FailedRetryCount,
                Added = DateTime.Now,
                ExpiresAt = DateTime.Now.AddDays(15),
                StatusName = nameof(StatusName.Failed)
            }, transaction: DbTransaction);
        }

        public MediumMessage StoreReceivedMessage(string name, string group, Message message)
        {
            var sql = $@"INSERT INTO `{_initializer.GetReceivedTableName()}`(`Id`,`Version`,`Name`,`Group`,`Content`,`Retries`,`Added`,`ExpiresAt`,`StatusName`) VALUES(@Id,'{_options.Value.Version}',@Name,@Group,@Content,@Retries,@Added,@ExpiresAt,@StatusName);";

            var mdMessage = new MediumMessage
            {
                DbId = SnowflakeId.Default().NextId().ToString(),
                Origin = message,
                Added = DateTime.Now,
                ExpiresAt = null,
                Retries = 0
            };
            var content = StringSerializer.Serialize(mdMessage.Origin);
            var connection = this.DbConnection;
            connection.Execute(sql, new
            {
                Id = mdMessage.DbId,
                Group = @group,
                Name = name,
                Content = content,
                mdMessage.Retries,
                mdMessage.Added,
                mdMessage.ExpiresAt,
                StatusName = nameof(StatusName.Scheduled)
            }, transaction: DbTransaction);
            return mdMessage;
        }

        public async Task<int> DeleteExpiresAsync(string table, DateTime timeout, int batchCount = 1000, CancellationToken token = default)
        {
            var connection = this.DbConnection;

            return await connection.ExecuteAsync(
                $@"DELETE FROM `{table}` WHERE ExpiresAt < @timeout limit @batchCount;",
                new { timeout, batchCount }, transaction: DbTransaction);
        }

        public async Task<IEnumerable<MediumMessage>> GetPublishedMessagesOfNeedRetry()
        {
            var fourMinAgo = DateTime.Now.AddMinutes(-4).ToString("O");
            var sql = $"SELECT * FROM `{_initializer.GetPublishedTableName()}` WHERE `Retries`<{_capOptions.Value.FailedRetryCount} AND `Version`='{_capOptions.Value.Version}' AND `Added`<'{fourMinAgo}' AND (`StatusName` = '{StatusName.Failed}' OR `StatusName` = '{StatusName.Scheduled}') LIMIT 200;";

            var result = new List<MediumMessage>();
            var connection = this.DbConnection;

            var reader = await connection.ExecuteReaderAsync(sql, transaction: DbTransaction);
            while (reader.Read())
            {
                result.Add(new MediumMessage
                {
                    DbId = reader.GetInt64(0).ToString(),
                    Origin = StringSerializer.DeSerialize(reader.GetString(3)),
                    Retries = reader.GetInt32(4),
                    Added = reader.GetDateTime(5)
                });
            }
            return result;
        }

        public async Task<IEnumerable<MediumMessage>> GetReceivedMessagesOfNeedRetry()
        {
            var fourMinAgo = DateTime.Now.AddMinutes(-4).ToString("O");
            var sql =
                $"SELECT * FROM `{_initializer.GetReceivedTableName()}` WHERE `Retries`<{_capOptions.Value.FailedRetryCount} AND `Version`='{_capOptions.Value.Version}' AND `Added`<'{fourMinAgo}' AND (`StatusName` = '{StatusName.Failed}' OR `StatusName` = '{StatusName.Scheduled}') LIMIT 200;";

            var result = new List<MediumMessage>();

            var connection = this.DbConnection;

            var reader = await connection.ExecuteReaderAsync(sql, transaction: DbTransaction);
            while (reader.Read())
            {
                result.Add(new MediumMessage
                {
                    DbId = reader.GetInt64(0).ToString(),
                    Origin = StringSerializer.DeSerialize(reader.GetString(4)),
                    Retries = reader.GetInt32(5),
                    Added = reader.GetDateTime(6)
                });
            }

            return result;
        }

        public IMonitoringApi GetMonitoringApi()
        {
            return MonitoringApi;
        }
    }
}
