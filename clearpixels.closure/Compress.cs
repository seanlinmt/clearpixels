using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using clearpixels.Logging;

namespace clearpixels.closure
{
    internal class Compress
    {
        private readonly string workingdir;

        internal Compress(string workingdir)
        {
            this.workingdir = workingdir;
        }

        internal string BuildJSFeature(string[] features, string outputfile)
        {
            var jsloader = new JsLoader(workingdir);
            var path = workingdir + outputfile;
            try
            {
                using (var output = File.CreateText(path))
                {
                    foreach (var feature in features)
                    {
                        output.Write(jsloader.LoadFeatures(feature));
                    }
                }
            }
            catch (Exception ex)
            {
                
                Syslog.Write(ex);
            }
            

            return path;
        }

        internal void Start(string path, string externs, string compilation_level, string detail_level)
        {
            string oldFile = path;
            string newFile = oldFile.Replace(".unc.js", ".min.js");
            using (var p = new Process())
            {
                // build cmd argument
                var argumentBuilder = new StringBuilder();

                argumentBuilder.AppendFormat(
                    @"-jar ""{0}"" --js {1} --js_output_file {2} --compilation_level {3} --summary_detail_level {4}",
                    @"D:\code\clearpixels\clearpixels.closure\jar\compiler.jar",
                    oldFile,
                    newFile,
                    compilation_level,
                    detail_level);

                if (!string.IsNullOrEmpty(externs))
                {
                    foreach (var extfile in externs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!extfile.EndsWith("js"))
                        {
                            var outputfile = BuildJSFeature(new[] { extfile }, extfile + ".js");
                            argumentBuilder.AppendFormat(" --externs {0}", outputfile);
                        }
                        else
                        {
                            argumentBuilder.AppendFormat(" --externs {0}{1}", workingdir, extfile);
                        }
                    }
                }


                // google map extern http://closure-compiler.googlecode.com/svn/trunk/contrib/externs/maps/
                argumentBuilder.Append(" --externs D:/code/clearpixels/clearpixels.closure/externs/google_maps_api_v3_9.js");

                p.StartInfo = new ProcessStartInfo
                {
                    FileName = "java",
                    Arguments = argumentBuilder.ToString(),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                p.StartInfo.EnvironmentVariables["Path"] = Environment.GetEnvironmentVariable("Path");
                try
                {
                    if (compilation_level != "NONE")
                    {
                        p.Start();
                        string[] warnings = p.StandardError.ReadToEnd()
                            .Replace("\r", String.Empty)
                            .Split(new[] {"\n\n"}, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var warning in warnings)
                        {
                            Debug.WriteLine(warning);
                        }
                        p.WaitForExit(5000);
                    }
                }
                catch (Exception ex)
                {
                    Syslog.Write(ex);
                }
            }
        }
    }
}
