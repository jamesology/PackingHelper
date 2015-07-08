using System;
using System.Collections.Generic;

namespace PackingHelper.Cli
{
    internal class TripInfo
    {
        public ICollection<string> Tags { get; private set; }
        public DateTime TripDate { get; private set; }

        public TripInfo(ICollection<string> tags, DateTime tripDate)
        {
            Tags = tags;
            TripDate = tripDate;
        }
    }
}
