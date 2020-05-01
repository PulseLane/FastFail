using BS_Utils;
using IPA;
using IPALogger = IPA.Logging.Logger;
using LogLevel = IPA.Logging.Logger.Level;
using System;
using System.Reflection;
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

            BS_Utils.Utilities.BSEvents.gameSceneLoaded += OnGameSceneLoaded;
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
