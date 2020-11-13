using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using clearpixels.Logging;

namespace clearpixels.CacheScheduler
{
    public sealed class CacheScheduler
    {
        // one thread for each timer, timer value is used as key to map of threads
#if DEBUG
        public const string HTTP_CACHEURL = "http://localhost:1545/dummy";
#else
        public const string HTTP_CACHEURL = "http://localhost/dummy";
#endif
        
        public readonly static CacheScheduler Instance = new CacheScheduler();
        private readonly Dictionary<CacheTimerType, Thread> runningThreads = new Dictionary<CacheTimerType, Thread>();

        private readonly Dictionary<int, List<Action>> scheduledTasks = new Dictionary<int, List<Action>>();

        private CacheScheduler()
        {
            var timers = Enum.GetValues(typeof(CacheTimerType));
            foreach (int timer in timers)
            {
                scheduledTasks[timer] = new List<Action>();
            }
        }

        public void AddTask(CacheTimerType type, Action action)
        {
            scheduledTasks[(int) type].Add(action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval">in seconds</param>
        /// <returns></returns>
        public string GetKey(int interval)
        {
            return string.Format("CacheTimerKey_{0}", interval);
        }

        public void Start()
        {
            Debug.WriteLine("CacheScheduler: STARTED");
            var timers = Enum.GetValues(typeof(CacheTimerType));
            foreach (int timer in timers)
            {
                var key = GetKey(timer);

                if (HttpRuntime.Cache[key] == null)
                {
                    HttpRuntime.Cache.Add(key, timer, null, DateTime.Now.AddSeconds(timer),
                        Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable,
                        CacheItemRemovedCallback);
                }
            }
        }

        private void CacheItemRemovedCallback(
            string key,
            object value,
            CacheItemRemovedReason reason
            )
        {
            Debug.WriteLine("CacheScheduler: Expired " + key);

            List<Action> tasks;
            var interval = (int) value;

            if (scheduledTasks.TryGetValue(interval, out tasks))
            {
                var timerType = (CacheTimerType)interval;
                var thread = new Thread(() => Parallel.ForEach(tasks, x =>
                                                                          {
                                                                              try
                                                                              {
                                                                                  x.Invoke();
                                                                              }
                                                                              catch (Exception ex)
                                                                              {
                                                                                  Syslog.Write(ex);
                                                                              }
                                                                          })) {Name = string.Format("{0}_WORK", timerType)};

                if (!runningThreads.ContainsKey(timerType))
                {
                    runningThreads.Add(timerType, thread);
                    thread.Start();
                }
                else
                {
                    if (!runningThreads[timerType].IsAlive)
                    {
                        runningThreads[timerType] = thread;
                        thread.Start();
                    }
                    else
                    {
                        Debug.WriteLine(string.Format("CacheScheduler: Thread {0} still alive", interval));
                    }
                }
            }
            HitPage();
        }

        private static void HitPage()
        {
            using (var client = new WebClient())
            {
                client.DownloadData(HTTP_CACHEURL);    
            }
        }
    }
}
