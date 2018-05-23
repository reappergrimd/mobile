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


namespace PandC_Mobile
{
    public class dataconnection
    {

      public static  void InsertInfo(string name, string surname, string telephone, string cell, string email, double lat, double lon,double clat,double clng,string sub,string city)
        {
            MySqlConnection conn = new MySqlConnection("server=vps.p-and-c-ictsolutions.co.za;port=3306;database=mobile;user id=mobile;password=90op[],.;charset=utf8");

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    client_capture.bg = "SQL Connection Open";
                
                    MySqlCommand cmd = new MySqlCommand("insert into userinfo(username,surname,telephone,cell,email,lat,lon,captured,capturedby,clat,clon,suburb,city,company) values (@user,@pass,@telephone,@cell,@email,@lat,@lon,@captured,'"+globals._loggedin_user+"',@clat,@clon,@sub,@town,'"+ globals._loggedin_company+"')", conn);
                    cmd.Parameters.AddWithValue("@user", name);
                    cmd.Parameters.AddWithValue("@pass", surname);
                    cmd.Parameters.AddWithValue("@telephone", telephone);
                    cmd.Parameters.AddWithValue("@cell", cell);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@lat", lat);
                    cmd.Parameters.AddWithValue("@lon", lon);
                    cmd.Parameters.AddWithValue("@captured", DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"));
                    cmd.Parameters.AddWithValue("@clat", clat);
                    cmd.Parameters.AddWithValue("@clon", clng);
                    cmd.Parameters.AddWithValue("@sub", sub);
                    cmd.Parameters.AddWithValue("@town", city);
                 
                   
                    cmd.ExecuteNonQuery();
                client_capture.bg = "Successfully Signup";
                }

            }
            catch (MySqlException ex)
            {
              client_capture.bg =ex.ToString();
            }
            finally
            {

                conn.Close();
            }
        }
        public static void upsdatescann(string name,int is_wind, Java.Util.Date is_Scanned_date,int is_scanned = 0)
        {
            MySqlConnection conn = new MySqlConnection("server=vps.p-and-c-ictsolutions.co.za;port=3306;database=mobile;user id=mobile;password=90op[],.;charset=utf8");

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    

                    Java.Text.SimpleDateFormat sdf =
                         new Java.Text.SimpleDateFormat("yyyy-MM-dd HH:mm:ss");

                    String currentTime = sdf.Format(is_Scanned_date);
                    conn.Open();
                    client_capture.bg = "SQL Connection Open";
//, is_wind_date = @iswinddate,
                    MySqlCommand cmd = new MySqlCommand("update userinfo SET is_wind = @iswind , is_Scanned_date =@scandate ,is_Scanned=@scancomplete where concat(id ,' ',username ,' ' ,surname) = '" + name + "'", conn);
                    cmd.Parameters.AddWithValue("@user", name);
                    cmd.Parameters.AddWithValue("@iswind", Convert.ToInt16(is_wind));
                   // cmd.Parameters.AddWithValue("@iswinddate", DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"));;
                    cmd.Parameters.AddWithValue("@scandate", currentTime);
                    cmd.Parameters.AddWithValue("@scancomplete", Convert.ToInt16(is_scanned));      

                    cmd.ExecuteNonQuery();
                    scan.bg = "Successfully Signup";
                }

            }
            catch (MySqlException ex)
            {
                scan.bg = ex.ToString();
            }
            finally
            {

                conn.Close();
            }
        }
        public static void upsdatesitein(string company, double lat, double lon, Java.Util.Date is_Scanned_date,string comments, int is_scanned)
        {
            MySqlConnection conn = new MySqlConnection("server=vps.p-and-c-ictsolutions.co.za;port=3306;database=mobile;user id=mobile;password=90op[],.;charset=utf8");

            try
            {
                if (conn.State == ConnectionState.Closed)
                {


                    Java.Text.SimpleDateFormat sdf =
                         new Java.Text.SimpleDateFormat("yyyy-MM-dd HH:mm:ss");

                    String currentTime = sdf.Format(is_Scanned_date);
                    conn.Open();
                    client_capture.bg = "SQL Connection Open";
                 
                    MySqlCommand cmd = new MySqlCommand("insert into sla_visits (company, user,cilat,cilon,clockin_note,clockin,clockin_is) values (@company,@user,@lat,@lon,@comments,@datein,@ischeck)", conn);

                    cmd.Parameters.AddWithValue("@company", company);
                    cmd.Parameters.AddWithValue("@user", globals._loggedin_user);
                    cmd.Parameters.AddWithValue("@lat", lat);
                    cmd.Parameters.AddWithValue("@lon", lon); 
                    cmd.Parameters.AddWithValue("@datein", currentTime);
                    cmd.Parameters.AddWithValue("@comments", comments);
                    cmd.Parameters.AddWithValue("@ischeck", Convert.ToInt16(is_scanned));
                    cmd.ExecuteNonQuery();
                   
                }

            }
            catch (MySqlException ex)
            {
                
            }
            finally
            {

                conn.Close();
            }
        }
        public static void upsdatesiteout(string company, double lat, double lon, Java.Util.Date is_Scanned_date, string comments, int is_scanned)
        {
            MySqlConnection conn = new MySqlConnection("server=vps.p-and-c-ictsolutions.co.za;port=3306;database=mobile;user id=mobile;password=90op[],.;charset=utf8");

            try
            {
                if (conn.State == ConnectionState.Closed)
                {


                    Java.Text.SimpleDateFormat sdf =
                         new Java.Text.SimpleDateFormat("yyyy-MM-dd HH:mm:ss");

                    String currentTime = sdf.Format(is_Scanned_date);
                    conn.Open();
                    client_capture.bg = "SQL Connection Open";

                    MySqlCommand cmd = new MySqlCommand("update sla_visits Set colat = @lat , colon = @lon, clockout = @datein , clockout_note = @comments, clockoutis = @ischeck where id = @idd ", conn);


                    cmd.Parameters.AddWithValue("@idd", company);
                    cmd.Parameters.AddWithValue("@lat", lat);
                    cmd.Parameters.AddWithValue("@lon", lon);
                    cmd.Parameters.AddWithValue("@datein", currentTime);
                    cmd.Parameters.AddWithValue("@comments", comments);
                    cmd.Parameters.AddWithValue("@ischeck", Convert.ToInt16(is_scanned));
                    cmd.ExecuteNonQuery();

                }

            }
            catch (MySqlException ex)
            {

            }
            finally
            {

                conn.Close();
            }
        }

    }



    public class NonActivityClass
    {

        public NonActivityClass(Context context)
        {

          
        }


    }
}