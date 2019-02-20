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
using System.Xml.Serialization;
using GUI_AgentAssignments.Annotations;
using Prism.Commands;
using Prism.Events;

namespace GUI_AgentAssignments
{
    public class ReceiveAgentsListsEvent : PubSubEvent<Agents> { }

    public class FileHeaderViewModel : INotifyPropertyChanged
    {
        private string _fileName = "Test1";
        private ICommand _exitCommand;
        private ICommand _newFileCommand;
        private ICommand _openFileCommand;
        private ICommand _saveFileCommand;
        private ICommand _saveFileAsCommand;
        private IEventAggregator _eventAggregator;

        public FileHeaderViewModel(IEventAggregator ea)
        {
            _eventAggregator = ea;
            _eventAggregator.GetEvent<ReceiveAgentsListsEvent>().Subscribe(CreateNewAgentsFile);
        }

        public AgentViewModel AgentViewModel { get; set; }

        public string FileName
        {
            get => _fileName;
            set
            {
                if (_fileName == value)
                    return;
                _fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }

        public ICommand NewFileCommand
        {
            get => _newFileCommand ?? new DelegateCommand(()=>_eventAggregator.GetEvent<RequestAgentsListEvent>().Publish());
        }

        public ICommand OpenFileCommand { get;}
        public ICommand SaveFileCommand { get; }
        public ICommand SaveFileAsCommand { get; }

        public ICommand ExitAppCommand
        {
            get => _exitCommand ?? new DelegateCommand(
                       Application.Current.MainWindow.Close,
                       () => Application.Current != null && Application.Current.MainWindow != null);
            private set => _exitCommand = value;
        }

        public void CreateNewAgentsFile(Agents agents)
        {
            XmlSerializer xmlSer = new XmlSerializer(typeof(Agents));
            string path = "../../" + FileName; //Set file name to this folder
            if(File.Exists(path))
                throw new ApplicationException("File already exists");
            using (StreamWriter sw = File.CreateText(path))
            {
                xmlSer.Serialize(sw, agents);
            }
            MessageBox.Show($"Created File {FileName} Successfully!");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}