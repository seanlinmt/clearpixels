
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using clearpixels.Helpers.exceptions;

namespace clearpixels.Helpers.viewengines
{
    // From http://www.codeproject.com/KB/aspnet/ASP2UserControlLibrary.aspx

    public class AssemblyVirtualPathProvider : VirtualPathProvider
    {
        private readonly Dictionary<string, Assembly> nameAssemblyCache;

        public AssemblyVirtualPathProvider()
        {
            nameAssemblyCache = new Dictionary<string, Assembly>(StringComparer.InvariantCultureIgnoreCase);
        }

        private static bool IsModulePath(string virtualPath)
        {
            string checkPath = VirtualPathUtility.ToAppRelative(virtualPath);

            return checkPath.StartsWith("~/Plugin/",
                                        StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool FileExists(string virtualPath)
        {
            return (IsModulePath(virtualPath) ||
                    base.FileExists(virtualPath));
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsModulePath(virtualPath))
            {
                return new AssemblyResourceFile(nameAssemblyCache, virtualPath);
            }

            return base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(
            string virtualPath,
            IEnumerable virtualPathDependencies,
            DateTime utcStart)
        {
            if (IsModulePath(virtualPath))
            {
                return null;
            }

            return base.GetCacheDependency(virtualPath,
                                           virtualPathDependencies, utcStart);
        }

        private class AssemblyResourceFile : VirtualFile
        {
            private readonly IDictionary<string, Assembly> nameAssemblyCache;
            private readonly string assemblyPath;

            public AssemblyResourceFile(IDictionary<string, Assembly> nameAssemblyCache, string virtualPath) :
                base(virtualPath)
            {
                this.nameAssemblyCache = nameAssemblyCache;
                assemblyPath = VirtualPathUtility.ToAppRelative(virtualPath);
            }

            public override Stream Open()
            {
                // ~/App_Resource/WikiExtension.dll/WikiExtension/Presentation/Views/Wiki/Index.aspx (or .ascx)
                var parts = assemblyPath.Split(new[] { '/' }, 4);

                if (parts.Length != 4 || parts[0] != "~" || parts[1] != "App_Resource")
                {
                    throw new ClearpixelsException("Wrong number of parts in assmbly path: '{0}'. Expected ~/App_Resource/<assmblyFileName>/<Path to view>.aspx (or .ascx)");
                }

                var assemblyName = parts[2];
                var resourceName = parts[3].Replace('/', '.');

                Assembly assembly;

                lock (nameAssemblyCache)
                {
                    if (!nameAssemblyCache.TryGetValue(assemblyName, out assembly))
                    {
                        var path = Path.Combine(HttpRuntime.BinDirectory, assemblyName);
                        assembly = Assembly.LoadFrom(path);

                        // TODO: Assert is not null
                        nameAssemblyCache[assemblyName] = assembly;
                    }
                }

                if (assembly == null)
                {
                    throw new ClearpixelsException("Could not load AddIn assembly '{0}' when attempting to load AddIn view '{1}'", assemblyName, assemblyPath);
                }

                var resourceStream = assembly.GetManifestResourceStream(resourceName);

                if (resourceStream == null)
                {
                    throw new ClearpixelsException("Could not load AddIn view. Failed to find resource '{0}' in assembly '{1}'", resourceName, assemblyName);
                }

                return resourceStream;
            }
        }
    }
}