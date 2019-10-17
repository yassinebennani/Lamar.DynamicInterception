# Lamar.DynamicInterception
This project is an adapation of StructureMap.DynamicInterception.
This Project is just a workaround to support dynamic interceptions with Lamar as interceptors are not supported yet natively by Lamar.

Available on [![NuGet](https://img.shields.io/nuget/v/Lamar.DynamicInterception)](https://www.nuget.org/packages/Lamar.DynamicInterception/) latest version.<br>

Package manager console command for installation: 
*Install-Package Lamar.DynamicInterception*

## Interceptor Behavior Definition
```cs
namespace App
{
    using Lamar.DynamicInterception;
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
## Service Interfaces Definitions
```cs
public interface IWidget
{
   String GetTitle();
}

public interface IWidgetProxy : IWidget
{
    //you should define this proxy Interface to act just as a proxy.
    //Never add any definition method in this interface.
    //All definitions methods should go into IWidget.
    //I created this interface to handel activator interception
    //because activator interceptor is not supported yet
    //by Lamar.
}
```
## Service Implementation
```cs
public class Widget : IWidget, IWidgetProxy
{
   public String GetTitle()
   {
       return "My name is DefaultWidget"
   }
}
```

## Service Registration
```cs
var container = new Container(services =>
{
   services.For<IWidgetProxy>().Use<Widget>;
   services.For<IWidget>.InterceptWith<IWidget, IWidgetProxy>(typeof(CustomInterceptionBehavior))
});
```
