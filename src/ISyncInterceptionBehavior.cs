﻿namespace Lamar.DynamicInterception
{
    public interface ISyncInterceptionBehavior : IInterceptionBehavior
    {
        IMethodInvocationResult Intercept(ISyncMethodInvocation methodInvocation);
    }
}