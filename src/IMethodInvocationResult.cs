using System;

namespace Lamar.DynamicInterception
{
    public interface IMethodInvocationResult
    {
        bool Successful { get; }

        object ReturnValue { get; }

        Exception Exception { get; }

        object GetReturnValueOrThrow();
    }
}