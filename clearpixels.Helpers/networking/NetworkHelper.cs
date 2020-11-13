
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
using System.Net;
using System.Net.Sockets;

namespace clearpixels.Helpers.networking
{
    public static class NetworkHelper
    {
        public static bool IsPrivate(this IPAddress ipaddress)
        {
            if (IPAddress.IsLoopback(ipaddress))
            {
                return true;
            }

            if (ipaddress.AddressFamily ==  AddressFamily.InterNetworkV6)
            {
                if (ipaddress.IsIPv6LinkLocal || 
                    ipaddress.IsIPv6SiteLocal)
                {
                    return true;
                }
                return false;
            }

            String[] straryIPAddress = ipaddress.ToString().Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            int[] iaryIPAddress = new int[] { int.Parse(straryIPAddress[0]), int.Parse(straryIPAddress[1]), int.Parse(straryIPAddress[2]), int.Parse(straryIPAddress[3]) };
            if (iaryIPAddress[0] == 10 || 
                (iaryIPAddress[0] == 192 && iaryIPAddress[1] == 168) || 
                (iaryIPAddress[0] == 172 && (iaryIPAddress[1] >= 16 && iaryIPAddress[1] <= 31)))
            {
                return true;
            }
            else
            {
                
                // IP Address is "probably" public. This doesn't catch some VPN ranges like OpenVPN and Hamachi.
                return false;
            }
        }

        public static bool PingTest(string ipaddr = "8.8.8.8")
        {
            var ping = new System.Net.NetworkInformation.Ping();

            System.Net.NetworkInformation.PingReply pingStatus =
                ping.Send(IPAddress.Parse(ipaddr));

            if (pingStatus == null ||
                pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                return false;
            }
            return true;
        }
    }
}
