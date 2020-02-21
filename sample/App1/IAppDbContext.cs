using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace App1
{
    [ConnectionStringName("Default")]
    public interface IAppDbContext : IEfCoreDbContext
    {

    }
}
