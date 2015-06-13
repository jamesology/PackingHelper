using log4net;

namespace PackingHelper.Cli
{
    internal class Configurator
    {
        public static Configuration Read(string settingsFilePath, ILog log)
        {
            log.Debug("Reading settings file.");
            return new Configuration();
        }
    }
}
