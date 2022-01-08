namespace EasyAbp.Abp.EventBus.Cap;

public interface ICapDbProviderInfoProvider
{
    CapDbProviderInfo GetOrNull(string dbProviderName);
}