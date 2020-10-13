using BeatSaberMarkupLanguage.Attributes;

namespace FastFail.UI
{
    class Settings : PersistentSingleton<Settings>
    {
        [UIValue("enabled")]
        public bool enabled
        {
            get => Config.enabled;
            set
            {
                Config.enabled = value;
                Config.Write();
            }
        }
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
