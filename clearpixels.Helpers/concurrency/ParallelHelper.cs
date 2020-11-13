/*
 * LICENSE NOTE:
 *
 * Copyright  2012-2013 Clear Pixels Limited, All Rights Reserved.
 *
 * Unless explicitly acquired and licensed from Licensor under another license, the
 * contents of this file are subject to the Reciprocal Public License ("RPL")
 * Version 1.5, or subsequent versions as allowed by the RPL, and You may not copy
 * or use this file in either source code or executable form, except in compliance
 * with the terms and conditions of the RPL. 
 *
 * All software distributed under the RPL is provided strictly on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND LICENSOR HEREBY
 * DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT LIMITATION, ANY WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, QUIET ENJOYMENT, OR
 * NON-INFRINGEMENT. See the RPL for specific language governing rights and
 * limitations under the RPL.
 *
 * @author         Sean Lin Meng Teck <seanlinmt@clearpixels.co.nz>
 * @copyright      2012-2013 Clear Pixels Limited
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace clearpixels.Helpers.concurrency
{
    public class ParallelHelper
    {
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private Timer _t;

        public ParallelHelper(CancellationTokenSource cts)
        {
            _cts = cts;
            _token = _cts.Token;
        }

        public ParallelHelper(CancellationToken ct)
        {
            _token = ct;
        }
        public ParallelHelper(int timeout)
        {
            // timer
            _cts = new CancellationTokenSource();
            _t = new Timer(_ => _cts.Cancel(), null, timeout, -1);
            _token = _cts.Token;
        }

        public IEnumerable<TLocal> ProcessData<TSource, TLocal>(IEnumerable<TSource> data,
            Func<TSource, IEnumerable<TLocal>> body)
        {
            var returnValueList = new ConcurrentBag<TLocal>();
            var exceptions = new ConcurrentQueue<Exception>();

            try
            {
                Parallel.ForEach(data,
                             new ParallelOptions { CancellationToken = _token },
                             Enumerable.Empty<TLocal>,
                             (x, state, local) =>
                             {
                                 _token.ThrowIfCancellationRequested();
                                 try
                                 {
                                     return body(x);
                                 }
                                 catch (Exception ex)
                                 {
                                     exceptions.Enqueue(ex);
                                 }

                                 return local;
                             },
                             z =>
                                 {
                                     if (z != null)
                                     {
                                         foreach (var entry in z)
                                         {
                                             returnValueList.Add(entry);
                                         }
                                         
                                     }
                                 });
            }
            finally
            {
                if (exceptions.Count > 0) throw new AggregateException(exceptions);
            }
                                     
            return returnValueList;
        }

        public void ProcessData<TSource>(IEnumerable<TSource> data,
            Action<TSource> body)
        {
            var exceptions = new ConcurrentQueue<Exception>();
            

            try
            {
                Parallel.ForEach(data,
                             new ParallelOptions { CancellationToken = _token },
                             x =>
                             {
                                 _token.ThrowIfCancellationRequested();
                                 try
                                 {
                                     body(x);
                                 }
                                 catch (Exception ex)
                                 {
                                     exceptions.Enqueue(ex);
                                 }
                             });
            }
            finally
            {
                if (_t != null)
                {
                    _t.Dispose();
                }

                if (exceptions.Count > 0) throw new AggregateException(exceptions);
            }
        }
    }
}
