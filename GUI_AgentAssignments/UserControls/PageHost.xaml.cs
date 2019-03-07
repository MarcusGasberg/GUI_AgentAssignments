using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI_AgentAssignments
{
    /// <summary>
    /// Interaction logic for PageHost.xaml
    /// </summary>
    public partial class PageHost : UserControl
    {
        #region Dependcy Properties

        /// <summary>
        /// The current page to show in the page host
        /// </summary>
        public BasePage CurrentPage
        {
            get => (BasePage) GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPage"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(BasePage), typeof(PageHost),
                new UIPropertyMetadata(CurrentPagePropertyChanged));

        #endregion

        #region Constructor

        /// <summary>
        /// Default 
        /// Constructor
        /// </summary>
        public PageHost()
        {
            InitializeComponent();
        }

        #endregion

        #region Property Changed Event

        /// <summary>
        /// Called when the <see cref="CurrentPage"/> has changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Get the frames
            var newPageFrame = (d as PageHost).NewPage;
            var oldPageFrame = (d as PageHost).OldPage;

            //Store the current pagee content as the old page
            var oldPageContent = newPageFrame.Content;

            //Remove the current page from new page frame
            newPageFrame.Content = null;

            //Move the previous page into the old page frame
            oldPageFrame.Content = null;
            //Set the new page content
            newPageFrame.Content = e.NewValue;
        }

        #endregion
    }
}