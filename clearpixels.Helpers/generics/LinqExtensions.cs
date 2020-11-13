using System;
using System.Collections.Generic;
using System.Linq;

namespace clearpixels.Helpers.generics
{

    public static class LinqExtensions
    {
        // http://brendan.enrick.com/post/linq-your-collections-with-iequalitycomparer-and-lambda-expressions.aspx
        public static IEnumerable<TSource> Except2<TSource>(this IEnumerable<TSource> first,
                                                           IEnumerable<TSource> second,
                                                           Func<TSource, TSource, bool> comparer)
        {
            return first.Except(second, new LambdaComparer<TSource>(comparer));
        }

        public static IEnumerable<TSource> Distinct2<TSource>(this IEnumerable<TSource> first,
                                                           Func<TSource, TSource, bool> comparer)
        {
            return first.Distinct(new LambdaComparer<TSource>(comparer));
        }
    }
}
