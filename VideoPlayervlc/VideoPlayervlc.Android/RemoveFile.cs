using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using VideoPlayervlc.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(RemoveFile))]
namespace VideoPlayervlc.Droid
{
    public class RemoveFile : DeleteFile
    {
        public void DeleteTemp(string name)
        {
           string documentsPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "Temp");
            string filePath = Path.Combine(documentsPath, name);
            bool doesExist = System.IO.File.Exists(filePath);
            if (doesExist)
            {
                System.IO.File.Delete(filePath);
            }


        }
    }
}