using System;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Telephony;

namespace CustomRingAndroid
{
    [Service]
    [IntentFilter(new String[] { "com.RingService" })]
    public class RingService : Service
    {

        private CallListener callListener;
     
        IBinder binder;
       
        public override IBinder OnBind(Intent intent)
        {
            binder = new RingServiceBinder(this);
            return binder;
        }
    }

    [BroadcastReceiver()]
    [IntentFilter(new[] { "android.intent.action.NEW_OUTGOING_CALL",
        "android.intent.action.PHONE_STATE",
        "android.intent.action.PHONE_STATE_CHANGED" })]
    class CallListener : BroadcastReceiver
    {
       
        public CallListener()
        {
            if (_delayTimer == null)
            {
                _delayTimer = new Timer();
                _delayTimer.Elapsed += DelayTimer_Elapsed;
            }
        }

        private void _player_Completion(object sender, EventArgs e)
        {
             _delayTimer.Start();
        }

        private void DelayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _player.Start();
            _delayTimer.Stop();
            if (_tickCounter < 2)
            _tickCounter += 1;
            if (_tickCounter == 2)
            {
                _delayTimer.Interval = 20000;
                _tickCounter += 1;
            }
            else
            if (_tickCounter > 2)
            _delayTimer.Interval += 20000;
            
           
        }

        static string _oldCallState = "";
        static MediaPlayer _player;
        private static int _tickCounter = 0;
        static private Timer _delayTimer;

        public override void OnReceive(Context context, Intent intent)
        {
            string state = intent.GetStringExtra(TelephonyManager.ExtraState);
            if (state == null)
            {
               
                _delayTimer.Interval = 10000;
                _player = MediaPlayer.Create(context, Resource.Raw.Alarm10);
                _player.Completion += _player_Completion;
                _delayTimer.Stop();
                _player.Start();
            }
           

            if ((_oldCallState == TelephonyManager.ExtraStateOffhook) && (state == TelephonyManager.ExtraStateIdle))
                {
                _delayTimer.Stop();
                _player.Stop();
            }
                _oldCallState = state;

        }
       }
}