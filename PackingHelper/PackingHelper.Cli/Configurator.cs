using System.IO;

using log4net;
using Newtonsoft.Json;

namespace PackingHelper.Cli
{
    internal class Configurator
    {
        public static Configuration Read(string settingsFilePath, ILog log)
        {
            log.Debug("Reading settings file.");

            var configuration = new Configuration();
            using (var stream = new FileStream(settingsFilePath, FileMode.Open, FileAccess.Read))
            using (var textReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(textReader))
            {
                var jsonSerializer = new JsonSerializer();
                configuration = jsonSerializer.Deserialize<Configuration>(jsonReader);
            }

            log.DebugFormat("TaskTemplate location: {0}", configuration.TaskTemplates);
            return configuration;
        }
    }
}
