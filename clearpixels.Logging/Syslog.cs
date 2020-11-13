
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
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using System.Transactions;
using System.Web;
using Elmah;

namespace clearpixels.Logging
{
    public enum ErrorLevel
    {
        [Description("Critical")]
        CRITICAL,
        [Description("Error")]
        ERROR,
        [Description("Warning")]
        WARNING,
        [Description("Information")]
        INFORMATION,
        [Description("Verbose")]
        VERBOSE
    }

    [SecuritySafeCritical]
    public class Syslog
    {
        public static void Write(Exception ex)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (HttpContext.Current != null)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
                else
                {
                    ErrorLog.GetDefault(null).Log(new Error(ex));
                }
                scope.Complete();
            }
        }

        public static void Write(string format, params object[] parameters)
        {
            var lex = new ExceptionWrapper(string.Format(format, parameters), new StackTrace(1, true));
            Write(lex);
        }

        public static void Write(Exception ex, string format, params object[] parameters)
        {
            var lex = new ExceptionWrapper(string.Format(format, parameters), ex);

            Write(lex);
        }
    }
}