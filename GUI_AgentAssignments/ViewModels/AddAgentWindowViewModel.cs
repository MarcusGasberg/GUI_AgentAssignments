using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;

namespace GUI_AgentAssignments
{
    public class AddAgentWindowViewModel : BaseViewModel
    {
        private ICommand _addAgentCommand;
        private ICommand _cancelCommand;
        private IEventAggregator _eventAggregator;
        public AddAgentWindowViewModel(IEventAggregator ea)
        {
            _eventAggregator = ea;
            NewAgent = new Agent();
        }
        public Agent NewAgent { get; set; }

        //TODO: Should be changed to keep DRY. It's also in AgentViewModel
        public ObservableCollection<string> SpecialityList { get; set; } = new ObservableCollection<string>()
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


        public ICommand AddAgentCommand
        {
            get => _addAgentCommand ?? new DelegateCommand<Window>(AddAgent);
            private set => _addAgentCommand = value;
        }

        public ICommand CancelCommand
        {
            get => _cancelCommand ?? new DelegateCommand<Window>(w => w.Close());
            private set => _cancelCommand = value;
        }

        private void AddAgent(Window w)
        {
            var agentToAdd = new Agent()
            {
                ID = NewAgent.ID ?? "0",
                CodeName = NewAgent.CodeName ?? "Unknown",
                Speciality = NewAgent.Speciality ?? "Unknown",
                Assignment = NewAgent.Assignment ?? "Unknown"
            };

            _eventAggregator.GetEvent<AddAgentCommand>().Publish(agentToAdd);
            w.Close();
        }
    }
}
