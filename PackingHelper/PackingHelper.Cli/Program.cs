using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using log4net;
using log4net.Config;
using Newtonsoft.Json;

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

                    var configuration = new Configuration();
                    if (File.Exists(settingsFile))
                    {
                        log.Debug("File Found.");
                        configuration = Configurator.Read(settingsFile, log);
                    }
                    else
                    {
                        log.Debug("File Not Found.");
                    }

                    if (Directory.Exists(configuration.TaskTemplates))
                    {
                        Console.Write("Enter Trip Date: ");
                        var tripDateEntry = Console.ReadLine();
                        DateTime tripDate = DateTime.Today.AddDays(7);
                        DateTime.TryParse(tripDateEntry, out tripDate);

                        Console.Write("Enter Tags (comma separated): ");
                        var tagsEntry = Console.ReadLine();
                        var tags = tagsEntry.Split(',').Select(x => x.Trim()).ToList();
                        log.DebugFormat("Tags: {0}", String.Join(",", tags));

                        var tripInfo = new TripInfo(tags, tripDate);

                        log.Debug("Loading Task Templates.");

                        var tasks = TaskEngine.Process(configuration.TaskTemplates, tripInfo, log);

                        var destinationPath = Path.Combine(configuration.OutputDirectory, String.Format("Tasks_{0:yyyyMMdd}.txt", DateTime.Today));
                        log.DebugFormat("Destination: {0}", destinationPath);
                        using (var fileStream = new StreamWriter(destinationPath, false))
                        {
                            foreach(var task in tasks)
                            {
                                fileStream.WriteLine(task);
                                log.Debug(task);
                            }
                        }
                    }
                    else
                    {
                        log.Warn("Task templates not found.");
                    }
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

    internal class TaskEngine
    {
        public static ICollection<Task> Process(string templateFolderLocation, TripInfo tripInfo, ILog log)
        {
            var result = new List<Task>();
            foreach(var taskFile in Directory.EnumerateFiles(templateFolderLocation, "*.tasks"))
            {
                log.DebugFormat("Processing: {0}", taskFile);
                var taskSet = new TaskSet();
                using (var textReader = new StreamReader(taskFile))
                using (var jsonReader = new JsonTextReader(textReader))
                {
                    var jsonSerializer = new JsonSerializer();
                    taskSet = jsonSerializer.Deserialize<TaskSet>(jsonReader);
                }

                log.InfoFormat("Set: {0}", taskSet.SetName);
                bool include = true;
                if (taskSet.Optional)
                {
                    log.Warn("TODO: Move to function parameter");
                    Console.Write("Include {0}? (Y/N) ", taskSet.SetName);
                    var userResponse = Console.ReadLine().ToLower();
                    include = (userResponse.IndexOf('y') == 0);
                }

                if (include)
                {
                    var tasks = taskSet.Tasks.Select(x => new Task(x.Description, (TaskPriority)x.Priority, tripInfo.Tags, tripInfo.TripDate.AddDays(x.DayOffset)));
                    result.AddRange(tasks);
                }

            }
            return result;
        }
    }
}
