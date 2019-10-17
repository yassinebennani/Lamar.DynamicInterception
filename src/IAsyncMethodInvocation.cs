using System.Threading.Tasks;

namespace Lamar.DynamicInterception
{
    public interface IAsyncMethodInvocation : IMethodInvocation
    {
        Task<IMethodInvocationResult> InvokeNextAsync();
    }
}