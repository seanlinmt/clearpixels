using System;
using System.Linq.Expressions;

namespace clearpixels.Helpers.database
{
    public class UpdateExpression<TEntity>
    {
        public Expression<Func<TEntity, bool>> filterExpression { get; set; }
        public Expression<Func<TEntity, TEntity>> updateExpression { get; set; }
    }
}
