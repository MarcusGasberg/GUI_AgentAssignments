using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GUI_AgentAssignments
{/// <summary>
 /// a base page for all pages to gain base functionality
 /// </summary>
    public class BasePage : Page
    {

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage()
        {
        }
        #endregion
    }

    /// <summary>
    /// a base page with added ViewModel support
    /// </summary>
    public class BasePage<VM> : BasePage
        where VM : BaseViewModel, new()
    {
        #region Private Members
        /// <summary>
        /// Viewmodel associated with this page
        /// </summary>
        private VM mViewModel { get; set; }
        #endregion

        #region Public Properties

        /// <summary>
        /// Viewmodel associated with this page
        /// </summary>
        public VM ViewModel
        {
            get => mViewModel;
            set
            {
                //If nothing has changed return
                if (mViewModel == value)
                    return;
                //update the value
                mViewModel = value;

                //Set the data context for this page
                DataContext = mViewModel;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage() : base()
        {
            //Create a default view model
            ViewModel = new VM();
        }
        #endregion
    }
}
