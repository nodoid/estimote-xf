using Foundation;
using UIKit;
using Estimote;
using Xamarin.Forms;
using CoreLocation;
using System;

namespace estimote.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public static AppDelegate Self { get; set; }

        public CLLocationManager LocationManager { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            AppDelegate.Self = this;
            global::Xamarin.Forms.Forms.Init();
            App.ScreenSize = new Size(UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
            LoadApplication(new App());

            LocationManager = new CLLocationManager();
            var iOSVersion = UIDevice.CurrentDevice.SystemVersion.Split('.');
            if (Convert.ToInt32(iOSVersion[0]) >= 8)
            {
                LocationManager.RequestAlwaysAuthorization();
                LocationManager.RequestWhenInUseAuthorization();
            }


            return base.FinishedLaunching(app, options);
        }
    }
}

