using DoubleMa.Extensions;

namespace DoubleQoL.Game.Patcher {

    internal class DrillLaserPatcher : APatcher<DrillLaserPatcher> {
        public override bool DefaultState => true;
        public override bool Enabled => true;
        private static bool Mute = false;

        public DrillLaserPatcher() : base("DrillLaser") {
            AddMethod(typeof(MiningDrillHH), "_updateBehavoir", this.GetHarmonyMethod(nameof(DrillPostfix)), true);
            AddMethod(typeof(MiningLaserHH), "_updateBehavoir", this.GetHarmonyMethod(nameof(LaserPostfix)), true);
        }

        private static void DrillPostfix(MiningDrillHH __instance) {
            __instance.audioSource_drilling.mute = Mute;
            __instance.audioSource_mouseDown.mute = Mute;
        }

        private static void LaserPostfix(MiningLaserHH __instance) {
            __instance.audioSource_drilling.mute = Mute;
            __instance.audioSource_mouseDown.mute = Mute;
        }

        public void setMute(bool mute) => Mute = mute;
    }
}