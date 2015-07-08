using System;
using System.Collections.Generic;
using System.Text;

namespace PackingHelper.Cli
{
    internal class Task
    {
        public string Description { get; private set; }
        public TaskPriority Priority { get; private set; }
        public ICollection<string> Tags { get; private set; }
        public DateTime DueDate { get; private set; }

        public Task(string description, TaskPriority priority, ICollection<string> tags, DateTime dueDate)
        {
            Description = description;
            Priority = priority;
            Tags = tags;
            DueDate = dueDate;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append(Description);
            result.AppendFormat(" !{0}", (int)Priority);
            foreach(var tag in Tags)
            {
                result.AppendFormat(" #{0}", tag);
            }
            result.AppendFormat(" ^{0:d}", DueDate);
            return result.ToString();
        }
    }
}
