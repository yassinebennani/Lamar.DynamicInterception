using Lamar.IoC.Instances;
using System;
using System.Collections.Generic;

namespace Lamar.DynamicInterception
{
    public static class InstanceExtension
    {
        #region InterceptWith Methods

        public static LambdaInstance<IServiceContext, TPluginType> InterceptWith<TPluginType, TPluginTypeProxy>(this ServiceRegistry.InstanceExpression<TPluginType> instance, Func<IServiceContext, IInterceptionBehavior> behavior)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            return instance.Use(InterceptorFunctionBuilder.Build<TPluginType, TPluginTypeProxy>(behavior));
        }

        public static LambdaInstance<IServiceContext, TPluginType> InterceptWith<TPluginType, TPluginTypeProxy>(this ServiceRegistry.InstanceExpression<TPluginType> instance, IInterceptionBehavior behavior)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            return instance.Use(InterceptorFunctionBuilder.Build<TPluginType, TPluginTypeProxy>(behavior));
        }

        public static LambdaInstance<IServiceContext, TPluginType> InterceptWith<TPluginType, TPluginTypeProxy>(this ServiceRegistry.InstanceExpression<TPluginType> instance, IEnumerable<IInterceptionBehavior> behaviors)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            return instance.Use(InterceptorFunctionBuilder.Build<TPluginType, TPluginTypeProxy>(behaviors));
        }

        public static LambdaInstance<IServiceContext, TPluginType> InterceptWith<TPluginType, TPluginTypeProxy>(this ServiceRegistry.InstanceExpression<TPluginType> instance, Type behaviorType)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            IInterceptionBehavior behavior = (IInterceptionBehavior)Activator.CreateInstance(behaviorType);
            return instance.InterceptWith<TPluginType,TPluginTypeProxy>(behavior);
        }

        public static LambdaInstance<IServiceContext, TPluginType> InterceptWith<TPluginType, TPluginTypeProxy>(this ServiceRegistry.InstanceExpression<TPluginType> instance, IEnumerable<Type> behaviorTypes)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            ICollection<IInterceptionBehavior> behaviors = new List<IInterceptionBehavior>();
            foreach (Type behavior in behaviorTypes)
            {
                behaviors.Add((IInterceptionBehavior)Activator.CreateInstance(behavior));
            }
            return instance.InterceptWith<TPluginType, TPluginTypeProxy>(behaviors);
        }

        #endregion InterceptWith Methods

        #region Named InterceptWith Methods

        public static LambdaInstance<IServiceContext, TPluginType> InterceptWith<TPluginType, TPluginTypeProxy>(this ServiceRegistry.InstanceExpression<TPluginType> instance, String instanceName, IInterceptionBehavior behavior)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            return instance.Use(InterceptorFunctionBuilder.Build<TPluginType, TPluginTypeProxy>(instanceName, behavior)).Named(instanceName);
        }

        public static LambdaInstance<IServiceContext, TPluginType> InterceptWith<TPluginType, TPluginTypeProxy>(this ServiceRegistry.InstanceExpression<TPluginType> instance, String instanceName, IEnumerable<IInterceptionBehavior> behaviors)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            return instance.Use(InterceptorFunctionBuilder.Build<TPluginType, TPluginTypeProxy>(instanceName, behaviors)).Named(instanceName);
        }

        public static LambdaInstance<IServiceContext, TPluginType> InterceptWith<TPluginType, TPluginTypeProxy>(this ServiceRegistry.InstanceExpression<TPluginType> instance, String instanceName, Type behaviorType)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            IInterceptionBehavior behavior = (IInterceptionBehavior)Activator.CreateInstance(behaviorType);
            return instance.InterceptWith<TPluginType, TPluginTypeProxy>(instanceName, behavior);
        }

        public static LambdaInstance<IServiceContext, TPluginType> InterceptWith<TPluginType, TPluginTypeProxy>(this ServiceRegistry.InstanceExpression<TPluginType> instance, String instanceName, IEnumerable<Type> behaviorTypes)
            where TPluginType : class
            where TPluginTypeProxy : TPluginType
        {
            ICollection<IInterceptionBehavior> behaviors = new List<IInterceptionBehavior>();
            foreach (Type behavior in behaviorTypes)
            {
                behaviors.Add((IInterceptionBehavior)Activator.CreateInstance(behavior));
            }
            return instance.InterceptWith<TPluginType, TPluginTypeProxy>(instanceName, behaviors);
        }

        #endregion Named InterceptWith Methods
    }
}