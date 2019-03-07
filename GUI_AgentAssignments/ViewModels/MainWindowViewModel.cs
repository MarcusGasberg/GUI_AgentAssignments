namespace GUI_AgentAssignments
{
    public class MainWindowViewModel
    {
        public FileHeaderViewModel FileHeaderViewModel { get; set; } = new FileHeaderViewModel();
        public ThemesViewModel ThemesViewModel { get; set; } = new ThemesViewModel();
    }
}