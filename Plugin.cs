using HarmonyLib;
using IPA;
using IPALogger = IPA.Logging.Logger;
using LogLevel = IPA.Logging.Logger.Level;
using System;
using System.Reflection;

namespace FastFail
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Harmony harmony;

        [Init]
        public void Init(IPALogger logger)
        {
            Logger.log = logger;
        }

        [OnStart]
        public void OnStart()
        {
            harmony = new Harmony("com.PulseLane.BeatSaber.FastFail");
            try
            {
                harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
                Logger.Log("Successfully applied Harmony Patches!");
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to apply harmony patches! {ex}", LogLevel.Error);
            }

        }

        [OnExit]
        public void OnExit()
        {
             harmony.UnpatchAll("com.PulseLane.BeatSaber.FastFail");
        }
    }
}
