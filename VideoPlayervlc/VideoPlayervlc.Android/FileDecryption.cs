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
using Java.IO;
using Javax.Crypto;
using Javax.Crypto.Spec;
using VideoPlayervlc.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileDecryption))]
namespace VideoPlayervlc.Droid
{
    public class FileDecryption : Decrypt
    {
        private string sKey = "0123456789abcdef";//key，
        private string ivParameter = "1020304050607080";
        public string DecriptFile(string filename, string name)
        {
            Java.IO.File extStore = Android.OS.Environment.GetExternalStoragePublicDirectory("Temp");
            Android.Util.Log.Error("Decryption Started", extStore + "");
            FileInputStream fis = new FileInputStream(extStore + "/" + filename);

            // createFile(filename, extStore);
            FileOutputStream fos = new FileOutputStream(extStore + "/" + "decrypted" + filename, false);

            System.IO.FileStream fs = System.IO.File.OpenWrite(extStore + "/" + name);
            Cipher cipher = Cipher.GetInstance("AES/CBC/PKCS5Padding");
            byte[] raw = System.Text.Encoding.Default.GetBytes(sKey);
            SecretKeySpec skeySpec = new SecretKeySpec(raw, "AES");
            IvParameterSpec iv = new IvParameterSpec(System.Text.Encoding.Default.GetBytes(ivParameter));//
            cipher.Init(CipherMode.DecryptMode, skeySpec, iv);
            CipherOutputStream cos = new CipherOutputStream(fs, cipher);
            int b;
            byte[] d = new byte[1024 * 1024];
            while ((b = fis.Read(d)) != -1)
            {
                cos.Write(d, 0, b);
            }
            System.IO.File.Delete(extStore + "/" + "decrypted" + filename);
            Android.Util.Log.Error("Decryption Ended", extStore + "/" + "decrypted" + name);
            return extStore.ToString();
            //return d;
        }
    }
}