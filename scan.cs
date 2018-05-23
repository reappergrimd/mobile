using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using System.Globalization;
using Android.Views;
using System.Data;
using Android.Widget;

using Android.Util;
using Android.Text.Format;

namespace PandC_Mobile
{
    [Activity(Label = "scan", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public  class  scan : Activity
    {
        static string user { get; set; }
        ArrayAdapter<string> adapter;
        TextView timev;
        TextView _dateDisplay;
        CheckBox _yes, _no,scan_yes,scan_no;
        int wind_scanned = 0;
        int scanned_complete = 0;
        public static string bg = "";
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.scann);
            // Create your application here
            FindViewById<Spinner>(Resource.Id.spCustomer).ItemSelected += spinner_ItemSelected;
            Spinner spinner1er = FindViewById<Spinner>(Resource.Id.spCustomer);
           
           
            FindViewById<Button>(Resource.Id.button3).Click += Scan_Click;
            FindViewById<Button>(Resource.Id.button2).Click += Scan_Click1;
            Button button = FindViewById<Button>(Resource.Id.button1);
            timev = FindViewById<TextView>(Resource.Id.textViewtime);
            _yes = FindViewById<CheckBox>(Resource.Id.checkBox1);
            _no = FindViewById<CheckBox>(Resource.Id.checkBox2);
            scan_yes = FindViewById<CheckBox>(Resource.Id.checkBox3);
            scan_no = FindViewById<CheckBox>(Resource.Id.checkBox4);
            _dateDisplay = FindViewById<TextView>(Resource.Id.textView14);
 _yes.CheckedChange += _Yes_CheckedChange;
            _no.CheckedChange += _No_CheckedChange;
            scan_yes.CheckedChange += Scan_yes_CheckedChange;
            scan_no.CheckedChange += Scan_no_CheckedChange;
           await myMethod();

           spinner1er.Adapter = adapter;

            button.Click += delegate
            {
                try
                {
                    Java.Util.Date d = new Java.Text.SimpleDateFormat("yyyy-MM-dd H:mm:ss").Parse(_dateDisplay.Text + " " + timev.Text.TrimEnd('P','A','M',' '));
                    //   Java.Util.Date date = new Java.Util.Date();
                    //   DateFormat dateFormat = Android.Text.Format.DateFormat.Format(,date);
                    // mTimeText.setText("Time: " + dateFormat.format(date));
                    //     var dateb = _dateDisplay.Text + " " + timev.Text;
                    //     DateTime dft = DateTime.ParseExact(dateb, "yyyy-MM-dd H:mm:ss", CultureInfo.InvariantCulture);

                    Toast.MakeText(this, "assumed updated ", ToastLength.Short).Show();

                    dataconnection.upsdatescann(user, wind_scanned, d, scanned_complete);

                    Finish();
                }
                catch (Exception x)
                {
                    Toast.MakeText(this, x.Message, ToastLength.Long).Show();
                }
            };

        }

        private void Scan_no_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                scan_yes.Checked = false;
                scanned_complete = 0;
            }
        }

        private void Scan_yes_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                scan_no.Checked = false;
                scanned_complete = 1;
             
            }
        }
        private void _No_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                _yes.Checked = false;
                wind_scanned = 0;
            }
        }

        private void _Yes_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                _no.Checked = false;
                wind_scanned = 1;

            }
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
            user = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
            try
            {
                List<string[]> d = collectUserdata();
                if (d[0][0].ToString() == "0")
                {
                    _no.Checked = true;
                    _yes.Checked = false;
                }
                else
                {
                    _no.Checked = false;
                    _yes.Checked = true;
                }
                if (d[0][2].ToString() == "0")
                {
                    scan_no.Checked = true;
                    scan_yes.Checked = false;
                }
                else
                {
                    scan_no.Checked = false;
                    scan_yes.Checked = true;
                }
                string[] b = d[0][3].ToString().Split(' ');
                _dateDisplay.Text = (Convert.ToDateTime(b[0]).ToString("yyyy-MM-dd")).ToString();
                timev.Text = b[1].ToString();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
            //  spinner1er.Adapter = adapter;
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
                    string cmd = "select CONCAT (id, ' ', username, ' ' ,surname) from userinfo where is_wind = 0 or is_scanned = 0  and company = '" + globals._loggedin_company + "'";
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

        List<string[]> collectUserdata()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=vps.p-and-c-ictsolutions.co.za;port=3306;database=mobile;user id=mobile;password=90op[],.;charset=utf8"))
                {
                    List<string[]> data = new List<string[]>();
                    string cmd = "select is_wind,is_wind_date,is_Scanned,is_Scanned_date from userinfo where concat(id, ' ', username ,' ' ,surname) = '" + user + "'  and company = '" + globals._loggedin_company + "'";
                    MySqlDataAdapter adapter;
                    DataTable table = new DataTable();
                    adapter = new MySqlDataAdapter(cmd, conn);
                    adapter.Fill(table);

                    foreach (DataRow r in table.Rows)
                    {
                        data.Add(new string[] { r[0].ToString(), r[1].ToString(), r[2].ToString(), r[3].ToString() });

                    }
                    return data;
                }
            }
            catch (MySqlException b)
            {
                List<string[]> data = new List<string[]>();
                data.Add(new string[] { "P and C" });
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