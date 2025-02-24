using JasperFx.Core.Reflection;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Lamar.DynamicInterception
{

    public class FuncInterceptor<T> : IFuncInterceptor
    {
        private readonly LambdaExpression _expression;
        private readonly string _description;

        // SAMPLE: FuncInterceptor-by-expression
        public FuncInterceptor(Expression<Func<T, T>> expression, string description = null)
        // ENDSAMPLE
        {
            _expression = expression;
            _description = description;
        }

        // SAMPLE: FuncInterceptor-by-expression-and-icontext
        public FuncInterceptor(Expression<Func<IServiceContext, T, T>> expression, string description = null)
        // ENDSAMPLE
        {
            _expression = expression;
            _description = description;
        }


        public Expression ToExpression(ParameterExpression context, ParameterExpression variable)
        {
            var body = _expression.ReplaceParameter(Accepts, variable)
                .ReplaceParameter(typeof(IServiceContext), context).Body;

            return Expression.Convert(body, typeof(T));

        }
        public Expression ToExpression(ParameterExpression variable)
        {
            var body = _expression.ReplaceParameter(Accepts, variable).Body;
            return Expression.Convert(body, typeof(T));
        }

        public virtual string Description
        {
            get
            {
                var bodyDescription = _description ?? _expression
                    .ReplaceParameter(Accepts, Expression.Parameter(Accepts, Accepts.Name))
                    .ReplaceParameter(typeof(IServiceContext), Expression.Parameter(typeof(IServiceContext), "IContext"))
                    .Body.ToString();

                return String.Format("FuncInterceptor of {0}: {1}", typeof(T).FullNameInCode(), bodyDescription);

            }
        }

        public InterceptorRole Role
        {
            get { return InterceptorRole.Decorates; }
        }

        public Type Accepts
        {
            get { return typeof(T); }
        }

        public Type Returns
        {
            get { return typeof(T); }
        }

        protected bool Equals(FuncInterceptor<T> other)
        {
            return Equals(_expression, other._expression);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FuncInterceptor<T>)obj);
        }

        public override int GetHashCode()
        {
            return (_expression != null ? _expression.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return String.Format("Interceptor of {0}: {1}", typeof(T).FullNameInCode(), Description);
        }
    }

    public static class DelegateExtensions
    {
        public static LambdaExpression ReplaceParameter(this LambdaExpression expression, Type acceptsType,
            ParameterExpression newParam)
        {
            return ParameterRewriter.ReplaceParameter(acceptsType, expression, newParam);
        }
    }

    public class ParameterRewriter : ExpressionVisitor
    {
        private readonly ParameterExpression _before;
        private readonly ParameterExpression _after;

        public static LambdaExpression ReplaceParameter(Type acceptsType, LambdaExpression expression,
            ParameterExpression newParam)
        {
            var before = expression.Parameters.FirstOrDefault(x => x.Type == acceptsType) ?? expression.Parameters.FirstOrDefault(x => x.Type.CanBeCastTo(acceptsType));

            if (before == null) return expression;

            var rewriter = new ParameterRewriter(before, newParam);
            return rewriter.VisitAndConvert(expression, "Activator");
        }

        public ParameterRewriter(ParameterExpression before, ParameterExpression after)
        {
            _before = before;
            _after = after;
        }


        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node != _before) return node;

            if (_after.Type == _before.Type) return _after;

            try
            {
                return Expression.Convert(_after, _before.Type);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}