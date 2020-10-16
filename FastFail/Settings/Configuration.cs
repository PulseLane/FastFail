using IPA.Config.Stores;
using System.Runtime.CompilerServices;

namespace FastFail.Settings
{
    public class Configuration
    {
        public static Configuration Instance { get; set; } = null;
        public virtual bool enabled { get; set; } = true;
        public virtual bool autoSkip { get; set; } = true;
    }
}