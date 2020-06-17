using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
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

        protected override bool OnBackButtonPressed()
        {

            PlayVideo.MediaPlayer.Stop();
            return (base.OnBackButtonPressed());

        }
    }
}
