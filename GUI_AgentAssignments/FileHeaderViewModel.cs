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
        private string _fileName = "Test2";
        private ICommand _exitCommand;
        private ICommand _newFileCommand;
        private ICommand _openFileCommand;
        private ICommand _saveFileCommand;
        private ICommand _saveFileAsCommand;
        private IEventAggregator _eventAggregator;

        public FileHeaderViewModel(IEventAggregator ea)
        {
            _eventAggregator = ea;
            _eventAggregator.GetEvent<ReceiveAgentsListsEvent>().Subscribe((a)=>AgentsList = a);
        }

        public Agents AgentsList { get; set; } = new Agents();

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
            throw new NotImplementedException();
        }

        private void SaveFile()
        {
            throw new NotImplementedException();
        }
        public void CreateNewAgentsFile()
        {
            _eventAggregator.GetEvent<RequestAgentsListEvent>().Publish();
            XmlSerializer xmlSer = new XmlSerializer(typeof(Agents));
            string path = "../../" + FileName; //Set file name to this folder
            try
            {
                if (File.Exists(path))
                    throw new ApplicationException("File already exists");
                using (StreamWriter sw = File.CreateText(path))
                {
                    xmlSer.Serialize(sw, AgentsList);
                }
                MessageBox.Show($"Created File {FileName} Successfully!");
            }
            catch (ApplicationException e)
            {
                MessageBox.Show($"{e.Message}");
            }
        }

        public void OpenAgentFile()
        {
            try
            {
                string path = "../../" + FileName;
                if (!File.Exists(path))
                    throw new ApplicationException("No such file exists");
                using (FileStream fs = new FileStream(path,FileMode.Open))
                {
                    var xmlSerializer = new XmlSerializer(typeof(Agents));
                    AgentsList = (Agents) xmlSerializer.Deserialize(fs);
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