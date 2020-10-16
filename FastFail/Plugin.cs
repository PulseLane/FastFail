using BeatSaberMarkupLanguage.Settings;
using FastFail.Settings;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;

namespace FastFail
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        [Init]
        public void Init(IPALogger logger, Config conf)
        {
            Logger.log = logger;
            Configuration.Instance = conf.Generated<Configuration>();
        }

        [OnEnable]
        public void OnEnable()
        {
            BS_Utils.Utilities.BSEvents.gameSceneLoaded += OnGameSceneLoaded;
            BSMLSettings.instance.AddSettingsMenu("FastFail", "FastFail.Settings.settings.bsml", SettingsHandler.instance);
        }

        [OnDisable]
        public void OnDisable()
        {
            BS_Utils.Utilities.BSEvents.gameSceneLoaded -= OnGameSceneLoaded;
            BSMLSettings.instance.RemoveSettingsMenu(SettingsHandler.instance);
        }

        private void OnGameSceneLoaded()
        {
            if (Configuration.Instance.enabled && !BS_Utils.Plugin.LevelData.Mode.Equals(BS_Utils.Gameplay.Mode.Multiplayer))
            {
                new GameObject("FailSkip Behavior").AddComponent<FailSkip>();
            }
                
        }
    }
}
