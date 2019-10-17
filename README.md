# Lamar.DynamicInterception
This project is an adapation of StructureMap.DynamicInterception.
This Project is just a workaround to support dynamic interceptions with Lamar as interceptors are not supported yet natively by Lamar.

```cs
namespace App
{
    using Lamar.Interception;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class CustomInterceptionBehavior : IAsyncInterceptionBehavior
    {

        public async Task<IMethodInvocationResult> InterceptAsync(IAsyncMethodInvocation methodInvocation)
        {
            MethodInfo method = methodInvocation.MethodInfo;

            return await methodInvocation.InvokeNextAsync().ConfigureAwait(false);
        }
    }
}
```
