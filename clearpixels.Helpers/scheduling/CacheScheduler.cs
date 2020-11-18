using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Caching;
using clearpixels.Logging;

namespace clearpixels.Helpers.scheduling
{
    public sealed class CacheScheduler
    {
        public readonly static CacheScheduler Instance = new CacheScheduler();

        private CacheScheduler()
        {

        }

        public void StartSystemThread(Action p, int v)
        {
            throw new NotImplementedException();
        }
    }
}
