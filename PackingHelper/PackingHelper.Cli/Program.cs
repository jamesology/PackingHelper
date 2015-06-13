using System;
using System.IO;

using log4net;
using log4net.Config;

namespace PackingHelper.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("PackingHelper.Cli.log4net.config"));
            var log = LogManager.GetLogger("main");

            try
            {
                if (args.Length > 0)
                {
                    var settingsFile = args[0];
                    log.DebugFormat("Settings File: {0}", settingsFile);

                    Configuration configuration;
                    if (File.Exists(settingsFile))
                    {
                        log.Debug("File Found.");
                        configuration = Configurator.Read(settingsFile, log);
                    }
                    else
                    {
                        log.Debug("File Not Found.");
                    }

                    //process task sets
                    //write task file
                }
                else
                {
                    log.Warn("No settings file provided. Unable to proceed.");
                }
            }
            catch(Exception exception)
            {
                log.Error("Oh Snap!", exception);
            }

            Console.Write("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
