﻿using Prism.Commands;
using Prism.Events;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Serialization;
using GUI_AgentAssignments.Annotations;

namespace GUI_AgentAssignments
{
    public class ReceiveAgentsListsEvent : PubSubEvent<Agents> { }

    public class FileHeaderViewModel : BaseViewModel
    {
        #region Private Fields
        private string _filePath = "../../Test";
        private ICommand _exitCommand;
        private ICommand _newFileCommand;
        private ICommand _openFileCommand;
        private ICommand _saveFileCommand;
        private ICommand _saveFileAsCommand;
        private DispatcherTimer _dispatcherTimer;
        private string _applicationTime;
        private readonly IEventAggregator _eventAggregator; 

        #endregion
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public FileHeaderViewModel()
        {
            _eventAggregator = EventAggregatorSingleton.Instance;
            _eventAggregator.GetEvent<ReceiveAgentsListsEvent>().Subscribe((a) => AgentsList = a);
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += (s, e) => ApplicationTime = DateTime.Now.ToLongTimeString();
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            _dispatcherTimer.Start();
        } 
        #endregion
        #region Properties
        /// <summary>
        /// Obeservable Collection containing all the agents 
        /// </summary>
        public Agents AgentsList { get; set; } = new Agents();

        /// <summary>
        /// The filepath for where to save the file
        /// </summary>
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
        /// <summary>
        /// The current time in the application
        /// </summary>
        public string ApplicationTime
        {
            get => _applicationTime;
            set
            {
                if (_applicationTime == value)
                    return;
                _applicationTime = value;
                OnPropertyChanged(nameof(ApplicationTime));
            }
        }
        #endregion
        #region Commands
        /// <summary>
        /// Command to create a new file. 
        /// </summary>
        public ICommand NewFileCommand => _newFileCommand ?? new DelegateCommand(CreateNewAgentsFile);
        /// <summary>
        /// Command to open a new agents file, which calls <see cref="OpenAgentFile"/>
        /// </summary>
        public ICommand OpenFileCommand => _openFileCommand ?? new DelegateCommand(OpenAgentFile);
        /// <summary>
        /// Command for saving a file calling <see cref="SaveFile"/>
        /// </summary>
        public ICommand SaveFileCommand => _saveFileCommand ?? new DelegateCommand(SaveFile);

        /// <summary>
        /// Command for saving a file as a Name, calling <see cref="SaveFileAs"/>
        /// </summary>
        public ICommand SaveFileAsCommand => _saveFileAsCommand ?? new DelegateCommand(SaveFileAs);

        public ICommand ExitAppCommand
        {
            get => _exitCommand ?? new DelegateCommand(
                       Application.Current.MainWindow.Close,
                       () => Application.Current != null && Application.Current.MainWindow != null);
            private set => _exitCommand = value;
        }
        #endregion
        #region Private Helper Function
        /// <summary>
        /// Creates a <see cref="Microsoft.Win32.SaveFileDialog"/> and saves a file as the specified name.
        /// </summary>
        private void SaveFileAs()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = FilePath.Substring(FilePath.LastIndexOf('/') + 1); // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result != true)
                return;
            //TODO: Make the function wait for the AgentsList
            //Request Agents to save to xml files
            _eventAggregator.GetEvent<RequestAgentsListEvent>().Publish(); 
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
        /// <summary>
        /// Gets the agents to save to a file and saves them in XML format.
        /// </summary>
        private void SaveFile()
        {
            //TODO: Make the function wait for the AgentsList
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

        /// <summary>
        /// Function which prompts the user if they wants to create a new file. Then clears the <see cref="AgentsList"/>
        /// </summary>
        public void CreateNewAgentsFile()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to override this file?", "New File Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
                _eventAggregator.GetEvent<ResetAgentsEvent>().Publish();
        }
        /// <summary>
        /// Opens a <see cref="Microsoft.Win32.OpenFileDialog"/> for opening a file and deserializes it
        /// </summary>
        public void OpenAgentFile()
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = FilePath.Substring(FilePath.LastIndexOf('/') + 1); ; // Default file name
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
        #endregion
    }
}