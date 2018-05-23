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
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;
using Android.Media;
using Java.Net;
using Android.Graphics;
using Java.IO;
using Android.Graphics.Drawables;
using Android.Util;
using System.Net;
using System.IO;
namespace PandC_Mobile
{
    [Activity(Label = "lostool", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class lostool : Activity
    {
        ArrayAdapter<string> adapter;
        ArrayAdapter<string> hight;
        List<string> chight = new List<string>() { "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
        ImageView imagen = null;
        string _local, _local2;
        string _cust1, _cust2;
        string hs,ch,ah;
        Bitmap imageBitmap;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Lostool);
            imagen = FindViewById<ImageView>(Resource.Id.imageView1);
            TextView street = FindViewById<EditText>(Resource.Id.editText1);
            Button button = FindViewById<Button>(Resource.Id.button1);
            TextView b = FindViewById<TextView>(Resource.Id.textView3);
            TextView bb = FindViewById<TextView>(Resource.Id.textView4);
            b.Text = "";
            bb.Text = "";
            street.TextChanged += Client_capture_TextChanged;
            FindViewById<Spinner>(Resource.Id.spinner1).ItemSelected += spinner_ItemSelected;
            Spinner spinner1er = FindViewById<Spinner>(Resource.Id.spinner1);
            Spinner building = FindViewById<Spinner>(Resource.Id.spinner2);
            FindViewById<Spinner>(Resource.Id.spinner2).ItemSelected += Lostoolspinner_ItemSelected;
            imagen.Click += Imagen_Click;
            await myMethod();

            
            spinner1er.Adapter = adapter;
           
            building.Adapter = hight;

            button.Click += async delegate
            {
                await updater();
                imagen.SetImageBitmap(imageBitmap);
            };
        }

        private void Lostoolspinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            ch = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
        }

        private void Imagen_Click(object sender, EventArgs e)
        {
            CapturePhotoUtils.insertImage(ContentResolver, imageBitmap, FindViewById<EditText>(Resource.Id.editText1).Text, "Wind Generated to " + hs);
            //     PictureShowFragment pic = new PictureShowFragment();


            //     pic.SetBitmap(imageBitmap);
            //     pic.Show(FragmentManager,"view");
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            hs = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
            List<string[]> bs = collecthsdata();

            _local = bs[0][0].ToString().Replace(',', '.');
            _local2 = bs[0][1].ToString().Replace(',', '.');
            ah = bs[0][2].ToString();
            //  spinner1er.Adapter = adapter;
        }
        async Task updater()
        {
            ProgressDialog progress = new ProgressDialog(this);
            progress.SetMessage("Fetching LOS");
            progress.SetTitle("Los Viewer");
            progress.Show();

            await Task.Run(() =>
            {

                imageBitmap = GetImageBitmapFromUrl("http://wind.rbwug.co.za/nodes_plot_linkm.php?clat=" + _cust1 + "&clon=" + _cust2 + "&alat=" + _local + "&alon=" + _local2 + "&ch=" + ch + " &ah=" + ah + "");


            });
            progress.Dismiss();

        }
        async Task myMethod()
        {

            await Task.Run(() =>
            {
                try
                {
                    adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, collectcompanydata());
                    hight = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, chight);
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
                    string cmd = "select username from userinfo where hs = 1 and company = '"+globals._loggedin_company+"'";
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

        List<string[]> collecthsdata()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=vps.p-and-c-ictsolutions.co.za;port=3306;database=mobile;user id=mobile;password=90op[],.;charset=utf8"))
                {
                    List<string[]> data = new List<string[]>();
                    string cmd = "select clat,clon,hs_height from userinfo where username = '" + hs + "' ";
                    MySqlDataAdapter adapter;
                    DataTable table = new DataTable();
                    adapter = new MySqlDataAdapter(cmd, conn);
                    adapter.Fill(table);

                    foreach (DataRow r in table.Rows)
                    {
                        data.Add(new string[] { r[0].ToString(), r[1].ToString(), r[2].ToString() });

                    }
                    return data;
                }
            }
            catch (MySqlException b)
            {
                List<string[]> data = new List<string[]>();
                data.Add(new string[] { "0", "0","15" });
                return data;

            }
        }
        private void Client_capture_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                update_client_gps_via_street(e.Text.ToString());
                TextView b = FindViewById<TextView>(Resource.Id.textView3);
                TextView bb = FindViewById<TextView>(Resource.Id.textView4);
                b.Text = _cust1.ToString();
                bb.Text = _cust2.ToString();

            }
            catch (Exception ex)
            { }
        }
        async void update_client_gps_via_street(string street)
        {
            try
            {

                var fb1 = await Plugin.Geolocator.CrossGeolocator.Current.GetPositionsForAddressAsync(street, "RJHqIE53Onrqons5CNOx~FrDr3XhjDTyEXEjng-CRoA~Aj69MhNManYUKxo6QcwZ0wmXBtyva0zwuHB04rFYAPf7qqGJ5cHb03RCDw1jIW8l");
                var a = fb1.FirstOrDefault();
                _cust1 = a.Latitude.ToString().Replace(',', '.');
                _cust2 = a.Longitude.ToString().Replace(',', '.');

            }
            catch (Exception ex)
            { }

        }
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

    }
    //public class PictureShowFragment : DialogFragment
    //{
    //    private Bitmap _bitmap;

    //    public PictureShowFragment()
    //    {
    //        _bitmap = null;
    //    }

    //    public void SetBitmap(Bitmap bitmap)
    //    {
    //        _bitmap = bitmap;
    //    }


    //    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    //    {
    //        base.OnCreateView(inflater, container, savedInstanceState);

    //        var view = inflater.Inflate(Resource.Layout.pic, container, false);

    //        if (_bitmap == null)
    //            Dismiss();

    //        ImageView pictureImageView = view.FindViewById<ImageView>(Resource.Id.orderpicture_imv_picture);
    //        pictureImageView.SetImageBitmap(_bitmap);



    //        return view;
    //    }

    //}
}