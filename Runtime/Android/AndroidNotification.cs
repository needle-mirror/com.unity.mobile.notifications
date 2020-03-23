using System;
using UnityEngine;

namespace Unity.Notifications.Android
{
    /// <summary>
    /// Allows applying a rich notification style to a notification.
    /// </summary>
    public enum NotificationStyle
    {
        /// <summary>
        /// Use the default style.
        /// </summary>
        None = 0,

        //// todo currently disabled, bigpicture style requires additional logic that will be implemented in a future release
        ///// <summary>
        ///// generate a large-format notification.
        ///// </summary>
        //bigpicture = 1,

        /// <summary>
        /// Generate a large-format notification that includes a lot of text.
        /// </summary>
        BigTextStyle = 2
    }

    /// <summary>
    /// Allows applying an alert behaviour to grouped notifications.
    /// </summary>
    public enum GroupAlertBehaviours
    {
        /// <summary>
        /// All notifications in a group with sound or vibration will make sound or vibrate, so this notification will not be muted when it is in a group.
        /// </summary>
        GroupAlertAll = 0,

        /// <summary>
        /// The summary notification in a group will be silenced (no sound or vibration) even if they would otherwise make sound or vibrate.
        /// Use this to mute this notification if this notification is a group summary.
        /// </summary>
        GroupAlertSummary = 1,

        /// <summary>
        /// All children notification in a group will be silenced (no sound or vibration) even if they would otherwise make sound or vibrate.
        /// Use this to mute this notification if this notification is a group child. This must be set on all children notifications you want to mute.
        /// </summary>
        GroupAlertChildren = 2,
    }

    /// <summary>
    /// The AndroidNotification is used schedule a local notification, which includes the content of the notification.
    /// </summary>
    public struct AndroidNotification
    {
        /// <summary>
        /// Notification title.
        /// Set the first line of text in the notification.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Notification body.
        /// Set the second line of text in the notification.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Notification small icon.
        /// It will be used to represent the notification in the status bar and content view (unless overridden there by a large icon)
        /// The icon PNG file has to be placed in the `/Assets/Plugins/Android/res/drawable` folder and it's name has to be specified without the extension.
        /// </summary>
        public string SmallIcon
        {
            get { return smallIcon; }
            set { smallIcon = value; }
        }

        private static long DatetimeToLong(DateTime value)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = value.ToUniversalTime() - origin;

            return (long)Math.Floor(diff.TotalMilliseconds);
        }

