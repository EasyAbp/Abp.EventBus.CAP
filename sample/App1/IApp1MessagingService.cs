using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace App1
{
    public interface IApp1MessagingService: ITransientDependency
    {
        Task RunAsync(string message);
    }
}