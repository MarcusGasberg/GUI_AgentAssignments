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
using System.Windows.Shapes;

namespace GUI_AgentAssignments
{
    /// <summary>
    /// Interaction logic for AddAgentWindow.xaml
    /// </summary>
    public partial class EditAgentWindow : Window
    {
        public EditAgentWindowViewModel EditAgentWindowViewModel { get; set; }
        public EditAgentWindow(Agent agentToEdit)
        {
            InitializeComponent();
            EditAgentWindowViewModel = new EditAgentWindowViewModel(agentToEdit);
            DataContext = EditAgentWindowViewModel;
        }
    }
}
