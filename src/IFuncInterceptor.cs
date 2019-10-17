using System;
using System.Linq.Expressions;

namespace Lamar.DynamicInterception
{
    public interface IFuncInterceptor : IInterceptor
    {
        Expression ToExpression(ParameterExpression context, ParameterExpression variable);
        Expression ToExpression(ParameterExpression variable);
    }
}