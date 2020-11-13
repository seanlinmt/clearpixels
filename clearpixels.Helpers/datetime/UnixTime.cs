
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

namespace clearpixels.Helpers.datetime
{
    public static class UnixTime
    {
        private static readonly DateTime EpochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(this double unixTime)
        {
            return EpochUtc.AddSeconds(unixTime);
        }

        public static DateTime ToDateTime(this double utc)
        {
            return EpochUtc.AddSeconds(utc);
        }

        public static DateTime ToLocalDateTime(this double utc)
        {
            return utc.ToDateTime().ToLocalTime();
        }

        public static double ToLocalUnixTime(this DateTime local)
        {
            TimeSpan diff = local - EpochUtc;
            return Math.Floor(diff.TotalSeconds);
        }

        public static double ToUnixTime(this DateTime local)
        {
            TimeSpan diff = local.ToUniversalTime() - EpochUtc;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}