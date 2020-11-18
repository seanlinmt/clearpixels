using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace clearpixels.Models
{
    public class TimerActionState
    {
        private string key;
        private Action action;
        private string value;

        public TimerActionState(string key, Action action, string value)
        {
            this.key = key;
            this.action = action;
            this.value = value;
        }
    }
}
