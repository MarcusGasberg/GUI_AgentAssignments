using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_AgentAssignments
{
    /// <summary>
    /// Converts a <see cref="ApplicationPage"/> to a <see cref="BasePage{VM}"/>
    /// </summary>
    class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Find the appropriate page
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.AgentPage:
                    return new AgentPage();
                case ApplicationPage.AddAgentPage:
                    return new AddAgentPage();
                case ApplicationPage.EditAgentPage:
                    return new EditAgentPage();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
