﻿namespace FastFail
{
    class Config
    {
        public static BS_Utils.Utilities.Config config = new BS_Utils.Utilities.Config("FastFail");
        public static bool autoSkip = true;
        public static bool enabled = true;

        public static void Read()
        {
            autoSkip = config.GetBool("FastFail", "autoSkip", true, true);
            enabled = config.GetBool("FastFail", "enabled", true, true);
        }

        public static void Write()
        {
            config.SetBool("FastFail", "autoSkip", autoSkip);
            config.SetBool("FastFail", "enabled", enabled);
        }
    }
}
