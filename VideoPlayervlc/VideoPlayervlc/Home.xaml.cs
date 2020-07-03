using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoPlayervlc
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        private HttpClient _client;
        private int bufferSize = 4095;
        public Home()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            _client = new HttpClient();
            string FileToDownload = "https://file-examples.com/wp-content/uploads/2017/04/file_example_MP4_480_1_5MG.mp4";

            var response1 = await _client.GetAsync(FileToDownload, HttpCompletionOption.ResponseHeadersRead);
            if (!response1.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("The request returned with HTTP status code {0}", response1.StatusCode));
            }
            //var fileName = response1.Content.Headers?.ContentDisposition?.FileName ?? "tmp.mp4";
            var totalData = response1.Content.Headers.ContentLength.GetValueOrDefault(-1L);

            var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (status.ToString() == "Granted")
            {
                Downloadprogress.IsVisible = true;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var stream = await response1.Content.ReadAsStreamAsync())
                    {
                        var totalRead = 0L;
                        var buffer = new byte[bufferSize];
                        double percentage = 0;
                        int read = 0;
                        int received = 0;
                        int total = 0;
                        while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {

                            await ms.WriteAsync(buffer, 0, read);
                            totalRead += read;
                            received = unchecked((int)totalRead);
                            total = unchecked((int)totalData);
                            percentage = ((float)received) / total;
                            await styledProgressBar.ProgressTo(percentage, 750, Easing.Linear);
                            textProgress.Text = Math.Round(percentage, 3) * 100 + " %";
                        }
                        Stream ALLstream = new MemoryStream(ms.ToArray());
                        System.Diagnostics.Debug.WriteLine("Downloaded Complete");
                        DependencyService.Get<IFileService>().SaveFile("VideoName", ALLstream);                       
                        Downloadprogress.IsVisible = false;
                    }
                }

                
            }
            else
            {
                await DisplayAlert("Alert", "Permission Denied", "OK");
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
           Boolean check= DependencyService.Get<IFileService>().checkFile("VideoName");
            if (check)
            {
                await Navigation.PushModalAsync(new MainPage("Local"));
            }
            else
            {
                await DisplayAlert("Alert", "Download Video First", "OK");
            }
           
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage("Remote"));
        }
    }
}