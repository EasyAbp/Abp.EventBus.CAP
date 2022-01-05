using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Serialization;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace EasyAbp.Abp.EventBus.Cap;

[Dependency(ReplaceServices = true)]
public class AbpJsonSerializer : ISerializer, ISingletonDependency
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

    public virtual Task<TransportMessage> SerializeAsync(Message message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (message.Value == null)
        {
            return Task.FromResult(new TransportMessage(message.Headers, null));
        }

        var json = _jsonSerializer.Serialize(message.Value);
        
        return Task.FromResult(new TransportMessage(message.Headers, Encoding.UTF8.GetBytes(json)));
    }

    public virtual Message Deserialize(string json)
    {
        return _jsonSerializer.Deserialize<Message>(json);
    }

    public virtual Task<Message> DeserializeAsync(TransportMessage transportMessage, Type valueType)
    {
        if (valueType == null || transportMessage.Body == null)
        {
            return Task.FromResult(new Message(transportMessage.Headers, null));
        }

        var json = Encoding.UTF8.GetString(transportMessage.Body);
        
        return Task.FromResult(new Message(transportMessage.Headers, _jsonSerializer.Deserialize(valueType, json)));
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