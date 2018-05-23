using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using System.Data;
using Android.Widget;
using Android.Util;
using Android.Text.Format;

namespace PandC_Mobile.sla
{
    [Activity(Label = "leave_site", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class leave_site : Activity
    {
        static string company { get; set; }
        string row_id { get; set; }
        ArrayAdapter<string> adapter;
        ArrayAdapter<string> adapter1;
        TextView timev;
        TextView _dateDisplay;
        EditText _comments;
        double _local = 0.0, _local2 = 0.0;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.sla_offsite);
            // Create your application here
            FindViewById<Spinner>(Resource.Id.spCustomer).ItemSelected += spinner_ItemSelected;
            Spinner spinner1er = FindViewById<Spinner>(Resource.Id.spCustomer);
            FindViewById<Spinner>(Resource.Id.spinner1).ItemSelected += Leave_site_ItemSelected;
            Spinner commentsview = FindViewById<Spinner>(Resource.Id.spinner1);
            _comments = FindViewById<EditText>(Resource.Id.editText1);

            FindViewById<Button>(Resource.Id.button3).Click += Scan_Click;
            FindViewById<Button>(Resource.Id.button2).Click += Scan_Click1;
            Button button = FindViewById<Button>(Resource.Id.button1);
            timev = FindViewById<TextView>(Resource.Id.textViewtime);

            _dateDisplay = FindViewById<TextView>(Resource.Id.textView14);

            await myMethod();
            InitializeLocationManagerAsync();
            spinner1er.Adapter = adapter;

            button.Click += delegate
            {
                try
                {
                    if (_local == 0.0)
                    {
                        Thread.Sleep(500);
                    }

                    Java.Util.Date d = new Java.Text.SimpleDateFormat("yyyy-MM-dd H:mm:ss").Parse(_dateDisplay.Text + " " + timev.Text.TrimEnd('P', 'A', 'M', ' '));
                   
                    dataconnection.upsdatesiteout(row_id, _local, _local2, d, _comments.Text, 1);
                    Toast.MakeText(this, "assumed updated ", ToastLength.Short).Show();
                    Finish();

                }
                catch (Exception x)
                {
                    Toast.MakeText(this, x.Message, ToastLength.Long).Show();
                }
            };

        }

        private void Leave_site_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            string value = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
            string[] bp = value.Split(' ');
          //  Toast.MakeText(this, bp[0], ToastLength.Long).Show();
            row_id = bp[0];
        }

        async void InitializeLocationManagerAsync()
        {
            Plugin.Geolocator.Abstractions.Position f = await Plugin.Geolocator.CrossGeolocator.Current.GetPositionAsync();

            _local = f.Latitude;
            _local2 = f.Longitude;


        }
        private void Scan_Click1(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _dateDisplay.Text = time.ToString("yyyy-MM-dd");
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void Scan_Click(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(
        delegate (DateTime time)
        {
            timev.Text = time.ToLongTimeString();
        });

            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            company = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
            Spinner spinner2 = FindViewById<Spinner>(Resource.Id.spinner1);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, collectcompanydata());

            spinner2.Adapter = adapter;
        }
        async Task myMethod()
        {

            await Task.Run(() =>
            {
                try
                {
                    adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, collectcompany());
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            });

        }
      
        List<string> collectcompany()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=vps.p-and-c-ictsolutions.co.za;port=3306;database=mobile;user id=mobile;password=90op[],.;charset=utf8"))
                {
                    List<string> data = new List<string>();
                    string cmd = "select distinct company from sla_visits where user = '" + globals._loggedin_user + "' and clockoutis = 0";
                    MySqlDataAdapter adapter;
                    DataTable table = new DataTable();
                    adapter = new MySqlDataAdapter(cmd, conn);
                    adapter.Fill(table);

                    foreach (DataRow r in table.Rows)
                    {
                        data.Add(r[0].ToString());

                    }
                    return data;
                }
            }
            catch (MySqlException b)
            {
                List<string> data = new List<string>();
                data.Add("P and C");
                return data;

            }
        }
        List<string> collectcompanydata()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=vps.p-and-c-ictsolutions.co.za;port=3306;database=mobile;user id=mobile;password=90op[],.;charset=utf8"))
                {
                    List<string> data = new List<string>();
                    string cmd = "select concat(id, ' ',clockin_note) as Item from sla_visits  where clockoutis !=1 and company = '" + company + "' and user = '" + globals._loggedin_user + "'";
                    MySqlDataAdapter adapter;
                    DataTable table = new DataTable();
                    adapter = new MySqlDataAdapter(cmd, conn);
                    adapter.Fill(table);

                    foreach (DataRow r in table.Rows)
                    {
                        data.Add(r[0].ToString());

                    }
                    return data;
                }
            }
            catch (MySqlException b)
            {
                List<string> data = new List<string>();
                data.Add("P and C");
                return data;

            }
        }


    }
 
    
}