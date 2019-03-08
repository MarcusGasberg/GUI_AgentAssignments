using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace GUI_AgentAssignments
{
    public class EventAggregatorSingleton
    {
        private EventAggregatorSingleton() { }
        public static EventAggregator Instance { get; } = new EventAggregator();
    }
}
