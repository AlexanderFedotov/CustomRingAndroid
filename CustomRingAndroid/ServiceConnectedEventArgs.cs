using System;
using Android.OS;

namespace CustomRingAndroid
{
    public class ServiceConnectedEventArgs : EventArgs
    {
        public IBinder Binder { get; set; }
    }
}