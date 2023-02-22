using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Serialization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;

namespace EasyAbp.Abp.EventBus.Cap;

public class AbpJsonSerializer : ISerializer
{
    public static bool CamelCase;
    private readonly IJsonSerializer _jsonSerializer;
    public AbpJsonSerializer(IJsonSerializer jsonSerializer, IOptions<AbpSystemTextJsonSerializerOptions> jsonOptions)
    {
        _jsonSerializer = jsonSerializer;
        CamelCase = jsonOptions.Value.JsonSerializerOptions.PropertyNamingPolicy == null ? false : true;
    }

    public virtual string Serialize(Message message)
    {
        return _jsonSerializer.Serialize(message, CamelCase);
    }

    public virtual ValueTask<TransportMessage> SerializeAsync(Message message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (message.Value == null)
        {
            return new ValueTask<TransportMessage>(new TransportMessage(message.Headers, null));
        }

        var json = _jsonSerializer.Serialize(message.Value, CamelCase);

        return new ValueTask<TransportMessage>(new TransportMessage(message.Headers, Encoding.UTF8.GetBytes(json)));
    }

    public virtual Message Deserialize(string json)
    {
        return _jsonSerializer.Deserialize<Message>(json, CamelCase);
    }

    public virtual ValueTask<Message> DeserializeAsync(TransportMessage transportMessage, Type valueType)
    {
        if (valueType == null || transportMessage.Body.IsEmpty)
        {
            return new ValueTask<Message>(new Message(transportMessage.Headers, null));
        }

        var json = Encoding.UTF8.GetString(transportMessage.Body.ToArray());

        return new ValueTask<Message>(new Message(transportMessage.Headers,
            _jsonSerializer.Deserialize(valueType, json, CamelCase)));
    }

    public virtual object Deserialize(object value, Type valueType)
    {
        return _jsonSerializer.Deserialize(valueType, value.ToString(), CamelCase);
    }

    public virtual bool IsJsonType(object jsonObject)
    {
        return jsonObject is JToken or JsonElement;
    }
}