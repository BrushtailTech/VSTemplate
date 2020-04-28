﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using EnterpriseApp1.Mobile.Shared;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EnterpriseApp1.Mobile.Android
{
    [Activity(Label = "EnterpriseApp1", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, WindowSoftInputMode = SoftInput.AdjustPan, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);

            Platform.Init(this, bundle);

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}