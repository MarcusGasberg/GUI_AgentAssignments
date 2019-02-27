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
using System.Windows.Threading;
using Prism.Events;

namespace GUI_AgentAssignments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public AgentViewModel AgentViewModel { get; set; }
        public FileHeaderViewModel FileHeaderViewModel { get; set; }
        public EventAggregator EventAggregator { get; set; }
        public ThemesViewModel ThemesViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            EventAggregator = new EventAggregator();
            AgentViewModel = new AgentViewModel(EventAggregator);
            FileHeaderViewModel = new FileHeaderViewModel(EventAggregator);
            ThemesViewModel = new ThemesViewModel();
            DataContext = this;
        }
    }
}
