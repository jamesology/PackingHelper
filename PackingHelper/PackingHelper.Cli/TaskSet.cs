using System.Collections.Generic;

namespace PackingHelper.Cli
{
    internal class TaskSet
    {
        public string SetName { get; set; }
        public bool Optional { get; set; }
        public IList<TaskTemplate> Tasks { get; set; }
    }
}
