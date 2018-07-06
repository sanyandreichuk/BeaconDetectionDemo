using System.Linq;
using System.Collections.Generic;
using Android.OS;
using Android.App;
using Android.Widget;
using Android.Support.V7.App;
using Org.Altbeacon.Beacon;


namespace BeaconDetectionDemo
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IBeaconConsumer, IRangeNotifier
    {
        private TextView tvDistance;
        private BeaconManager beaconManager;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            tvDistance = FindViewById<TextView>(Resource.Id.tvDistance);

            beaconManager = BeaconManager.GetInstanceForApplication(this);

            //-------------------------
            // To detect proprietary beacons, you must add a line like below corresponding to your beacon
            // type.  Do a web search for "setBeaconLayout" to get the proper expression.
            //var beaconParser = new BeaconParser();
            //beaconParser.SetBeaconLayout("m:2-3=beac,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
            //beaconManager.BeaconParsers.Add(beaconParser);
            //-------------------------

            beaconManager.AddRangeNotifier(this);
            beaconManager.Bind(this);
        }

        protected override void OnDestroy()
        {
            beaconManager.RemoveAllRangeNotifiers();
            beaconManager.Unbind(this);

            base.OnDestroy();
        }

        public void OnBeaconServiceConnect()
        {
            beaconManager.StartRangingBeaconsInRegion(new Region("myRangingRegion", null, null, null));
        }

        public void DidRangeBeaconsInRegion(ICollection<Beacon> p0, Region p1)
        {
            if (!p0.Any())
                return;

            RunOnUiThread(() => {
                tvDistance.Text = p0.First().Distance.ToString("0.00");
            });
        }
    }
}

