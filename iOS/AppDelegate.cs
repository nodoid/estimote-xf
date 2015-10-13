using Foundation;
using UIKit;
using Estimote;
using Xamarin.Forms;

namespace estimote.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            //Config.Setup(App.Self.APP_ID, App.Self.APP_TOKEN);
            App.ScreenSize = new Size(UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

            return base.FinishedLaunching(app, options);
        }
    }
}

