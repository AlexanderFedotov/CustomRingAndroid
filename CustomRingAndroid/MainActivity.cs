using System;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace CustomRingAndroid
{

    [Activity(Label = "RingService", MainLauncher = true, Icon = "@drawable/Icon", Theme = "@style/Theme.NoDisplay",
        Exported = true)]
    public class MainActivity : Activity
    {
        protected override void OnResume()
        {
            base.OnResume();
            var manager = (ActivityManager) GetSystemService(ActivityService);
            if (manager.GetRunningServices(int.MaxValue).Any(
                service => service.Service.ClassName.Contains("RingService")))
            {
                ExchangeShortcut(Resource.Drawable.Icon_off);
                StopService(new Intent(ApplicationContext, typeof (RingService)));
            }
            else
            {
                ExchangeShortcut(Resource.Drawable.Icon);
                StartService(new Intent(ApplicationContext, typeof (RingService)));
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            AddShortcut(Resource.Drawable.Icon);
            App.Current.RingServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
            };

        }

        private void AddShortcut(int icon)
        {
            Intent myLauncherIntent = new Intent();
            myLauncherIntent.SetClassName("CustomRingAndroid.CustomRingAndroid", typeof (MainActivity).Name);
            myLauncherIntent.AddFlags(ActivityFlags.NewTask);

            var shortcutIntent = new Intent(this.ApplicationContext, typeof (MainActivity));
            shortcutIntent.SetAction(Intent.ActionMain);

            var iconResource = Intent.ShortcutIconResource.FromContext(
                this.ApplicationContext, icon);

            var intent = new Intent();
            intent.PutExtra(Intent.ExtraShortcutIntent, myLauncherIntent);
            intent.PutExtra(Intent.ExtraShortcutName, "CustomRingAndroid");
            intent.PutExtra(Intent.ExtraShortcutIconResource, iconResource);
            intent.PutExtra("dublicate", false);
            intent.SetAction("com.android.launcher.action.INSTALL_SHORTCUT");
            this.ApplicationContext.SendBroadcast(intent);
        }

        private void DelShortcut()
        {
            Intent myLauncherIntent = new Intent();
            myLauncherIntent.SetClassName("CustomRingAndroid.CustomRingAndroid", typeof (MainActivity).Name);
            var shortcutIntent = new Intent(this.ApplicationContext, typeof (MainActivity));
            shortcutIntent.SetAction(Intent.ActionMain);

            // Decorate the shortcut
            Intent delIntent = new Intent();
            delIntent.PutExtra(Intent.ExtraShortcutIntent, myLauncherIntent);
            delIntent.PutExtra(Intent.ExtraShortcutName, "CustomRingAndroid");

            // Inform launcher to remove shortcut
            delIntent.SetAction("com.android.launcher.action.UNINSTALL_SHORTCUT");
            this.ApplicationContext.SendBroadcast(delIntent);
        }

        private void ExchangeShortcut(int icon)
        {
            DelShortcut();
            AddShortcut(icon);
        }
    }
}