using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Json;
using Volo.Abp.Testing;
using Xunit;

namespace EasyAbp.Abp.EventBus.CAP;

public class NewtonsoftSerializerTests : AbpIntegratedTest<AbpEventBusCapTestsModule>
{
    protected ISerializer Serializer;

    public NewtonsoftSerializerTests()
    {
        Serializer = GetRequiredService<ISerializer>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.PreConfigure<AbpJsonOptions>(options =>
        {
            options.UseHybridSerializer = false;
        });
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
}