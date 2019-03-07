using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_AgentAssignments
{
    /// <summary>
    /// Locates view models from the IoC for use in binding in XAML files
    /// </summary>
    public class ViewModelLocator
    {
        #region Public Properties
        /// <summary>
        /// Singleton instace of the locator
        /// </summary>
        public static ViewModelLocator Instance { get; } = new ViewModelLocator();

        /// <summary>
        /// The application view model
        /// </summary>
        public static ApplicationViewModel ApplicationViewModel { get; set; } = new ApplicationViewModel();
        #endregion
    }
}
