using System;

using Android.Content;
using Android.OS;
using Android.Util;

namespace CustomRingAndroid
{
    public class RingServiceConnection : Java.Lang.Object, IServiceConnection
    {

            public event EventHandler<ServiceConnectedEventArgs> ServiceConnected = delegate { };

            public RingServiceBinder Binder
            {
                get { return this.binder; }
                set { this.binder = value; }
            }
            protected RingServiceBinder binder;

            public RingServiceConnection(RingServiceBinder binder)
            {
                if (binder != null)
                {
                    this.binder = binder;
                }
            }

            public void OnServiceConnected(ComponentName name, IBinder service)
            {
                RingServiceBinder serviceBinder = service as RingServiceBinder;
                if (serviceBinder != null)
                {
                    this.binder = serviceBinder;
                    this.binder.IsBound = true;
                    this.ServiceConnected(this, new ServiceConnectedEventArgs() { Binder = service });

                
                }
            }

            public void OnServiceDisconnected(ComponentName name)
            {
                this.binder.IsBound = false;
            
            }
        }
}
