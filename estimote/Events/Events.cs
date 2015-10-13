using System;

namespace estimote
{
    public class BeaconChangedEventArgs : EventArgs
    {
        public BeaconChangedEventArgs(string val = "")
        {
            ModuleName = val;
        }

        public readonly string ModuleName;
    }

    public class BeaconChangedEvent
    {
        public event BeaconChangeHandler Change;

        public delegate void BeaconChangeHandler(object s,BeaconChangedEventArgs ea);

        protected void OnChange(object s, BeaconChangedEventArgs e)
        {
            if (Change != null)
                Change(s, e);
        }

        public void BroadcastIt(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var info = new BeaconChangedEventArgs(message);
                OnChange(this, info);
            }
        }
    }
}

