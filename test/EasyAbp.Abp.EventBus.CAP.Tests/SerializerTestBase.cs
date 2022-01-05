using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Serialization;
using EasyAbp.Abp.EventBus.CAP.Models;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Json;
using Volo.Abp.Testing;
using Xunit;

namespace EasyAbp.Abp.EventBus.CAP;

public abstract class SerializerTestBase : AbpIntegratedTest<AbpEventBusCapTestsModule>
{
    protected ISerializer Serializer;

    public SerializerTestBase()
    {
        Serializer = GetRequiredService<ISerializer>();
    }

    [Fact]
    public void Should_Deserialize_ExtraProperties()
    {
        var book = new Book("book1");

        book.SetProperty("Id", "58FF1709-14CC-4ECD-B612-65EDCB731A4C");

        var message = new Message(
            headers: new Dictionary<string, string> {
                { "cap-msg-name", "authentication.users.update"},
                { "cap-msg-type", "User" },
                { "cap-corr-seq", "0"},
                { "cap-msg-group","service.v1"}
            },
            value: book);
        
        Serializer.IsJsonType(message.Value).ShouldBeFalse();

        var convertedBook = Convert.ChangeType(message.Value, typeof(Book)) as Book;

        convertedBook.ShouldNotBeNull();
        convertedBook.Name.ShouldBe("book1");
        convertedBook.GetProperty("Id").ShouldBe("58FF1709-14CC-4ECD-B612-65EDCB731A4C");

        var json = Serializer.Serialize(message);
        var deserializedMessage = Serializer.Deserialize(json);
        
        Serializer.IsJsonType(deserializedMessage.Value).ShouldBeTrue();

        var deserializedBook = Serializer.Deserialize(deserializedMessage.Value, typeof(Book)) as Book;
        
        deserializedBook.ShouldNotBeNull();
        deserializedBook.Name.ShouldBe("book1");
        deserializedBook.GetProperty("Id").ShouldBe("58FF1709-14CC-4ECD-B612-65EDCB731A4C");
    }

    [Fact]
    public void Should_Deserialize_And_Get_Extra_Property()
    {
        var json = @"
        {
          ""Headers"": {
            ""cap-callback-name"": null,
            ""cap-msg-id"": ""1478803731525300224"",
            ""cap-corr-id"": ""1478803731525300224"",
            ""cap-corr-seq"": ""0"",
            ""cap-msg-name"": ""EasyAbp.PaymentService.Payments.PaymentEto.Created"",
            ""cap-msg-type"": ""Object"",
            ""cap-senttime"": ""01/06/2022 03:00:59 +08:00"",
            ""cap-msg-group"": ""cap.queue.duotai.dentureplus.web.0.v1"",
            ""cap-exception"": ""SubscriberExecutionFailedException-->Object must implement IConvertible.""
          },
          ""Value"": {
            ""Entity"": {
              ""UserId"": ""39f66e87-25d8-5ac8-eab9-28746e2526b2"",
              ""Id"": ""3a013dca-7d92-9378-cc13-b04cf1ef401b"",
              ""TenantId"": null,
              ""PaymentMethod"": ""Prepayment"",
              ""PayeeAccount"": null,
              ""ExternalTradingCode"": null,
              ""Currency"": ""CNY"",
              ""OriginalPaymentAmount"": 0.2,
              ""PaymentDiscount"": 0,
              ""ActualPaymentAmount"": 0.2,
              ""RefundAmount"": 0,
              ""PendingRefundAmount"": 0,
              ""CompletionTime"": null,
              ""CanceledTime"": null,
              ""CreationTime"": ""2022-01-06T03:00:58.9615749+08:00"",
              ""PaymentItems"": [
                {
                  ""Id"": ""3a013dca-7d31-be42-3553-0d828ac43382"",
                  ""ItemType"": ""EasyAbpEShopOrder"",
                  ""ItemKey"": ""3a013d97-28fb-41a7-a0c8-4a4bb68ead44"",
                  ""OriginalPaymentAmount"": 0,
                  ""PaymentDiscount"": 0,
                  ""ActualPaymentAmount"": 0,
                  ""RefundAmount"": 0,
                  ""PendingRefundAmount"": 0,
                  ""ExtraProperties"": {
                    ""StoreId"": ""39f6370a-a302-afd6-b570-1ebb0cf54d04""
                  }
                },
                {
                  ""Id"": ""3a013dca-7d34-14dc-32b6-2a8fcca6e133"",
                  ""ItemType"": ""EasyAbpEShopOrder"",
                  ""ItemKey"": ""3a013d97-31be-ebf6-ccd2-306df7a6ef40"",
                  ""OriginalPaymentAmount"": 0.2,
                  ""PaymentDiscount"": 0,
                  ""ActualPaymentAmount"": 0.2,
                  ""RefundAmount"": 0,
                  ""PendingRefundAmount"": 0,
                  ""ExtraProperties"": {
                    ""StoreId"": ""39f741fd-f657-fce4-5002-6488349ddf6c""
                  }
                }
              ],
              ""ExtraProperties"": {}
            }
          }
        }";

        var message = Serializer.Deserialize(json);

        var eto = Serializer.Deserialize(message.Value, typeof(EntityWrappedEto<Payment>)) as EntityWrappedEto<Payment>;
        var payment = eto.Entity;

        payment.ShouldNotBeNull();
        payment.PaymentItems.ShouldNotBeNull();
        payment.PaymentItems.Count.ShouldBe(2);
        var firstItem = payment.PaymentItems.First();
        firstItem.Id.ToString().ShouldBe("3a013dca-7d31-be42-3553-0d828ac43382");
        var storeId = firstItem.GetProperty<string>("StoreId");
        storeId.ShouldBe("39f6370a-a302-afd6-b570-1ebb0cf54d04");
    }
}