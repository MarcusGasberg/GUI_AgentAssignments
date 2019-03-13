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
    public class AddAgentViewModel : BaseViewModel
    {
        private ICommand _addAgentCommand;
        private ICommand _cancelCommand;
        private readonly IEventAggregator _eventAggregator;
        public AddAgentViewModel()
        {
            _eventAggregator = EventAggregatorSingleton.Instance;
            NewAgent = new Agent();
        }
        public Agent NewAgent { get; set; }

        public ICommand AddAgentCommand
        {
            get => _addAgentCommand ?? new DelegateCommand(AddAgent);
            private set => _addAgentCommand = value;
        }

        public ICommand CancelCommand
        {
            get => _cancelCommand ?? new DelegateCommand(()=>ViewModelLocator.ApplicationViewModel.GoToPage(ApplicationPage.AgentPage));
            private set => _cancelCommand = value;
        }

        private void AddAgent()
        {
            var agentToAdd = new Agent()
            {
                ID = NewAgent.ID ?? "0",
                CodeName = NewAgent.CodeName ?? "Unknown",
                Speciality = NewAgent.Speciality ?? "Unknown",
                Assignment = NewAgent.Assignment ?? "Unknown"
            };

            _eventAggregator.GetEvent<AddAgentCommand>().Publish(agentToAdd);
            ViewModelLocator.ApplicationViewModel.GoToPage(ApplicationPage.AgentPage);
        }
    }
}
