using Xamarin.Forms;
using System.Diagnostics;

namespace estimote
{
    public class Beacons : ContentPage
    {
        public Beacons()
        {
            BindingContext = Application.Current;

            var image = new Image
            { 
                Source = ImageSource.FromFile("backdrop.png"),
                Aspect = Aspect.Fill,
            };

            var btnStart = new Button
            {
                WidthRequest = App.ScreenSize.Width / 2,
                Text = "Start scanning",
                BackgroundColor = Color.Gray,
                TextColor = Color.White
            };
            btnStart.Clicked += delegate
            {
                DependencyService.Get<IBeacon>().StartScanning(5);
            };

            var btnStop = new Button
            {
                WidthRequest = App.ScreenSize.Width / 2,
                Text = "Stop scanning",
                BackgroundColor = Color.Gray,
                TextColor = Color.White,
                IsEnabled = App.Self.IsRunning
            };
            btnStop.Clicked += (object sender, System.EventArgs e) =>
            {
                DependencyService.Get<IBeacon>().StopScanning();
            };
            btnStop.SetBinding(Button.IsEnabledProperty, new Binding("IsRunning"));

            var btnStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(0, Device.OS == TargetPlatform.iOS ? 20 : 0),
                Children =
                {
                    btnStart, btnStop
                }
            };

            var listView = new ListView
            {
                HasUnevenRows = true,
                ItemsSource = App.Self.Beacons,
                ItemTemplate = new DataTemplate(typeof(BeaconListViewCell)),
                IsPullToRefreshEnabled = true,
                SeparatorVisibility = SeparatorVisibility.None,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            var stackLayout = new StackLayout
            { 
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    btnStack, listView
                }
            };

            var relativeLayout = new RelativeLayout();

            relativeLayout.Children.Add(image,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => App.ScreenSize.Width),
                Constraint.RelativeToParent((parent) => App.ScreenSize.Height));

            relativeLayout.Children.Add(stackLayout,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => App.ScreenSize.Width),
                Constraint.RelativeToParent((parent) => App.ScreenSize.Height));

            Content = relativeLayout;
        }

        public class BeaconListViewCell : ViewCell
        {
            public BeaconListViewCell()
            {
                var lblUUID = new Label()
                {
                    Text = "lyric",
                    Font = Font.SystemFontOfSize(NamedSize.Default),
                    TextColor = Color.White
                };
                lblUUID.SetBinding(Label.TextProperty, new Binding("ProximityUUID", stringFormat: "UUID = {0}"));

                var lblMajor = new Label()
                {
                    Text = "date",
                    Font = Font.SystemFontOfSize(NamedSize.Small),
                    TextColor = Color.Green,
                    WidthRequest = App.ScreenSize.Width / 2
                };
                lblMajor.SetBinding(Label.TextProperty, new Binding("Major", stringFormat: "Major val = {0}"));

                var lblMinor = new Label()
                {
                    Text = "date",
                    Font = Font.SystemFontOfSize(NamedSize.Small),
                    TextColor = Color.Green,
                    WidthRequest = App.ScreenSize.Width / 2
                };
                lblMinor.SetBinding(Label.TextProperty, new Binding("Minor", stringFormat: "Minor val = {0}"));

                var lblMac = new Label()
                {
                    Text = "date",
                    Font = Font.SystemFontOfSize(NamedSize.Small),
                    TextColor = Color.Lime,
                    WidthRequest = App.ScreenSize.Width / 2
                };
                lblMac.SetBinding(Label.TextProperty, new Binding("MacAddress", stringFormat: "Mac = {0}"));

                var lblRSSI = new Label()
                {
                    Text = "date",
                    TextColor = Color.Lime,
                    WidthRequest = App.ScreenSize.Width / 2,
                    Font = Font.SystemFontOfSize(NamedSize.Small)
                };
                lblRSSI.SetBinding(Label.TextProperty, new Binding("Rssi", stringFormat: "Signal = {0}"));

                var lblProximity = new Label()
                {
                    Text = "date",
                    TextColor = Color.Yellow,
                    Font = Font.SystemFontOfSize(NamedSize.Small)
                };
                lblProximity.SetBinding(Label.TextProperty, new Binding("ProximityDist", stringFormat: "Proximity = {0}"));

                var stackDataTop = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                    {
                        lblMajor, lblMinor
                    }
                };
                var stackDataMiddle = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        lblProximity
                    }
                };
                var stackDataBottom = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                    {
                        lblMac, lblRSSI
                    }
                };

                View = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Padding = new Thickness(12, 8),
                    Children = { lblUUID, stackDataTop, stackDataMiddle, stackDataBottom }
                };
            }
        }
    }
}


