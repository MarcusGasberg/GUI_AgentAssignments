using System;
using System.Globalization;

namespace GUI_AgentAssignments
{
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