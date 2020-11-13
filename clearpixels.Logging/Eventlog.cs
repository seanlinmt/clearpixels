
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
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace clearpixels.Logging
{
    public class Eventlog
    {
        private readonly EventLog logger;

        public Eventlog(string moduleName)
        {
            logger = new EventLog(moduleName + "Log");


        }
        public void Info(string text)
        {
            logger.WriteEntry(text, EventLogEntryType.Information);
        }

        public void Warn(string text)
        {
            logger.WriteEntry(text, EventLogEntryType.Warning);
        }

        public void Error(string text, int eventid = 0)
        {
            logger.WriteEntry(text, EventLogEntryType.Error, eventid);
        }

        public void Error(string text, Exception ex, int eventid = 0)
        {
            logger.Write(text, EventLogEntryType.Error, eventid, ex);
        }
    }

}
