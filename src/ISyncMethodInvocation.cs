namespace Lamar.DynamicInterception
{
    public interface ISyncMethodInvocation : IMethodInvocation
    {
        IMethodInvocationResult InvokeNext();
    }
}