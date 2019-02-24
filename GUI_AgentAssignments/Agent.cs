using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GUI_AgentAssignments.Annotations;

namespace GUI_AgentAssignments
{
    public class Agents : ObservableCollection<Agent> { };  // Just to reference it from xaml

    [Serializable]
   public class Agent : INotifyPropertyChanged
   {
      string _id;
      string _codeName;
      string _speciality;
      string _assignment;

      public Agent()
      {
      }

      public Agent(string aId, string aName, string aSpeciality, string aAssignment)
      {
         _id = aId;
         _codeName = aName;
         _speciality = aSpeciality;
         _assignment = aAssignment;
      }

      public string ID
      {
         get => _id;
         set
         {
             if (_id == value)
                 return;
            _id = value;
            OnPropertyChanged(nameof(ID));
         }
      }

      public string CodeName
      {
         get => _codeName;
         set
         {
             if (_codeName == value)
                return;
            _codeName = value;
            OnPropertyChanged(nameof(CodeName));
         }
      }

      public string Speciality
      {
         get => _speciality;
         set
         {
             if (_speciality == value)
                 return;
            _speciality = value;
            OnPropertyChanged(nameof(Speciality));
         }
      }

      public string Assignment
      {
         get => _assignment;
         set
         {
             if (_assignment == value)
                 return;
            _assignment = value;
            OnPropertyChanged(nameof(Assignment));
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
