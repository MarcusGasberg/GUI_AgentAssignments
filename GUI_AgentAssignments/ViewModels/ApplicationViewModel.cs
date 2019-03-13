using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_AgentAssignments
{ /// <summary>
  /// The application state as a viewmodel
  /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        private ApplicationPage _applicationPage;
        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage
        {
            get =>_applicationPage;
            private set
            {
                if (_applicationPage == value)
                    return;
                _applicationPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        } 

        /// <summary>
        /// Navigate to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        public void GoToPage(ApplicationPage page)
        {
            CurrentPage = page;
            
        }
    }

  public enum ApplicationPage
  {
      AgentPage,
      EditAgentPage,
      AddAgentPage,
  }
}
