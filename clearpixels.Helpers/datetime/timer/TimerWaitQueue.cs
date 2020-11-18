using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clearpixels.Models;

namespace clearpixels.Helpers.datetime.timer
{
    public class TimerWaitQueue
    {
        public readonly static TimerWaitQueue Instance = new TimerWaitQueue();

        private TimerWaitQueue()
        {

        }

        public IEnumerable<object> GetAllKeys(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllTimers()
        {
            throw new NotImplementedException();
        }

        public bool IsTimerRunning(string key)
        {
            throw new NotImplementedException();
        }

        public bool Add(TimerActionState state, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
    }
}
