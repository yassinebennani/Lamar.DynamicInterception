using Castle.DynamicProxy;
using JasperFx.Core.Reflection;
using Lamar.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lamar.DynamicInterception
{
    public class DynamicProxyInterceptor<TPluginType> : FuncInterceptor<TPluginType>
        where TPluginType : class

    {
        private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

        private readonly string _description;

        public DynamicProxyInterceptor(IEnumerable<Type> interceptionBehaviorTypes) : this(interceptionBehaviorTypes.ToArray())
        {
        }

        public DynamicProxyInterceptor(params Type[] interceptionBehaviorTypes) : base(BuildExpression(interceptionBehaviorTypes))
        {
            _description = BuildDescription(interceptionBehaviorTypes);
        }

        public DynamicProxyInterceptor(IEnumerable<IInterceptionBehavior> interceptionBehaviors) : this(interceptionBehaviors.ToArray())
        {
        }

        private DynamicProxyInterceptor(IInterceptionBehavior[] interceptionBehaviors) : base(BuildExpression(interceptionBehaviors))
        {
            _description = BuildDescription(interceptionBehaviors.Select(b => b.GetType()));
        }

        private static Expression<Func<IServiceContext, TPluginType, TPluginType>> BuildExpression(IEnumerable<Type> interceptionBehaviorTypes)
        {
            return (context, instance) => proxyGenerator.CreateInterfaceProxyWithTarget(
                instance,
                WrapInterceptors(interceptionBehaviorTypes.Select(t => (IInterceptionBehavior)context.GetInstance(t)).ToArray())
            );
        }

        private static Expression<Func<TPluginType, TPluginType>> BuildExpression(IEnumerable<IInterceptionBehavior> interceptionBehaviors)
        {
            return instance => proxyGenerator.CreateInterfaceProxyWithTarget(
                instance,
                WrapInterceptors(interceptionBehaviors.ToArray())
            );
        }

        private static CastleInterceptor WrapInterceptors(IInterceptionBehavior[] interceptionBehaviors)
        {
            foreach (var behavior in interceptionBehaviors)
            {
                if (!(behavior is ISyncInterceptionBehavior) &&
                    !(behavior is IAsyncInterceptionBehavior))
                {
                    throw new LamarException(
                        $"{behavior.GetType().FullNameInCode()} implements neither ISyncInterceptionBehavior nor IAsyncInterceptionBehavior");
                }
            }
            return new CastleInterceptor(interceptionBehaviors);
        }

        private static string BuildDescription(IEnumerable<Type> interceptionBehaviorTypes)
        {
            return
                $"DynamicProxyInterceptor of {typeof(TPluginType).FullNameInCode()} with interception behaviors: {string.Join(", ", interceptionBehaviorTypes.Select(t => t.FullNameInCode()))}";
        }

        public override string Description => _description;
    }
}