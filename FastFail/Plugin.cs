using BeatSaberMarkupLanguage.Settings;
using FastFail.UI;
using IPA;
using IPALogger = IPA.Logging.Logger;
using UnityEngine;

namespace FastFail
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        [Init]
        public void Init(IPALogger logger)
        {
            Logger.log = logger;
        }

        [OnEnable]
        public void OnEnable()
        {
            Config.Read();
            BS_Utils.Utilities.BSEvents.gameSceneLoaded += OnGameSceneLoaded;
            BSMLSettings.instance.AddSettingsMenu("FastFail", "FastFail.UI.settings.bsml", Settings.instance);
        }

        [OnDisable]
        public void OnDisable()
        {
            BS_Utils.Utilities.BSEvents.gameSceneLoaded -= OnGameSceneLoaded;
            BSMLSettings.instance.RemoveSettingsMenu(Settings.instance);
        }


        private void OnGameSceneLoaded()
        {
            if (Config.enabled)
                new GameObject("FailSkip Behavior").AddComponent<FailSkip>();
        }
    }
}
