
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
using System.Diagnostics;
using System.Text;

namespace clearpixels.Logging
{
    internal static class EventlogHelper
    {
        internal static void Write(this EventLog log,
          string message,
          EventLogEntryType type,
          int eventID,
          Exception ex)
        {
            if (String.IsNullOrWhiteSpace(message))
                throw new ArgumentException("message is null or empty.", "message");
            if (ex == null)
                throw new ArgumentNullException("ex", "ex is null.");
            
            var seperator = new String('-', 50);
            var builder = new StringBuilder(message);

            // Write each of the inner exception messages.
            Exception parentException = ex;
            bool isInnerException = false;
            do
            {
                builder.AppendLine();
                builder.AppendLine(seperator);
                if (isInnerException)
                    builder.Append("INNER ");
                builder.AppendLine("EXCEPTION DETAILS");
                builder.AppendLine();
                builder.Append("EXCEPTION TYPE:\t");
                builder.AppendLine(parentException.GetType().ToString());
                builder.AppendLine();
                builder.Append("EXCEPTION MESSAGE:\t");
                builder.AppendLine(parentException.Message);
                builder.AppendLine();
                builder.AppendLine("STACK TRACE:");
                builder.AppendLine(parentException.StackTrace);


                parentException = parentException.InnerException;
                isInnerException = true;
            }
            while (parentException != null);

            log.Source = "OverwatchSource";
            log.WriteEntry(builder.ToString(), type, (int)eventID);
        }
    }
}
