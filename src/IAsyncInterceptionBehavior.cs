using System.Threading.Tasks;

namespace Lamar.DynamicInterception
{
    public interface IAsyncInterceptionBehavior : IInterceptionBehavior
    {
        Task<IMethodInvocationResult> InterceptAsync(IAsyncMethodInvocation methodInvocation);
    }
}