        private static DateTime LongToDatetime(long value)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddMilliseconds(value).ToLocalTime();
        }

        /// <summary>
        /// The date and time when the notification should be delivered.
        /// </summary>
        public DateTime FireTime
        {
            get { return LongToDatetime(fireTime); }
            set { fireTime = DatetimeToLong(value); }
        }

        /// <summary>
        /// The notification will be be repeated on every specified time interval.
        /// Do not set for one time notifications.
        /// </summary>
        public TimeSpan? RepeatInterval
        {
            get { return TimeSpan.FromMilliseconds(repeatInterval); }
            set
            {
                if (value != null)
                    repeatInterval = (long)value.Value.TotalMilliseconds;
                else
                    repeatInterval = -1L;
            }
        }

        /// <summary>
        /// Notification large icon.
        /// Add a large icon to the notification content view. This image will be shown on the left of the notification view in place of the small icon (which will be placed in a small badge atop the large icon).
        /// The icon PNG file has to be placed in the `/Assets/Plugins/Android/res/drawable folder` and it's name has to be specified without the extension.
        /// </summary>
        public string LargeIcon
        {
            get { return largeIcon; }
            set { largeIcon = value; }
        }

        /// <summary>
        /// Apply a custom style to the notification.
        /// Currently only BigPicture and BigText styles are supported.
        /// </summary>
        public NotificationStyle Style
        {
            get { return (NotificationStyle)style; }
            set { style = (int)value; }
        }

        /// <summary>
        /// Accent color to be applied by the standard style templates when presenting this notification.
        /// The template design constructs a colorful header image by overlaying the icon image (stenciled in white) atop a field of this color. Alpha components are ignored.
        /// </summary>
        public Color? Color
        {
            get
            {
                if (color == 0)
                    return null;

                int a = (color >> 24) & 0xff;
                int r = (color >> 16) & 0xff;
                int g = (color >> 8) & 0xff;
                int b = (color) & 0xff;

                return new Color32((byte)a, (byte)r, (byte)g, (byte)b);
            }
            set
            {
                if (value == null)
                    color = 0;
                else
                {
                    var color32 = (Color32)value.Value;
                    color = (color32.a & 0xff) << 24 | (color32.r & 0xff) << 16 | (color32.g & 0xff) << 8 |
                        (color32.b & 0xff);
                }
            }
        }

        /// <summary>
        /// Sets the number of items this notification represents.
        /// Is displayed as a badge count on the notification icon if the launcher supports this behavior.
        /// </summary>
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        /// <summary>
        /// This notification will automatically be dismissed when the user touches it.
        /// By default this behavior is turned off.
        /// </summary>
        public bool ShouldAutoCancel
        {
            get { return shouldAutoCancel; }
            set { shouldAutoCancel = value; }
        }

        /// <summary>
        /// Show the notification time field as a stopwatch instead of a timestamp.
        /// </summary>
        public bool UsesStopwatch
        {
            get { return usesStopwatch; }
            set { usesStopwatch = value; }
        }

        /// <summary>
        ///Set this property for the notification to be made part of a group of notifications sharing the same key.
        /// Grouped notifications may display in a cluster or stack on devices which support such rendering.
        /// Only available on Android 7.0 (API level 24) and above.
        /// </summary>
        public string Group
        {
            get { return group; }
            set { group = value; }
        }

        /// <summary>
        /// Set this notification to be the group summary for a group of notifications. Requires the 'Group' property to also be set.
        /// Grouped notifications may display in a cluster or stack on devices which support such rendering.
        /// Only available on Android 7.0 (API level 24) and above.
        /// </summary>
        public bool GroupSummary
        {
            get { return groupSummary; }
            set { groupSummary = value; }
        }

        /// <summary>
        /// Sets the group alert behavior for this notification. Set this property to mute this notification if alerts for this notification's group should be handled by a different notification.
        /// This is only applicable for notifications that belong to a group. This must be set on all notifications you want to mute.
        /// Only available on Android 8.0 (API level 26) and above.
        /// </summary>
        public GroupAlertBehaviours GroupAlertBehaviour
        {
            get { return (GroupAlertBehaviours)groupAlertBehaviour; }
            set { groupAlertBehaviour = (int)value; }
        }

        /// <summary>
        /// The sort key will be used to order this notification among other notifications from the same package.
        /// Notifications will be sorted lexicographically using this value.
        /// </summary>
        public string SortKey
        {
            get { return sortKey; }
            set { sortKey = value; }
        }

        /// <summary>
        /// Use this to save arbitrary string data related to the notification.
        /// </summary>
        public string IntentData
        {
            get { return intentData; }
            set { intentData = value; }
        }

        /// <summary>
        /// Enable it to show a timestamp on the notification when it's delivered, unless the "CustomTimestamp" property is set "FireTime" will be shown.
        /// </summary>
        public bool ShowTimestamp
        {
            get { return showTimestamp; }
            set { showTimestamp = value; }
        }

        /// <summary>
        /// Set this to show custom date instead of the notification's "FireTime" as the notification's timestamp'.
        /// </summary>
        public DateTime CustomTimestamp
        {
            get { return LongToDatetime(customTimestamp); }
            set
            {
                showCustomTimestamp = true;
                customTimestamp = DatetimeToLong(value);
            }
        }

        internal string title;
        internal string text;

        internal string smallIcon;
        internal long fireTime;
        internal bool shouldAutoCancel;

        internal string largeIcon;

        internal int style;
        internal int color;

        internal int number;
        internal bool usesStopwatch;
        internal long repeatInterval;

        internal string intentData;

        internal string group;
        internal bool groupSummary;

        internal string sortKey;
        internal int groupAlertBehaviour;

        internal bool showTimestamp;
        internal long customTimestamp;

        internal bool showCustomTimestamp;

        /// <summary>
        /// Create a notification struct with all optional fields set to default values.
        /// </summary>
        public AndroidNotification(String title, String text, DateTime fireTime)
        {
            this.title = title;
            this.text = text;

            repeatInterval = -1;
            smallIcon = "";
            shouldAutoCancel = false;
            largeIcon = "";
            style = (int)NotificationStyle.None;
            color = 0;
            number = -1;
            usesStopwatch = false;
            intentData = "";
            this.fireTime = -1;
            group = "";
            this.groupSummary = false;
            this.sortKey = "";
            this.groupAlertBehaviour = -1;

            customTimestamp = -1;
            showTimestamp = false;
            showCustomTimestamp = false;

            this.FireTime = fireTime;
        }

        /// <summary>
        /// Create a repeatable notification struct with all optional fields set to default values.
        /// </summary>
        /// <remarks>
        /// There is a minimum period of 1 minute for repeating notifications.
        /// </remarks>
        public AndroidNotification(String title, String text, DateTime fireTime, TimeSpan repeatInterval)
            : this(title, text, fireTime)
        {
            this.RepeatInterval = repeatInterval;
        }

        public AndroidNotification(String title, String text, DateTime fireTime, TimeSpan repeatInterval, String smallIcon)
            : this(title, text, fireTime, repeatInterval)
        {
            this.SmallIcon = smallIcon;
        }
    }
}
