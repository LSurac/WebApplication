using MediatR;
using WebTimeClock.ApplicationData.Contract.Models.DataModels;

namespace WebTimeClock.ApplicationNotification.Notification
{
    public class ClockOutNotification : INotification
    {
        public EmployeeDataModel Employee { get; set; }
        public TimeClockDataModel TimeClock { get; set; }
    }
}
