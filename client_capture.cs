using Android.Content;
using System.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Locations;
using Android.OS;
using Android.Util;
using Android.Widget;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using Android.Views;

namespace PandC_Mobile
{
    [Activity(Label = "New Client Form", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class client_capture : Activity , ILocationListener
    {
        static readonly string TAG = "X:" + typeof(client_capture).Name;
       // int count = 1;

       // private EditText etusername;
       // private EditText etpass;
       // private Button btninsert;
        private TextView tvTips;
       // Location _currentLocation;
       // LocationManager _locationManager;
      //  string _locationProvider;
        TextView _locationText;
        TextView _addressText;
        double _local,_local2;
        double _cust1, _cust2;
        string _suburb, _town;
        string localaddress;

        public async void OnLocationChanged(Location location)
        {
          
        }
        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            Log.Debug(TAG, "{0}, {1}", provider, status);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ClientCapture);
            tvTips = FindViewById<TextView>(Resource.Id.tvTips);
            _locationText = FindViewById<TextView>(Resource.Id.textView2);
            _addressText = FindViewById<TextView>(Resource.Id.textView1);
            // Get our button from the layout resource,
            // and attach an event to it
            TextView customername = FindViewById<EditText>(Resource.Id.editText1);
            TextView customersurname = FindViewById<EditText>(Resource.Id.editText2);
            TextView customerphone = FindViewById<EditText>(Resource.Id.editText3);
            TextView customercell = FindViewById<EditText>(Resource.Id.editText4);
            TextView customeremail = FindViewById<EditText>(Resource.Id.editText5);
            TextView street = FindViewById<EditText>(Resource.Id.editText6);
        //    Button button1 = FindViewById<Button>(Resource.Id.btnofficecall);
            Button button = FindViewById<Button>(Resource.Id.btndetails);
            TextView b = FindViewById<TextView>(Resource.Id.textView8);
            TextView bb = FindViewById<TextView>(Resource.Id.textView9);
            b.Text = "";
            bb.Text = "";
            TextView bbb = FindViewById<TextView>(Resource.Id.textView14);
            bbb.Text = "";
            street.TextChanged += Client_capture_TextChanged;
            
            InitializeLocationManagerAsync();
            button.Click += delegate
            {
                if (street.Text != "")
                {

                    dataconnection.InsertInfo(customername.Text, customersurname.Text, customerphone.Text, customercell.Text, customeremail.Text, _local, _local2, _cust1, _cust2, _suburb, _town);
                    Toast.MakeText(this, bg, ToastLength.Short).Show();
                    customername.Text = "";
                    customersurname.Text = "";
                    customerphone.Text = "";
                    customercell.Text = "";
                    customeremail.Text = "";
                    street.Text = "";
                    bbb.Text = "";
                    b.Text = "";
                    bb.Text = "";
                }
                else Toast.MakeText(this, "You need to Set the Survey Address!", ToastLength.Short).Show();

            };


            //button1.Click += delegate
            //{
            //    update_client_gps_via_street(street.Text);
            //    if (_cust1 != 0)
            //    {
            //        bg = _cust1.ToString() + " " + _cust2.ToString();
            //        Toast.MakeText(this, bg, ToastLength.Long).Show();
            //    }
            //    else
            //    {
            //        update_client_gps_via_street(street.Text);
            //        if (_cust1 != 0)
            //        {
            //            bg = _cust1.ToString() + " " + _cust2.ToString();
            //            Toast.MakeText(this, bg, ToastLength.Long).Show();
            //        }
            //    }
                
                //  var uri = Android.Net.Uri.Parse("tel:+27625018494");
                ////  var intent = new Intent(Intent.ActionCall, uri);
                //   StartActivity(intent);
         //   };
        }

        private void Client_capture_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                update_client_gps_via_street(e.Text.ToString());
                TextView b = FindViewById<TextView>(Resource.Id.textView8);
                TextView bb = FindViewById<TextView>(Resource.Id.textView9);
                b.Text = _cust1.ToString();
                bb.Text = _cust2.ToString();
                TextView bbb = FindViewById<TextView>(Resource.Id.textView14);
                bbb.Text = localaddress.ToString();

            }
            catch (Exception ex)
            { }
        }

        private void Client_capture_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
           
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // set the menu layout on Main Activity  
            MenuInflater.Inflate(Resource.Menu.mainMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menuItem1:
                    {
                        // add your code  
                        return true;
                    }
                case Resource.Id.menuItem2:
                    {
                        //  StartActivity(typeof(SecondActivity));
                        return true;
                    }
                case Resource.Id.menuItem3:
                    {
                        // add your code  
                        return true;
                    }
            }

            return base.OnOptionsItemSelected(item);
        }
        async void InitializeLocationManagerAsync()
        {

            Plugin.Geolocator.Abstractions.Position f = await Plugin.Geolocator.CrossGeolocator.Current.GetPositionAsync();
          
            _local = f.Latitude;
            _local2 = f.Longitude;
         

        }
        async void update_client_gps_via_street(string street)
        {
            try
            {

                var fb1 = await Plugin.Geolocator.CrossGeolocator.Current.GetPositionsForAddressAsync(street, "RJHqIE53Onrqons5CNOx~FrDr3XhjDTyEXEjng-CRoA~Aj69MhNManYUKxo6QcwZ0wmXBtyva0zwuHB04rFYAPf7qqGJ5cHb03RCDw1jIW8l");
            var a = fb1.FirstOrDefault();
            _cust1 =  a.Latitude;
            _cust2 = a.Longitude;
                var fb = await Plugin.Geolocator.CrossGeolocator.Current.GetAddressesForPositionAsync(a, "RJHqIE53Onrqons5CNOx~FrDr3XhjDTyEXEjng-CRoA~Aj69MhNManYUKxo6QcwZ0wmXBtyva0zwuHB04rFYAPf7qqGJ5cHb03RCDw1jIW8l");
                var tet = fb.FirstOrDefault();
                localaddress = $"{tet.SubLocality} , {tet.Locality}";
                _suburb = tet.SubLocality;
                _town = tet.Locality;

            }
            catch (Exception ex)
            { }

        }
        protected override void OnResume()
        {
            base.OnResume();
         
            //Log.Debug(TAG, "Listening for location updates using " + _locationProvider + ".");
        }

        protected override void OnPause()
        {
            base.OnPause();
         
            //Log.Debug(TAG, "No longer listening for location updates.");
        }


        public static string bg { get; set; }
    }
}

