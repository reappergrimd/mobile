using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PandC_Mobile
{
    [Activity(Label = "Menu", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Menu : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Menu);
            // Create your application here
                     
            Button button = FindViewById<Button>(Resource.Id.button1);
            Button button1 = FindViewById<Button>(Resource.Id.button2);
            Button lostool = FindViewById<Button>(Resource.Id.button5);
            Button slain = FindViewById<Button>(Resource.Id.button3);
            Button slaout = FindViewById<Button>(Resource.Id.button7);




            button.Click += delegate
            {
                StartActivity(typeof(PandC_Mobile.client_capture));

              //  Toast.MakeText(this, "testc", ToastLength.Short).Show();
                //  var uri = Android.Net.Uri.Parse("tel:+27625018494");
                ////  var intent = new Intent(Intent.ActionCall, uri);
                //   StartActivity(intent);
            };

            button1.Click += delegate
            {
                StartActivity(typeof(PandC_Mobile.scan));

                //  Toast.MakeText(this, "testc", ToastLength.Short).Show();
                //  var uri = Android.Net.Uri.Parse("tel:+27625018494");
                ////  var intent = new Intent(Intent.ActionCall, uri);
                //   StartActivity(intent);
            };
            lostool.Click += delegate
            {
                StartActivity(typeof(PandC_Mobile.lostool));

             
            };
            slain.Click += delegate
            {
                StartActivity(typeof(PandC_Mobile.sla.onsite_capture));


            };
            slaout.Click += delegate
            {
                StartActivity(typeof(PandC_Mobile.sla.leave_site));


            };
        }
    }
    }
