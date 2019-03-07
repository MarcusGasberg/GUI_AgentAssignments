using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GUI_AgentAssignments
{
    public class NoFrameHistoryAttachedProperty : BaseAttachedProperty<NoFrameHistoryAttachedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Get the frame
            var frame = (sender as Frame);

            //Hide navigation bar
            frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

            //Clear history of navigation
            frame.Navigated += (ss, ee) => ((Frame)ss).NavigationService.RemoveBackEntry();
        }
    }
}
