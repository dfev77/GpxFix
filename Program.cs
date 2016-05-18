using System;

namespace GpxFix
{
    public static class Program
    {
        static int Main(string[] args)
        {
            try
            {
                if (args.Length == 1)
                {
                    GpxUpdater gpxUpdater = new GpxUpdater();
                    gpxUpdater.Process(args[0]);
                    return 0;
                }
                else
                {
                    Console.WriteLine("Missing input file");
                    Console.WriteLine("Usage: GpxFix <gpx file path>");
                    return -1;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error processing file: {0}", ex);
                return -2;
            }
        }

    }
}
