using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace App2
{
    public interface IApp2MessagingService : ITransientDependency
    {
        Task RunAsync(string message);
    }
}