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
using System.Windows.Data;
using System.Windows.Input;
using GUI_AgentAssignments.Annotations;
using Prism.Commands;
using Prism.Events;

namespace GUI_AgentAssignments
{

    public class RequestAgentsListEvent : PubSubEvent { }
    public class UpdateAgentsListsEvent : PubSubEvent<Agents> { }
    public class ResetAgentsEvent : PubSubEvent { }

    public class AgentViewModel : INotifyPropertyChanged
    {
        private Agent _currentAgent;
        private Agents _agents;
        private int _selectedIndex = -1;
        private ICommand _previousAgentCommand;
        private ICommand _nextAgentCommand;
        private ICommand _addAgentCommand;
        private ICommand _deleteAgentCommand;
        private ICommand _sortAgentsCommand;
        private ICommand _filterSpecialityCommand;
        private IEventAggregator _eventAggregator;

        public AgentViewModel(IEventAggregator ea)
        {
            _eventAggregator = ea;
            _eventAggregator.GetEvent<RequestAgentsListEvent>().Subscribe(RespondToAgentRequest);
            _eventAggregator.GetEvent<UpdateAgentsListsEvent>().Subscribe((Agents a) => Agents = a);
            _eventAggregator.GetEvent<ResetAgentsEvent>().Subscribe(ResetAgents);
            CurrentAgent = new Agent();

            Agents = new Agents()
            {
                new Agent("007","James Bond","License to kill","Kill Bad Guy"),
                new Agent("001","Anna Banana","Seduction","Charm Bad Guy")
            };

            SpecialityList = new ObservableCollection<string>()
            {
                "Assasination",
                "License to kill",
                "Bombs",
                "Low Profile",
                "Seduction",
                "Spy",
                "Martinis"
            };
        }

        public ObservableCollection<string> SpecialityList { get; set; }

        private void ResetAgents()
        {
            SelectedIndex = -1;
            CurrentAgent = new Agent();
            Agents.Clear();
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

        public ICommand FilterSpecialityCommand
        {
            get => _filterSpecialityCommand ?? new DelegateCommand<string>(FilterSpeciality);
            set => _filterSpecialityCommand = value;
        }

        private void FilterSpeciality(string s)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Agents);
            collectionView.Filter = new Predicate<object>( o => (o as Agent)?.Speciality == s);
        }

        public ICommand SortAgentsCommand
        {
            get => _sortAgentsCommand?? new DelegateCommand<object>(SortAgents);
            set => _sortAgentsCommand = value;
        }

        private void SortAgents(object s)
        {
            var sortingFor = (s as ComboBoxItem)?.Content.ToString();
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Agents);
            collectionView?.SortDescriptions.Clear();
            if (sortingFor != "None")
            {
                var sortDesc = new SortDescription(sortingFor, ListSortDirection.Ascending);
                collectionView?.SortDescriptions.Add(sortDesc);
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
