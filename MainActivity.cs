using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace PandC_Mobile
{
    [Activity(Label = "P and C Mobile", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {

     static   string  company { get; set; }
      static  string  username { get; set; }
        ArrayAdapter<string> adapter;
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            
            FindViewById<Spinner>(Resource.Id.spinner1).ItemSelected += spinner_ItemSelected;
            Spinner spinner1er = FindViewById<Spinner>(Resource.Id.spinner1);
            Spinner spinner2er = FindViewById<Spinner>(Resource.Id.spinner2);
            ProgressBar myProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            FindViewById<Spinner>(Resource.Id.spinner2).ItemSelected += spinner1_ItemSelected;
            myProgressBar.Visibility = Android.Views.ViewStates.Visible;
            await myMethod();
            myProgressBar.Visibility = Android.Views.ViewStates.Gone;
         
             TextView Password = FindViewById<EditText>(Resource.Id.editText1);
            spinner1er.Adapter = adapter;
            Button button = FindViewById<Button>(Resource.Id.button1);
            button.Click += delegate
            {
            List<string> p=    collectdata();
               if( p[0].ToString()==Password.Text.ToString() )
               {
                    globals._loggedin_user = username;
                    globals._loggedin_company = company;
                   StartActivity(typeof(Menu));
                }
                else
                {
                    Toast.MakeText(this,"Incorrect Password",ToastLength.Short);
                    Password.Text ="";
                }

            };
        }
        async Task myMethod()
        {
            await Task.Run(() => {
                adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, collectcompanydata());
            });
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

       
        List<string> collectcompanydata()
        {try
            {
                using (MySqlConnection conn = new MySqlConnection("server=154.127.59.43;port=3306;database=mobile;user id=mobile;password=90op[],.;charset=utf8"))
                  {
               // MySqlConnection conn = new MySqlConnection("server=154.127.59.43;port=3306;database=mobile;user id=mobile;password=90op[],.;charset=utf8;connectiontimeout=500");
                  //  conn.Open();
                    List<string> data = new List<string>();
                    string cmd = "select company from companies";
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
            catch (Exception b)
            {
                List<string> data = new List<string>();
                data.Add("P and C");                
                return data;

            }
        }
        List<string> collectcompanyuserdata()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=154.127.59.43;port=3306;database=mobile;user id=mobile;password=90op[],.;charset=utf8"))
            {
                List<string> data = new List<string>();
                string cmd = "select name from mobile.Users where company = '" + company + "'";
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
        data.Add("Philip");
                return data;

            }
}
        List<string> collectdata()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=154.127.59.43;port=3306;database=mobile;user id=mobile;password=90op[],.;charset=utf8"))
                {
                    List<string> data = new List<string>();
                    string cmd = "select pass from Users where company = '" + company + "' and name = '" + username + "' ";
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
                data.Add("90op[],.");
                return data;

            }
        
        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            company = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
            Spinner spinner1er = FindViewById<Spinner>(Resource.Id.spinner2);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, collectcompanyuserdata());

            spinner1er.Adapter = adapter;
        }

        private void spinner1_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            username = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
          
        }



    }

}