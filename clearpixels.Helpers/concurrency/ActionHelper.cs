using System;

namespace clearpixels.Helpers.concurrency
{
    public static class ActionHelper
    {
        public static bool CompareWith(this Action firstAction, Action secondAction)
        {
            if (firstAction.Target != secondAction.Target)
                return false;

            var firstMethodBody = firstAction.Method.GetMethodBody().GetILAsByteArray();
            var secondMethodBody = secondAction.Method.GetMethodBody().GetILAsByteArray();

            if (firstMethodBody.Length != secondMethodBody.Length)
                return false;

            for (var i = 0; i < firstMethodBody.Length; i++)
            {
                if (firstMethodBody[i] != secondMethodBody[i])
                    return false;
            }
            return true;
        }
    }
}
