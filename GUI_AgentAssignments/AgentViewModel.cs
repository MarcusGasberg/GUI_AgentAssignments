using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GUI_AgentAssignments.Annotations;
using Prism.Commands;
using Prism.Events;

namespace GUI_AgentAssignments
{

    public class RequestAgentsListEvent : PubSubEvent { }
    public class UpdateAgentsListsEvent : PubSubEvent<Agents> { }
    public class AgentViewModel : INotifyPropertyChanged
    {
        private Agent _currentAgent;
        private Agents _agents;
        private int _selectedIndex = -1;
        private ICommand _previousAgentCommand;
        private ICommand _nextAgentCommand;
        private ICommand _addAgentCommand;
        private ICommand _deleteAgentCommand;
        private IEventAggregator _eventAggregator;

        public AgentViewModel(IEventAggregator ea)
        {
            _eventAggregator = ea;
            _eventAggregator.GetEvent<RequestAgentsListEvent>().Subscribe(RespondToAgentRequest);
            _eventAggregator.GetEvent<UpdateAgentsListsEvent>().Subscribe((Agents a) => Agents = a);
            CurrentAgent = new Agent();
            Agents = new Agents()
            {
                new Agent("007","James Bond","License to kill","Kill Bad Guy"),
                new Agent("001","Anna Banana","Looks","Charm Bad Guy")
            };
        }

        public Agent CurrentAgent
        {
            get => _currentAgent;
            set
            {
                if (_currentAgent == value)
                    return;
                _currentAgent = value ?? new Agent();
                OnPropertyChanged(nameof(CurrentAgent));
            }
        }

        public Agents Agents
        {
            get => _agents;
            set
            {
                if (_agents == value)
                    return;
                _agents = value ?? new Agents();
                OnPropertyChanged(nameof(Agents));
            }
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex == value)
                    return;
                _selectedIndex = value >= -1 && value < Agents.Count ? value : -1 ;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        public ICommand PreviousAgentCommand
        {
            get => _previousAgentCommand?? new DelegateCommand(PreviousAgent);
            private set => _previousAgentCommand = value;
        }

        public ICommand NextAgentCommand
        {
            get => _nextAgentCommand?? new DelegateCommand(NextAgent);
            private set => _nextAgentCommand = value;
        }

        public ICommand AddAgentCommand
        {
            get => _addAgentCommand?? new DelegateCommand(AddAgent);
            private set => _addAgentCommand = value;
        }

        public ICommand DeleteAgentCommand
        {
            get => _deleteAgentCommand?? new DelegateCommand(DeleteAgent);
            private set => _deleteAgentCommand = value;
        }


        private void DeleteAgent()
        {
            if (SelectedIndex < 0 || SelectedIndex > Agents.Count)
                return;
            Agents.RemoveAt(SelectedIndex);
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

        private void RespondToAgentRequest()
        {
            _eventAggregator.GetEvent<ReceiveAgentsListsEvent>().Publish(Agents);
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
