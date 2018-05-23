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
    [Activity(Label = "onsite_capture", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class onsite_capture : Activity
    {
        static string company { get; set; }
        ArrayAdapter<string> adapter;
        TextView timev;
        TextView _dateDisplay;
        EditText _comments;
        double _local=0.0, _local2=0.0;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.sla_onsite);
            // Create your application here
            FindViewById<Spinner>(Resource.Id.spCustomer).ItemSelected += spinner_ItemSelected;
            Spinner spinner1er = FindViewById<Spinner>(Resource.Id.spCustomer);
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
                        Toast.MakeText(this, "assumed updated ", ToastLength.Short).Show();
                        dataconnection.upsdatesitein(company, _local, _local2, d, _comments.Text, 1);
                        Finish();
                   
                }
                catch (Exception x)
                {
                    Toast.MakeText(this, x.Message, ToastLength.Long).Show();
                }
            };

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
           
        }
        async Task myMethod()
        {

            await Task.Run(() =>
            {
                try
                {
                    adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, collectcompanydata());
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            });

        }
        List<string> collectcompanydata()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=vps.p-and-c-ictsolutions.co.za;port=3306;database=mobile;user id=mobile;password=90op[],.;charset=utf8"))
                {
                    List<string> data = new List<string>();
                    string cmd = "select company_name from sla_customers where contractor = '" + globals._loggedin_company + "'";
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
    public class TimePickerFragment : DialogFragment, TimePickerDialog.IOnTimeSetListener
    {
        public static readonly string TAG = "MyTimePickerFragment";
        Action<DateTime> timeSelectedHandler = delegate { };

        public static TimePickerFragment NewInstance(Action<DateTime> onTimeSelected)
        {
            TimePickerFragment frag = new TimePickerFragment();
            frag.timeSelectedHandler = onTimeSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currentTime = DateTime.Now;
            bool is24HourFormat = DateFormat.Is24HourFormat(Activity);
            TimePickerDialog dialog = new TimePickerDialog
                (Activity, this, currentTime.Hour, currentTime.Minute, is24HourFormat);
            return dialog;
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            DateTime currentTime = DateTime.Now;
            DateTime selectedTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hourOfDay, minute, 0);
            Log.Debug(TAG, selectedTime.ToLongTimeString());
            timeSelectedHandler(selectedTime);
        }
    }
    public class DatePickerFragment : DialogFragment,
                                  DatePickerDialog.IOnDateSetListener
    {
        // TAG can be any string of your choice.
        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

        // Initialize this value to prevent NullReferenceExceptions.
        Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
        {
            DatePickerFragment frag = new DatePickerFragment();
            frag._dateSelectedHandler = onDateSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                           this,
                                                           currently.Year,
                                                           currently.Month - 1,
                                                           currently.Day);
            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            Log.Debug(TAG, selectedDate.ToString("yyyy-MM-dd"));
            _dateSelectedHandler(selectedDate);
        }
    }
}