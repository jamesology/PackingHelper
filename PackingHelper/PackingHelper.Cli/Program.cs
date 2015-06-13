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

            log.Debug("Log Configured.");
            Console.ReadLine();
        }
    }
}
