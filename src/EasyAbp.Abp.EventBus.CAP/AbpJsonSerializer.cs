using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Serialization;
using Newtonsoft.Json.Linq;
using Volo.Abp.Json;

namespace EasyAbp.Abp.EventBus.Cap;

public class AbpJsonSerializer : ISerializer
{
    private readonly IJsonSerializer _jsonSerializer;

    public AbpJsonSerializer(IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    public virtual string Serialize(Message message)
    {
        return _jsonSerializer.Serialize(message);
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

        var json = _jsonSerializer.Serialize(message.Value);

        return new ValueTask<TransportMessage>(new TransportMessage(message.Headers, Encoding.UTF8.GetBytes(json)));
    }

    public virtual Message Deserialize(string json)
    {
        return _jsonSerializer.Deserialize<Message>(json);
    }

    public virtual ValueTask<Message> DeserializeAsync(TransportMessage transportMessage, Type valueType)
    {
        if (valueType == null || transportMessage.Body.IsEmpty)
        {
            return new ValueTask<Message>(new Message(transportMessage.Headers, null));
        }

        var json = Encoding.UTF8.GetString(transportMessage.Body.ToArray());

        return new ValueTask<Message>(new Message(transportMessage.Headers,
            _jsonSerializer.Deserialize(valueType, json)));
    }

    public virtual object Deserialize(object value, Type valueType)
    {
        return _jsonSerializer.Deserialize(valueType, value.ToString());
    }

    public virtual bool IsJsonType(object jsonObject)
    {
        return jsonObject is JToken or JsonElement;
    }
}