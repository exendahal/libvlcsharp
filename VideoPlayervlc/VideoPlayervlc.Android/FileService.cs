using System.IO;
using Javax.Crypto;
using Javax.Crypto.Spec;
using VideoPlayervlc.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace VideoPlayervlc.Droid
{
    public class FileService : IFileService
    {
        private string sKey = "0123456789abcdef";//key，
        private string ivParameter = "1020304050607080";

        public bool checkFile(string name)
        {
            string documentsPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "Temp");
            Directory.CreateDirectory(documentsPath);
            string filePath = Path.Combine(documentsPath, name);
            return (System.IO.File.Exists(filePath));
        }

        public void SaveFile(string name, Stream data)
        {
            string documentsPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "Temp");
            Directory.CreateDirectory(documentsPath);

            string filePath = Path.Combine(documentsPath, name);           

            // Length is 16 byte
            Cipher cipher = Cipher.GetInstance("AES/CBC/PKCS5Padding");
            byte[] raw = System.Text.Encoding.Default.GetBytes(sKey);
            SecretKeySpec skeySpec = new SecretKeySpec(raw, "AES");
            IvParameterSpec iv = new IvParameterSpec(System.Text.Encoding.Default.GetBytes(ivParameter));//
            cipher.Init(CipherMode.EncryptMode, skeySpec, iv);

            // Wrap the output stream
            CipherInputStream cis = new CipherInputStream(data, cipher);

            byte[] bArray = new byte[1024 * 1024];
            int b;
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                while ((b = cis.Read(bArray)) != -1)
                {
                    fs.Write(bArray, 0, b);
                }
               
            }

        }
    }
}