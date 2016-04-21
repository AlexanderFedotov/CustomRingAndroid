using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Util;

namespace CustomRingAndroid
{
    public class App
    {
        public event EventHandler<ServiceConnectedEventArgs> RingServiceConnected = delegate { };

        protected RingServiceConnection ringServiceConnection;


        // properties

        public static App Current
        {
            get { return current; }
        }
        private static App current;

        public RingService RingService => this.ringServiceConnection.Binder.Service;

        #region Application context

        static App()
        {
            current = new App();
        }
        protected App()
        {
            new Task(() => {

                Android.App.Application.Context.StartService(new Intent(Android.App.Application.Context, typeof(RingService)));

                this.ringServiceConnection= new RingServiceConnection(null);

                this.ringServiceConnection.ServiceConnected += (object sender, ServiceConnectedEventArgs e) => {

                    this.RingServiceConnected(this, e);

                };

                Intent ringServiceIntent = new Intent(Android.App.Application.Context, typeof(RingService));

                Android.App.Application.Context.BindService(ringServiceIntent, ringServiceConnection, Bind.AutoCreate);

            }).Start();
        }

        #endregion

    }
}


