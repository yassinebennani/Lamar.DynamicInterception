using System;

namespace Lamar.DynamicInterception
{
    public interface IInterceptor
    {
        InterceptorRole Role { get; }

        String Description { get; }

        Type Accepts { get; }

        Type Returns { get; }
    }
}