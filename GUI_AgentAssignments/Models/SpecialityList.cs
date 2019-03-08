using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_AgentAssignments
{
    public static class SpecialityList
    {

        public static ObservableCollection<string> Collection { get; set; } = new ObservableCollection<string>()
        {
            "None",
            "Assassination",
            "License to kill",
            "Bombs",
            "Low Profile",
            "Seduction",
            "Spy",
            "Martinis"
        };
    }
}
