using System;
using Android.OS;

namespace CustomRingAndroid
{
    public class RingServiceBinder : Binder
    {

        
            public RingService Service
            {
                get { return this.service; }
            }

            protected RingService service;

            public bool IsBound { get; set; }

            public RingServiceBinder(RingService service)
            {
                this.service = service;
            }
        
    }
}