using System;
using System.Globalization;

namespace GUI_AgentAssignments
{
    /// <summary>
    /// Converts an Agent to a Boolean if that Agent is James Bond
    /// </summary>
    public class AgentToBooleanConverter : BaseValueConverter<AgentToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Agent)?.Equals(new Agent(){CodeName = "James Bond", ID = "007"});
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}