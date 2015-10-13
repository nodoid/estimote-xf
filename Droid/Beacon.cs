using System;
using Android.Content;
using EstimoteSdk;
using Java.Util.Concurrent;
using System.Linq;
using estimote.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(BeaconDS))]
namespace estimote.Droid
{
    public class BeaconDS : Java.Lang.Object, BeaconManager.IServiceReadyCallback, IBeacon
    {
        BeaconManager BeaconManager;
        Region BeaconRegion;

        Context context = MainActivity.GlobalActivity.ApplicationContext;

        public void InitialiseBeacon()
        {
            Estimote.Initialize(context, App.Self.APP_ID, App.Self.APP_TOKEN);
        }

        public void StartScanning(int milliSecond = 0)
        {
            BeaconManager = new BeaconManager(context);
            if (CheckForBluetooth())
            {
                BeaconRegion = new Region("rid", App.Self.BEACON_ID);

                BeaconManager.SetBackgroundScanPeriod(TimeUnit.Seconds.ToMillis(milliSecond), 0);

                BeaconManager.Connect(this);

                BeaconManager.Ranging += (object sender, BeaconManager.RangingEventArgs e) =>
                {
                    foreach (var beacon in App.Self.Beacons)
                    {
                        var found = e.Beacons.FirstOrDefault(t => t.MacAddress.ToString() == beacon.MacAddress);
                        var idx = e.Beacons.IndexOf(found);
                        if (found != null)
                        {
                            if (idx < App.Self.Beacons.Count)
                                App.Self.Beacons[idx].Rssi = found.Rssi;
                        }
                    }
                };

                BeaconManager.EnteredRegion += (sender, e) =>
                {
                    if (App.Self.Beacons.Count != 0)
                    {
                        var exists = (from beacon in App.Self.Beacons
                                                        from ebeacon in e.Beacons
                                                        where beacon.Major == ebeacon.Major
                                                        select beacon).FirstOrDefault();
                        //var exists = App.Self.Beacons.FirstOrDefault(t => t.Major == e.Beacons.FirstOrDefault(w => w.Major));
                        if (exists == null)
                            App.Self.Beacons.Add(exists);
                        else
                        {
                            var index = App.Self.Beacons.IndexOf(exists);
                            if (index != -1)
                                App.Self.Beacons[index] = exists;
                        }
                    }
                    else
                    {
                        foreach (var beacon in e.Beacons)
                        {
                            App.Self.Beacons.Add(new BeaconData
                                {
                                    MacAddress = beacon.MacAddress.ToString(),
                                    Major = beacon.Major, MeasuredPower = beacon.MeasuredPower, Minor = beacon.Minor,
                                    Name = beacon.Name, Rssi = beacon.Rssi, ProximityUUID = beacon.ProximityUUID.ToString(),
                                    Region = new RegionData
                                    {
                                        Major = e.Region.Major, Minor = e.Region.Minor, 
                                        Identifier = e.Region.Identifier, ProximityUUID = e.Region.ProximityUUID.ToString()
                                    }
                                });
                        }
                    }
                };

                BeaconManager.ExitedRegion += (sender, e) =>
                {
                    if (App.Self.Beacons.Count != 0)
                    {
                        var exists = (from b in App.Self.Beacons
                                                        let reg = b.Region
                                                        where reg.Identifier == e.Region.Identifier
                                                        select reg).FirstOrDefault();
                        if (exists != null)
                        {
                            var index = App.Self.Beacons.IndexOf(App.Self.Beacons.FirstOrDefault(t => t.Region.Major == e.Region.Major));
                            App.Self.Beacons.RemoveAt(index);
                        }
                    }
                };
            }
        }

        public void StopScanning()
        {
            BeaconManager.StopMonitoring(BeaconRegion);
            BeaconManager.Dispose();
        }

        public void OnServiceReady()
        {
            BeaconManager.StartMonitoring(BeaconRegion);
            BeaconManager.StartRanging(BeaconRegion);
        }

        bool CheckForBluetooth()
        {
            if (!BeaconManager.IsBluetoothEnabled)
            {
                App.Self.BeaconChangedClass.BroadcastIt("Bluetooth has not been enabled");
                return false;
            }
            BeaconManager.Connect(this);   
            return true;
        }
    }
}

