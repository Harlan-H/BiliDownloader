using System;
using System.IO;
using System.Media;

namespace BiliDownloader.Services
{
    public class SoundsService
    {
        private readonly string SoundLocalDir = "Sounds/";
        private readonly SoundPlayer? SuccessSound;
        private readonly SoundPlayer? ErrorSound;

        public SoundsService()
        {
            string successPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SoundLocalDir, "success.wav");
            string errorPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SoundLocalDir, "error.wav");
            if(File.Exists(successPath) && File.Exists(errorPath))
            {
                SuccessSound = new SoundPlayer(successPath);
                SuccessSound?.LoadAsync();

                ErrorSound = new SoundPlayer(errorPath);
                ErrorSound?.LoadAsync();
            }

        }

        public void PlaySuccess()
        {
            try
            {
                SuccessSound?.Stop();
                SuccessSound?.PlaySync();
            }
            catch (FileNotFoundException)
            {

            }
        }

        public void PlayError()
        {
            try
            {
                ErrorSound?.Stop();
                ErrorSound?.PlaySync();
            }
            catch (FileNotFoundException)
            {

            }
        }
    }
}
