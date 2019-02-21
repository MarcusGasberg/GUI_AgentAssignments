using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using GUI_AgentAssignments.Annotations;
using Prism.Commands;
using Prism.Events;

namespace GUI_AgentAssignments
{
    public class ReceiveAgentsListsEvent : PubSubEvent<Agents> { }

    public class FileHeaderViewModel : INotifyPropertyChanged
    {
        private string _filePath = "../../Test";
        private ICommand _exitCommand;
        private ICommand _newFileCommand;
        private ICommand _openFileCommand;
        private ICommand _saveFileCommand;
        private ICommand _saveFileAsCommand;
        private readonly IEventAggregator _eventAggregator;

        public FileHeaderViewModel(IEventAggregator ea)
        {
            _eventAggregator = ea;
            _eventAggregator.GetEvent<ReceiveAgentsListsEvent>().Subscribe((a)=>AgentsList = a);
        }

        public Agents AgentsList { get; set; } = new Agents();

        public string FilePath
        {
            get => _filePath;
            set
            {
                if (_filePath == value)
                    return;
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public ICommand NewFileCommand => _newFileCommand ?? new DelegateCommand(CreateNewAgentsFile);

        public ICommand OpenFileCommand => _openFileCommand?? new DelegateCommand(OpenAgentFile);
        public ICommand SaveFileCommand => _saveFileCommand ?? new DelegateCommand(SaveFile);


        public ICommand SaveFileAsCommand => _saveFileAsCommand ?? new DelegateCommand(SaveFileAs);

        public ICommand ExitAppCommand
        {
            get => _exitCommand ?? new DelegateCommand(
                       Application.Current.MainWindow.Close,
                       () => Application.Current != null && Application.Current.MainWindow != null);
            private set => _exitCommand = value;
        }
        private void SaveFileAs()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = FilePath; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result != true)
                return;

            _eventAggregator.GetEvent<RequestAgentsListEvent>().Publish(); //Request Agents to save to xml file
            FilePath = dlg.FileName;
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Agents));
                using (StreamWriter fs = new StreamWriter(FilePath))
                {
                    xmlSer.Serialize(fs, AgentsList);
                }
            }
            catch (ApplicationException e)
            {
                MessageBox.Show($"{e.Message}");
            }
        }

        private void SaveFile()
        {
            _eventAggregator.GetEvent<RequestAgentsListEvent>().Publish(); //Request Agents to save to xml file
            
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Agents));
                if (!File.Exists(FilePath))
                    throw new ApplicationException("No such file exists");
                using (StreamWriter fs = new StreamWriter(FilePath))
                {
                    xmlSer.Serialize(fs, AgentsList);
                }
                MessageBox.Show($"Saved File {FilePath} Successfully!");
            }
            catch (ApplicationException e)
            {
                MessageBox.Show($"{e.Message}");
            }
        }
        public void CreateNewAgentsFile()
        {
            _eventAggregator.GetEvent<RequestAgentsListEvent>().Publish();
            XmlSerializer xmlSer = new XmlSerializer(typeof(Agents));
            try
            {
                if (File.Exists(FilePath))
                    throw new ApplicationException("File already exists");
                using (StreamWriter sw = File.CreateText(FilePath))
                {
                    xmlSer.Serialize(sw, AgentsList);
                }
                MessageBox.Show($"Created File {FilePath} Successfully!");
            }
            catch (ApplicationException e)
            {
                MessageBox.Show($"{e.Message}");
            }
        }

        public void OpenAgentFile()
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = FilePath; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result != true)
                return;

            FilePath = dlg.FileName;
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Agents));
                using (FileStream fs = new FileStream(FilePath, FileMode.Open))
                {
                    var xmlSerializer = new XmlSerializer(typeof(Agents));
                    AgentsList = (Agents)xmlSerializer.Deserialize(fs);
                    _eventAggregator.GetEvent<UpdateAgentsListsEvent>().Publish(AgentsList);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}