using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using GUI_AgentAssignments.Annotations;
using Prism.Commands;

namespace GUI_AgentAssignments
{
    public class ThemesViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private ICommand _changeColorCommand;
        private string _backgroundBrush = "FF0000";
        #endregion
        #region Properties
        public string BackgroundBrush
        {
            get => _backgroundBrush;
            set
            {
                if (value != null && _backgroundBrush == value)
                    return;
                _backgroundBrush = value;
                OnPropertyChanged(nameof(BackgroundBrush));
            }
        }
        #endregion
        #region PropertyChanged

        public ICommand ChangeColorCommand => _changeColorCommand ?? new DelegateCommand<string>(s => BackgroundBrush = s);


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion
    }
}
