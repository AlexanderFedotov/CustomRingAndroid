using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace CustomRingAndroid
{
    [Service]

    public class RingService : Service
    {


        private CallListener callListener;


        public RingService()
        {

        }

     
        IBinder binder;
        public TelephonyManager TM { get; set; }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            //MyPhoneStateListener phoneStateListener = new MyPhoneStateListener();

            //TM = (TelephonyManager)GetSystemService(Context.TelephonyService);

            //TM.Listen(phoneStateListener, PhoneStateListenerFlags.CallState);
            callListener = new CallListener(/*(TelephonyManager)GetSystemService(Context.TelephonyService)*/);

           
            return StartCommandResult.Sticky;
        }

        // This gets called once, the first time any client bind to the Service
        // and returns an instance of the LocationServiceBinder. All future clients will
        // reuse the same instance of the binder
        public override IBinder OnBind(Intent intent)
        {
            binder = new RingServiceBinder(this);
            return binder;
        }
    }

    public class MyPhoneStateListener : PhoneStateListener
    {
        public override void OnCallStateChanged(CallState state, string incomingNumber)
        {
            base.OnCallStateChanged(state, incomingNumber);

            

            switch (state)
            {
                //case TelephonyManager.ExtraStateIdle:
                //    Log.d("DEBUG", "IDLE");
                //    phoneRinging = false;
                //    break;
                //case TelephonyManager.CALL_STATE_OFFHOOK:
                //    Log.d("DEBUG", "OFFHOOK");
                //    phoneRinging = false;
                //    break;
                //case TelephonyManager.CALL_STATE_RINGING:
                //    Log.d("DEBUG", "RINGING");
                //    phoneRinging = true;

                //    break;
                default:
                    break;
            }
        }
    }

    [BroadcastReceiver()]
    [IntentFilter(new[] { "android.intent.action.NEW_OUTGOING_CALL", "android.intent.action.PHONE_STATE", "android.intent.action.PHONE_STATE_CHANGED" })]
    class CallListener : BroadcastReceiver
    {
       
        public CallListener()
        {
            if (delayTimer == null)
            {
                delayTimer = new Timer();
                delayTimer.Elapsed += DelayTimer_Elapsed;
            }


        }

        private void _player_Completion(object sender, EventArgs e)
        {
             delayTimer.Start();
        }

        private void DelayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _player.Start();
            delayTimer.Stop();
            if (tickCounter < 2)
            tickCounter += 1;
            if (tickCounter == 2)
            {
                delayTimer.Interval = 20000;
                tickCounter += 1;
            }
            else
            if (tickCounter > 2)
            delayTimer.Interval += 20000;
            
           
        }


        //public TelephonyManager TM { get; set; }
        static string OldCallState = "";
        static MediaPlayer _player;
        static private Timer playTimer;

        private static int tickCounter = 0;

         static private Timer delayTimer;
        public override void OnReceive(Context context, Intent intent)
        {
            //запустить поток с двумя таймером
            // по алгоритму запукать проигрыватель 
            

            string state = intent.GetStringExtra(TelephonyManager.ExtraState);
            if (state == null)
            {

               
                delayTimer.Interval = 10000;
                _player = MediaPlayer.Create(context, Resource.Raw.Alarm10);
                _player.Completion += _player_Completion;
               
               delayTimer.Stop();
                _player.Start();


            }
            // call ended
            //if (state == TelephonyManager.ActionPhoneStateChanged)
            //{
            //}

            //MyPhoneStateListener phoneStateListener = new MyPhoneStateListener();



            // TM.Listen(phoneStateListener, PhoneStateListenerFlags.CallState);


            if ((OldCallState == TelephonyManager.ExtraStateOffhook) && (state == TelephonyManager.ExtraStateIdle))
                {
                //string PhoneNumer = intent.Extras(TelephonyManager.ExtraIncomingNumber);

                //Toast.MakeText(context, PhoneNumer, ToastLength.Long);
               
               

                }
                OldCallState = state;


            //if (intent.getStringExtra(TelephonyManager.EXTRA_STATE).equals(
            //   TelephonyManager.EXTRA_STATE_RINGING))
            //{

            //    // Phone number 
            //    String incomingNumber = intent.getStringExtra(TelephonyManager.EXTRA_INCOMING_NUMBER);

            //    // Ringing state
            //    // This code will execute when the phone has an incoming call
            //}
            //else if (intent.getStringExtra(TelephonyManager.EXTRA_STATE).equals(
            //      TelephonyManager.EXTRA_STATE_IDLE)
            //      || intent.getStringExtra(TelephonyManager.EXTRA_STATE).equals(
            //              TelephonyManager.EXTRA_STATE_OFFHOOK))
            //{

            //    // This code will execute when the call is answered or disconnected
            //}

        }

        //async Task AccessTheWebAsync()
        //{

            
        //    return ;
        //}
    }
}