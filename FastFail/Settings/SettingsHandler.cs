using BeatSaberMarkupLanguage.Attributes;

namespace FastFail.Settings
{
    class SettingsHandler : PersistentSingleton<SettingsHandler>
    {
        [UIValue("enabled")]
        public bool enabled
        {
            get => Configuration.Instance.enabled;
            set
            {
                Logger.log.Debug($"{value}");
                Configuration.Instance.enabled = value;
            }
        }
        [UIValue("autoSkip")]
        public bool autoSkip
        {
            get => Configuration.Instance.autoSkip;
            set
            {
                Configuration.Instance.autoSkip = value;
            }
        }
    }
}
