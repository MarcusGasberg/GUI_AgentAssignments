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
    public class EditAgentWindowViewModel : BaseViewModel
    {
        private Agent _agentToEdit = new Agent();
        private ICommand _confirmEditCommand;
        private ICommand _cancelCommand;
        private IEventAggregator _eventAggregator;
        public EditAgentWindowViewModel( Agent agentToEdit)
        {
            _eventAggregator = EventAggregatorSingleton.GetInstance();
            PropertyChanged += AgentToEdit_PropertyChanged;
            AgentToEdit = agentToEdit;
        }

        private void AgentToEdit_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AgentToView = new Agent(AgentToEdit.ID, AgentToEdit.CodeName,
                AgentToEdit.Speciality, AgentToEdit.Assignment);
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


        public ICommand ConfirmEditCommand
        {
            get => _confirmEditCommand ?? new DelegateCommand<Window>(AddAgent);
            private set => _confirmEditCommand = value;
        }

        public ICommand CancelCommand
        {
            get => _cancelCommand ?? new DelegateCommand<Window>(w => w.Close());
            private set => _cancelCommand = value;
        }

        private void AddAgent(Window w)
        {
            AgentToEdit.ID = AgentToView.ID;
            AgentToEdit.CodeName = AgentToView.CodeName;
            AgentToEdit.Speciality = AgentToView.Speciality;
            AgentToEdit.Assignment = AgentToView.Assignment;
            w.Close();
        }
    }
}
