using JasperFx.Core.Reflection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Lamar.DynamicInterception
{
    internal class InterceptorFunctionBuilder
    {
        #region Build Registration Functions Methods

        public static Func<IServiceContext, TPluginType> Build<TPluginType, TPluginTypeProxy>(Func<IServiceContext, IInterceptionBehavior> behavior)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            Func<IServiceContext, TPluginType> proxyFunction = (context) =>
            {
                TPluginType service = context.GetInstance<TPluginTypeProxy>();
                return InterceptorFunctionBuilder.BuildInterceptorFunction<TPluginType>(behavior(context))(service);
            };
            return proxyFunction;
        }

        public static Func<IServiceContext, TPluginType> Build<TPluginType, TPluginTypeProxy>(IInterceptionBehavior behavior)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            Func<IServiceContext, TPluginType> proxyFunction = (context) =>
            {
                TPluginType service = context.GetInstance<TPluginTypeProxy>();
                return InterceptorFunctionBuilder.BuildInterceptorFunction<TPluginType>(behavior)(service);
            };
            return proxyFunction;
        }

        public static Func<IServiceContext, TPluginType> Build<TPluginType, TPluginTypeProxy>(IEnumerable<IInterceptionBehavior> behaviors)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            Func<IServiceContext, TPluginType> proxyFunction = (context) =>
            {
                TPluginType service = context.GetInstance<TPluginTypeProxy>();
                return InterceptorFunctionBuilder.BuildInterceptorsFunction<TPluginType>(behaviors)(service);
            };
            return proxyFunction;
        }

        public static Func<IServiceContext, TPluginType> Build<TPluginType, TPluginTypeProxy>(String instanceName, IInterceptionBehavior behavior)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            Func<IServiceContext, TPluginType> proxyFunction = (context) =>
            {
                TPluginType service = context.GetInstance<TPluginTypeProxy>(instanceName);
                return InterceptorFunctionBuilder.BuildInterceptorFunction<TPluginType>(behavior)(service);
            };
            return proxyFunction;
        }

        public static Func<IServiceContext, TPluginType> Build<TPluginType, TPluginTypeProxy>(String instanceName, IEnumerable<IInterceptionBehavior> behaviors)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            Func<IServiceContext, TPluginType> proxyFunction = (context) =>
            {
                TPluginType service = context.GetInstance<TPluginTypeProxy>(instanceName);
                return InterceptorFunctionBuilder.BuildInterceptorsFunction<TPluginType>(behaviors)(service);
            };
            return proxyFunction;
        }


        #endregion Build Registration Functions Methods

        #region Build Interceptor Functions Methods

        private static Func<TPluginType, TPluginType> BuildInterceptorFunction<TPluginType>(IInterceptionBehavior behavior)
             where TPluginType : class
        {
            ICollection<IInterceptionBehavior> behaviors = new List<IInterceptionBehavior>();
            behaviors.Add(behavior);
            return BuildInterceptorsFunction<TPluginType>(behaviors);
        }
        private static Func<TPluginType, TPluginType> BuildInterceptorsFunction<TPluginType>(IEnumerable<IInterceptionBehavior> behaviors)
             where TPluginType : class
        {
            DynamicProxyInterceptor<TPluginType> interceptor = new DynamicProxyInterceptor<TPluginType>(behaviors);
            ParameterExpression variable = Expression.Variable(typeof(TPluginType), "pluginType");
            Expression expression = interceptor.ToExpression(variable);
            Type lambdaType = typeof(Func<TPluginType, TPluginType>);
            LambdaExpression lambda = Expression.Lambda(lambdaType, expression, variable);
            return lambda.Compile().As<Func<TPluginType, TPluginType>>();
        }

        #endregion Build Interceptor Functions Methods
    }
}