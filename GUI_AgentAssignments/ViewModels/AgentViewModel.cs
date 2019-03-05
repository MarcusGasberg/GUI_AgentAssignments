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
    public class AddAgentCommand : PubSubEvent<Agent> { }

    public class AgentViewModel : BaseViewModel
    {
        #region Private Fields

        private Agent _currentAgent;
        private Agents _agents;
        private int _selectedIndex = -1;
        private ICommand _previousAgentCommand;
        private ICommand _nextAgentCommand;
        private ICommand _addAgentCommand;
        private ICommand _deleteAgentCommand;
        private ICommand _sortAgentsCommand;
        private ICommand _filterSpecialityCommand;
        private ICommand _editAgentCommand;
        private IEventAggregator _eventAggregator;

        #endregion
        #region Default Constructor
        public AgentViewModel(IEventAggregator ea)
        {
            _eventAggregator = ea;
            _eventAggregator.GetEvent<RequestAgentsListEvent>().Subscribe(RespondToAgentRequest);
            _eventAggregator.GetEvent<UpdateAgentsListsEvent>().Subscribe((Agents a) => Agents = a);
            _eventAggregator.GetEvent<ResetAgentsEvent>().Subscribe(ResetAgents);
            _eventAggregator.GetEvent<AddAgentCommand>().Subscribe(a => Agents.Add(a));
            CurrentAgent = new Agent();

            Agents = new Agents()
            {
                new Agent("007","James Bond","License to kill","Kill Bad Guy"),
                new Agent("001","Anna Banana","Seduction","Charm Bad Guy")
            };
            
        }
        #endregion
        #region Properties



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
                _selectedIndex = value >= -1 && value < Agents.Count ? value : -1;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        #endregion
        #region Commands
        public ICommand FilterSpecialityCommand
        {
            get => _filterSpecialityCommand ?? new DelegateCommand<string>(FilterSpeciality);
            set => _filterSpecialityCommand = value;
        }

        private void FilterSpeciality(string s)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Agents);
            if (s == "None")
            {
                collectionView.Filter = null;
            }
            else
            {
                collectionView.Filter = o => (o as Agent)?.Speciality == s;
            }
        }

        public ICommand EditAgentCommand
        {
            get => _editAgentCommand ?? new DelegateCommand<Agent>(EditAgent);
            private set => _editAgentCommand = value;
        }

        private void EditAgent(Agent obj)
        {
            var window = new EditAgentWindow(CurrentAgent);
            window.Show();
        }

        public ICommand SortAgentsCommand
        {
            get => _sortAgentsCommand ?? new DelegateCommand<string>(SortAgents);
            set => _sortAgentsCommand = value;
        }

        public ICommand PreviousAgentCommand
        {
            get => _previousAgentCommand ?? new DelegateCommand(PreviousAgent);
            private set => _previousAgentCommand = value;
        }


        public ICommand NextAgentCommand
        {
            get => _nextAgentCommand ?? new DelegateCommand(NextAgent);
            private set => _nextAgentCommand = value;
        }

        public ICommand AddAgentCommand
        {
            get => _addAgentCommand ?? new DelegateCommand(OpenAddAgentWindow);
            private set => _addAgentCommand = value;
        }

        public ICommand DeleteAgentCommand
        {
            get => _deleteAgentCommand ?? new DelegateCommand(DeleteAgent);
            private set => _deleteAgentCommand = value;
        }
        #endregion
        #region Private Helpers for Commands
        private void SortAgents(string s)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Agents);
            collectionView?.SortDescriptions.Clear();
            if (s != "None")
            {
                var sortDesc = new SortDescription(s, ListSortDirection.Ascending);
                collectionView?.SortDescriptions.Add(sortDesc);
            }
        }
        private void OpenAddAgentWindow()
        {
            var window = new AddAgentWindow();
            window.Show();

        }

        private void DeleteAgent()
        {
            if (SelectedIndex < 0 || SelectedIndex > Agents.Count)
                return;
            Agents.RemoveAt(SelectedIndex);
        }

        private void ResetAgents()
        {
            SelectedIndex = -1;
            CurrentAgent = new Agent();
            Agents.Clear();
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


        private void RespondToAgentRequest()
        {
            _eventAggregator.GetEvent<ReceiveAgentsListsEvent>().Publish(Agents);
        }
        #endregion
    }
}
