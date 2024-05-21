using muteme;
using System.IO;

namespace DoubleMa.INI.Config {

    internal class ConfigLoader : AINILoader<ConfigLoader> {
        public override string path => Path.Combine(Path.GetFullPath("."), $"Config\\{MuteMe.Name}.ini");

        public ConfigLoader() {
        }
    }
}