namespace GUI_AgentAssignments
{
    public class MainWindowViewModel
    {
        /// <summary>
        /// ViewModel for the FileHeader in the Window
        /// </summary>
        public FileHeaderViewModel FileHeaderViewModel { get; set; } = new FileHeaderViewModel();
        /// <summary>
        /// The Theme ViewModel for the Window
        /// </summary>
        public ThemesViewModel ThemesViewModel { get; set; } = new ThemesViewModel();
    }
}