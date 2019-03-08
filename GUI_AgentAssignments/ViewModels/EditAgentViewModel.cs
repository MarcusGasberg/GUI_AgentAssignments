using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;

namespace GUI_AgentAssignments
{
    public class ReceiveCurrentAgentEvent : PubSubEvent<Agent> { }
    public class EditAgentViewModel : BaseViewModel
    {
        private Agent _agentToEdit = new Agent();
        private ICommand _confirmEditCommand;
        private ICommand _cancelCommand;
        private IEventAggregator _eventAggregator;
        public EditAgentViewModel()
        {
            _eventAggregator = EventAggregatorSingleton.Instance;
            PropertyChanged += AgentToEditOnPropertyChanged;
            _eventAggregator.GetEvent<ReceiveCurrentAgentEvent>().Subscribe(a => AgentToEdit = a);
            _eventAggregator.GetEvent<RequestCurrentAgentEvent>().Publish();
        }

        private void AgentToEditOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (AgentToEdit == AgentToView)
                return;
            AgentToView = new Agent(AgentToEdit.ID, AgentToEdit.CodeName, AgentToEdit.Speciality, AgentToEdit.Assignment);
        }

        public Agent AgentToView { get; set; } 

        public Agent AgentToEdit
        {
            get => _agentToEdit;
            set
            {
                if (_agentToEdit == value)
                    return;
                _agentToEdit = value;
                OnPropertyChanged(nameof(AgentToEdit));
            }
        }

        public ICommand ConfirmEditCommand
        {
            get => _confirmEditCommand ?? new DelegateCommand(AddAgent);
            private set => _confirmEditCommand = value;
        }

        public ICommand CancelCommand
        {
            get => _cancelCommand ?? new DelegateCommand(() =>
                       ViewModelLocator.ApplicationViewModel.GoToPage(ApplicationPage.AgentPage));
            private set => _cancelCommand = value;
        }

        private void AddAgent()
        {
            AgentToEdit.ID = AgentToView.ID;
            AgentToEdit.CodeName = AgentToView.CodeName;
            AgentToEdit.Speciality = AgentToView.Speciality;
            AgentToEdit.Assignment = AgentToView.Assignment;
            ViewModelLocator.ApplicationViewModel.GoToPage(ApplicationPage.AgentPage);
        }
    }
}
