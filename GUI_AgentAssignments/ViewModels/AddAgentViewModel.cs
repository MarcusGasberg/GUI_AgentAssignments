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
    /// <summary>
    /// ViewModel for the <see cref="AgentPage"/>
    /// </summary>
    public class AddAgentViewModel : BaseViewModel
    {
        #region Private Fields

        private ICommand _addAgentCommand;
        private ICommand _cancelCommand;
        private readonly IEventAggregator _eventAggregator; 
        #endregion
        #region Default Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AddAgentViewModel()
        {
            _eventAggregator = EventAggregatorSingleton.Instance;
            NewAgent = new Agent();
        }
        #endregion
        #region Properties
        public Agent NewAgent { get; set; }
        #endregion
        #region Commands

        public ICommand AddAgentCommand
        {
            get => _addAgentCommand ?? new DelegateCommand(AddAgent);
            private set => _addAgentCommand = value;
        }

        public ICommand CancelCommand
        {
            get => _cancelCommand ?? new DelegateCommand<Window>(w => w.Close());
            private set => _cancelCommand = value;
        }
        #endregion
        #region Private Helpers

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
        #endregion
    }
}
