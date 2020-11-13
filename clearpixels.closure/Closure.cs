using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace clearpixels.closure
{
    public class Closure
    {
        private static int Main(string[] args)
        {
            if (args.Length != 6)
            {
                Console.WriteLine("Invalid arguments");
                return 1;
            }
            string workingdir = args[0];
            string[] features = args[1].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            string outputfile = args[2];
            string library = args[3];
            string externs = args[4];
            string compilationlevel = args[5];

            var compressor = new Compress(workingdir);
            var inputfile = compressor.BuildJSFeature(features, outputfile.Replace(".js",".unc.js"));
            var libraryfile = compressor.BuildJSFeature(new[]{library}, library + ".unc.js");
            try
            {
                // compress main files
                //compressor.Start(inputfile, externs, "ADVANCED_OPTIMIZATIONS", "3");
                compressor.Start(inputfile, string.Join(",", new[] { externs, library }), compilationlevel, "3");

                // lightly compress library files
                compressor.Start(libraryfile, externs, "SIMPLE_OPTIMIZATIONS", "3");
            }
            catch (Exception)
            {
                
                return 1;
            }
            

            return 0;
        }
    }
}
