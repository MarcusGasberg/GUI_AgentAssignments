using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using GUI_AgentAssignments.Annotations;

namespace GUI_AgentAssignments
{
    public class AgentViewModel : INotifyPropertyChanged
    {
        private Agent _currentAgent;
        private int _selectedIndex = -1;
        public Agent CurrentAgent
        {
            get => _currentAgent;
            set
            {
                if (_currentAgent == value)
                    return;
                if (value != null)
                {
                    _currentAgent = value;
                }
                else
                {
                    _currentAgent = new Agent();
                }
                OnPropertyChanged(nameof(CurrentAgent));
            }
        }
        public Agents Agents { get; set; }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex == value)
                    return;
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }
        public ICommand PreviousAgentCommand { get; set; }
        public ICommand NextAgentCommand { get; set; }
        public ICommand AddAgentCommand { get; set; }

        public AgentViewModel()
        {
            CurrentAgent = new Agent();
            Agents = new Agents()
            {
                new Agent("007","James Bond","License to kill","Kill Bad Guy"),
                new Agent("001","Nina","Looks","Charm Bad Guy")
            };
            AddAgentCommand = new RelayCommand(()=>AddAgent());
            PreviousAgentCommand = new RelayCommand(()=>PreviousAgent());
            NextAgentCommand = new RelayCommand(()=>NextAgent());
        }

        private void PreviousAgent()
        {
            if (SelectedIndex > -1)
            {
                SelectedIndex--;
            }
        }

        private void NextAgent()
        {
            if (Agents.Count > SelectedIndex)
            {
                SelectedIndex++;
            }
        }

        private void SelectionChanged(object parameter)
        {
            if (SelectedIndex >= 0 && parameter is Agent agent)
                CurrentAgent = agent;
        }

        private void AddAgent()
        {
            var agentToAdd = new Agent()
            {
                ID = CurrentAgent.ID?? "0",
                CodeName = CurrentAgent.CodeName?? "Unknown",
                Speciality = CurrentAgent.Speciality?? "Unknown",
                Assignment = CurrentAgent.Assignment?? "Unknown"
            };
            if (Agents.Any(agent => agentToAdd.CodeName == agent.CodeName &&
                                    agentToAdd.ID == agent.ID))
                return;
            Agents.Add(agentToAdd);
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
