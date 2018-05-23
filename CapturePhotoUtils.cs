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
using Android.Media;
using Android.Graphics;
using Android.Systems;
using Android.OS.Storage;
using System.IO;
using Android.Provider;

namespace PandC_Mobile
{
    /**
  * Android internals have been modified to store images in the media folder with 
  * the correct date meta data
  * @author samuelkirton
  */
    public class CapturePhotoUtils
    {

        /**
         * A copy of the Android internals  insertImage method, this method populates the 
         * meta data with DATE_ADDED and DATE_TAKEN. This fixes a common problem where media 
         * that is inserted manually gets saved at the end of the gallery (because date is not populated).
         * @see android.provider.MediaStore.Images.Media#insertImage(ContentResolver, Bitmap, String, String)
         */
        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
        private static readonly DateTime Jan1st1970 = new DateTime
    (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static  String insertImage(ContentResolver cr,
                Bitmap source,
                String title,
                String description)
        {

            ContentValues values = new ContentValues();
            values.Put(MediaStore.Images.ImageColumns.Title, title);
            values.Put(MediaStore.Images.ImageColumns.DisplayName, title);
            values.Put(MediaStore.Images.ImageColumns.Description, description);
            values.Put(MediaStore.Images.ImageColumns.MimeType, "image/jpeg");
            // Add the date meta data to ensure the image is added at the front of the gallery
            values.Put(MediaStore.Images.ImageColumns.DateAdded, CurrentTimeMillis());// System.currentTimeMillis());
            values.Put(MediaStore.Images.ImageColumns.DateTaken, CurrentTimeMillis());

           Android.Net.Uri url = null;
            String stringUrl = null;    /* value to be returned */

            try
            {
                url = cr.Insert(MediaStore.Images.Media.InternalContentUri, values);

                if (source != null)
                {
              System.IO.Stream imageOut = cr.OpenOutputStream(url);
                    try
                    {
                        source.Compress(Bitmap.CompressFormat.Jpeg, 50, imageOut);
                    }
                    finally
                    {
                        imageOut.Close();
                    }

                    long id = ContentUris.ParseId(url);
                    // Wait until MINI_KIND thumbnail is generated.
                    Bitmap miniThumb = MediaStore.Images.Thumbnails.GetThumbnail(cr, id, ThumbnailKind.MiniKind, null);
                    // This is for backward compatibility.
                    storeThumbnail(cr, miniThumb, id, 50F, 50F, 3); //ThumbnailKind id
                }
                else
                {
                    cr.Delete(url, null, null);
                    url = null;
                }
            }
            catch (Exception e)
            {
                if (url != null)
                {
                    cr.Delete(url, null, null);
                    url = null;
                }
            }

            if (url != null)
            {
                stringUrl = url.ToString();
            }

            return stringUrl;
        }

        /**
         * A copy of the Android internals StoreThumbnail method, it used with the insertImage to
         * populate the android.provider.MediaStore.Images.Media#insertImage with all the correct
         * meta data. The StoreThumbnail method is private so it must be duplicated here.
         * @see android.provider.MediaStore.Images.Media (StoreThumbnail private method)
         */
        private static  Bitmap storeThumbnail(
                ContentResolver cr,
                Bitmap source,
                long id,
                float width,
                float height,
                int kind)
        {

            // create the matrix to scale it
            Matrix matrix = new Matrix();

            float scaleX = width / source.Width;
            float scaleY = height / source.Height;

            matrix.SetScale(scaleX, scaleY);

            Bitmap thumb = Bitmap.CreateBitmap(source, 0, 0,
                source.Width,
                source.Height, matrix,
                true
            );

            ContentValues values = new ContentValues(4);
            values.Put(MediaStore.Images.Thumbnails.Kind, kind);
            values.Put(MediaStore.Images.Thumbnails.ImageId, (int)id);
            values.Put(MediaStore.Images.Thumbnails.Height, thumb.Height);
            values.Put(MediaStore.Images.Thumbnails.Width, thumb.Width);

           Android.Net. Uri url = cr.Insert(MediaStore.Images.Thumbnails.InternalContentUri, values);

            try
            {
              System.IO.Stream  thumbOut = cr.OpenOutputStream(url);
                thumb.Compress(Bitmap.CompressFormat.Jpeg, 100, thumbOut);
                thumbOut.Close();
                return thumb;
            }
            catch (FileNotFoundException ex)
            {
                return null;
            }
            catch (IOException ex)
            {
                return null;
            }
        }
    }
}