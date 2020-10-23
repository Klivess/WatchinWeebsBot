using System;
using System.Collections.Generic;
using System.Text;

namespace WatchinWeebsBot
{
    // This is an awful name and I hate to do this but hey, we're doing it. 
    // btw, you should switch over to the generic host builder pattern at some point
    // if you do i'll come in and get rid of this... atrocity.
    public class InMemoryStorage
    {
        public InMemoryStorage()
        {
            IdsToTrace = new HashSet<ulong>();
        }

        public ISet<ulong> IdsToTrace { get; set; }
    }
}
