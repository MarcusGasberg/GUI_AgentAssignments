using Prism.Commands;
using System.Windows.Input;

namespace GUI_AgentAssignments
{
    public class ThemesViewModel : BaseViewModel
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
        #region Commands
        public ICommand ChangeColorCommand => _changeColorCommand ?? new DelegateCommand<string>(s => BackgroundBrush = s);


        #endregion
    }
}
