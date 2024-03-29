# FAQ

#### Why are small Android icons white in the Editor notification settings preview?

Because small notification icons are to be monochrome and Android ignores all non-alpha channels in the icon image, Unity automatically strips all RGB channels. For more information, see external documentation on [Anatomy of a notification](https://material.io/design/platform-guidance/android-notifications.html#anatomy-of-a-notification).

Another way to provide icons is to place them in the `\Assets\Plugins\Android\res\drawable-{scaleFactor}` folder which will not be automatically processed. However, icons which contain non alpha channel will not be correctly displayed on Android 5.0 and above.

The notification color can be modified by [AndroidNotification.Color](../api/Unity.Notifications.Android.AndroidNotification.html#Unity_Notifications_Android_AndroidNotification_Color) property.

#### Why are notifications not delivered on certain phones when my app is closed and not running in the background?

Some devices utilize [aggressive battery saver techniques](https://stackoverflow.com/questions/47145722/how-to-deal-with-huaweis-and-xiaomis-battery-optimizations) which restrict app background activities, unless the app has been whitelisted by the user in device settings. This behavior is observed on devices of certain manufacturers such as Asus, Huawei (including Honor), and Xiaomi. Adjusting battery settings to reduce power consumption might affect more devices.
This means that scheduled notifications will not be delivered if the app is closed or not running in the background. Currently, there's no available workaround besides encouraging the user to whitelist your app.

#### Why is an exception thrown on Android causing notifications to not work as expected?

On Android, the notifications are set up to open an activity when tapped. During initialization, Unity determines the activity in the same way as it does when the notification is received. If Unity can not determine an activity (when more than one activity is active), it throws an exception to indicate that the activity must be explicitly configured in the [notification settings](Settings.md#custom-activity).

#### What can I do if notifications with a location trigger don’t work?

Make sure you've added the `CoreLocation` framework to your Project. For information on how to do this, see documentation on [notification settings](Settings.md#include-corelocation-framework).
Alternatively, you can add the `CoreLocation` framework manually to the Xcode project, or use the Unity Xcode API. You also need to use the [Location Service API](https://docs.unity3d.com/ScriptReference/LocationService.html) to request permission for your app to access location data.
