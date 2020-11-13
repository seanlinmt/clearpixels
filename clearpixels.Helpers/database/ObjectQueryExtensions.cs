using System.Data.Entity.Core.Objects;

namespace clearpixels.Helpers.database
{
    public static class ObjectQueryExtensions
    {
        public static ObjectQuery<T> DisablePlanCaching<T>(this ObjectQuery<T> query)
        {
            query.EnablePlanCaching = false;
            return query;
        }
        public static ObjectQuery DisablePlanCaching(this ObjectQuery query)
        {
            query.EnablePlanCaching = false;
            return query;
        }
    }
}
