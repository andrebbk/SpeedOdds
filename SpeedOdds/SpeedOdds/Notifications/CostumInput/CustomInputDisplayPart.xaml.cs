using ToastNotifications.Core;

namespace SpeedOdds.Notifications.CostumInput
{
    /// <summary>
    /// Interaction logic for CustomInputDisplayPart.xaml
    /// </summary>
    public partial class CustomInputDisplayPart : NotificationDisplayPart
    {
        public CustomInputDisplayPart(CustomInputNotification notification)
        {
            InitializeComponent();
            Bind(notification);
        }
    }
}
