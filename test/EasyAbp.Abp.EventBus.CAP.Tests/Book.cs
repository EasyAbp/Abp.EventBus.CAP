using System;
using System.Text.Json.Serialization;
using Volo.Abp.Data;

namespace EasyAbp.Abp.EventBus.CAP;

public class Book : IHasExtraProperties
{
    public string Name { get; set; }

    [JsonInclude]
    public ExtraPropertyDictionary ExtraProperties { get; protected set; } = new();
    
    public Book(string name)
    {
        Name = name;
    }
}
