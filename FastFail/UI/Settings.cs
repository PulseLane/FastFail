using BeatSaberMarkupLanguage.Attributes;

namespace FastFail.UI
{
    class Settings : PersistentSingleton<Settings>
    {
        [UIValue("autoSkip")]
        public bool autoSkip
        {
            get => Config.autoSkip;
            set
            {
                Config.autoSkip = value;
                Config.Write();
            }
        }
    }
}
