using System;
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



        protected override void OnCreate(Bundle bundle)
        {
           
            //etTheme(Android.Resource.Style.ThemeTranslucentNoTitleBarFullScreen);
            base.OnCreate(bundle);
            AddShortcut();
            App.Current.RingServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
            };


         

          
        }

        private void AddShortcut()
        {
            var shortcutIntent = new Intent(this.ApplicationContext, typeof (MainActivity));
            shortcutIntent.SetAction(Intent.ActionMain);

            var iconResource = Intent.ShortcutIconResource.FromContext(
                this.ApplicationContext, Resource.Drawable.Icon);

            var intent = new Intent();
            intent.PutExtra(Intent.ExtraShortcutIntent, shortcutIntent);
            intent.PutExtra(Intent.ExtraShortcutName, "My Awesome App!");
            intent.PutExtra(Intent.ExtraShortcutIconResource, iconResource);
            intent.PutExtra("dublicate",false);
            intent.SetAction("com.android.launcher.action.INSTALL_SHORTCUT");
            this.ApplicationContext.SendBroadcast(intent);
        }

      
    }
}