
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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace clearpixels.Helpers.generics
{
    /// <summary>
    /// a faster method compared to Activator
    /// http://www.smelser.net/blog/post/2010/03/05/When-Activator-is-just-to-slow.aspx
    /// http://stackoverflow.com/questions/367577/why-does-the-c-sharp-compiler-emit-activator-createinstance-when-calling-new-in
    /// </summary>
    public static class ParameterlessConstructor<T>
        where T : new()
    {
        public static T Create()
        {
            return _func();
        }

        private static Func<T> CreateFunc()
        {
            return Expression.Lambda<Func<T>>(Expression.New(typeof (T))).Compile();
        }

        private static Func<T> _func = CreateFunc();
    }
}
