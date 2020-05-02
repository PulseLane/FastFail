using BeatSaberMarkupLanguage.Settings;
using FastFail.UI;
using IPA;
using IPALogger = IPA.Logging.Logger;
using UnityEngine;

namespace FastFail
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        [Init]
        public void Init(IPALogger logger)
        {
            Logger.log = logger;
        }

        [OnStart]
        public void OnStart()
        {
            Config.Read();
            BS_Utils.Utilities.BSEvents.gameSceneLoaded += OnGameSceneLoaded;
            BSMLSettings.instance.AddSettingsMenu("FastFail", "FastFail.UI.settings.bsml", Settings.instance);
        }

        private void OnGameSceneLoaded()
        {
            new GameObject("FailSkip Behavior").AddComponent<FailSkip>();
        }

        [OnExit]
        public void OnExit()
        {
        }
    }
}
