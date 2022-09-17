using System;
using System.Linq.Expressions;

namespace Global.Shared.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<TEntity, bool>> And<TEntity>(
            this Expression<Func<TEntity, bool>> leftSideExpression,
            Expression<Func<TEntity, bool>> rightSideExpression)
        {
            var param = Expression.Parameter(typeof(TEntity));

            var leftSideVisitor = new ReplaceExpressionVisitor(leftSideExpression.Parameters[0], param);
            var newLeftSideExpression = leftSideVisitor.Visit(leftSideExpression.Body);

            var rightSideVisitor = new ReplaceExpressionVisitor(rightSideExpression.Parameters[0], param);
            var newRightSideExpression = rightSideVisitor.Visit(rightSideExpression.Body);

            var body = Expression.AndAlso(newLeftSideExpression, newRightSideExpression);
            return Expression.Lambda<Func<TEntity, bool>>(body, param);
        }
    }

    internal class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue, _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
                return _newValue;
            return base.Visit(node);
        }
    }
}
