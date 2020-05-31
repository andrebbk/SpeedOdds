using ToastNotifications;
using ToastNotifications.Core;

namespace SpeedOdds.Notifications.CostumInput
{
    public static class CustomInputExtensions
    {
        public static void ShowCustomInput(this Notifier notifier,
            string message,
            MessageOptions messageOptions = null)
        {
            notifier.Notify(() => new CustomInputNotification(message, message, messageOptions));
        }
    }
}
