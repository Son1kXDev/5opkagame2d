namespace Enjine.Data.SaveLoadSystem
{
    [System.Serializable]
    public class SettingsData
    {
        public int Quality;
        public float MasterVolume;
        public float MusicVolume;
        public float SFXVolume;
        public float AmbientVolume;
        public float UIVolume;
        public bool VSync;
        public bool Particles;

        public SettingsData()
        {
            this.Quality = 2;
            this.VSync = true;
            this.Particles = true;
            this.MasterVolume = 1;
            this.MusicVolume = 1;
            this.SFXVolume = 1;
            this.AmbientVolume = 1;
            this.UIVolume = 1;
        }

    }
}
