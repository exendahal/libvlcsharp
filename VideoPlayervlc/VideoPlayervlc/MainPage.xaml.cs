using LibVLCSharp.Shared;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace VideoPlayervlc
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        LibVLC libVLC;
        string videoType = "";
        public MainPage(string type)
        {
            InitializeComponent();
            videoType = type;
        }

        protected async override void OnAppearing()
        {
            if (videoType == "Local")
            {               
               string path= DependencyService.Get<Decrypt>().DecriptFile("VideoName", "temp");
                Core.Initialize();
                libVLC = new LibVLC();
                var media = new Media(libVLC, path+"/temp", FromType.FromPath);
                PlayVideo.MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = false };
                await Task.Delay(200);
                PlayVideo.MediaPlayer.Play();
                loading.IsVisible = false;
                PlayVideo.IsVisible = true;
            }
            else
            {
                Core.Initialize();
                libVLC = new LibVLC();
                var media = new Media(libVLC, "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4", FromType.FromLocation);
                await media.Parse(MediaParseOptions.ParseNetwork);
                PlayVideo.MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true };
                PlayVideo.MediaPlayer.Play();
                loading.IsVisible = false;
                PlayVideo.IsVisible = true;
            }
        }

        protected override bool OnBackButtonPressed()
        {

            PlayVideo.MediaPlayer.Stop();
            try { DependencyService.Get<DeleteFile>().DeleteTemp("temp"); } catch { }
            return (base.OnBackButtonPressed());

        }
    }
}
