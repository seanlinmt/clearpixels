
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

namespace clearpixels.crypto
{
    // TODO: i have a feeling this might end up like http://dilbert.com/strips/comic/2001-10-25/. Try use microsoft's implementation.
    public sealed class RandomNumberGenerator : Random
    {
        private static readonly Random _global = new Random();
        [ThreadStatic]
        private static Random _localInstance;

        RandomNumberGenerator()
        {

        }

        public static Random Instance
        {
            get
            {
                Random inst = _localInstance;
                if (inst == null)
                {
                    int seed;
                    lock (_global) seed = _global.Next();
                    _localInstance = new Random(seed);
                }
                return _localInstance;
            }
        }
    }

}
