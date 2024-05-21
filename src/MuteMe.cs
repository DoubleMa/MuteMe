using DoubleMa.INI.config;
using DoubleMa.Logging;
using DoubleQoL.Game.Patcher;
using System;
using UnityEngine;

namespace muteme {

    public class MuteMe : MonoBehaviour {
        public static Version ModVersion = new Version(1, 0, 0);
        public static string Name => "MuteMe";
        private bool JetPackState = false;
        private bool MinerState = false;

        private void Awake() {
            Log.Info($"Current {Name} version v{ModVersion.ToString(3)}");
            DrillLaserPatcher.Instance.Init();
            changeVolume();
        }

        private void Update() {
            CheckForKeyPresses();
        }

        private void CheckForKeyPresses() {
            if (Input.GetKeyDown(ConfigManager.Instance.KeyCodes_mutejetpack.Value)) {
                CharacterJetpack jetpack = FindObjectOfType<CharacterJetpack>();
                if (jetpack != null) {
                    JetPackState = !JetPackState;
                    jetpack.jetpackAudioEmitter.mute = JetPackState;
                }
            }

            if (Input.GetKeyDown(ConfigManager.Instance.KeyCodes_muteminer.Value)) {
                MinerState = !MinerState;
                DrillLaserPatcher.Instance.setMute(MinerState);
            }
        }

        private void changeVolume() {
            CharacterJetpack jetpack = FindObjectOfType<CharacterJetpack>();
            if (jetpack != null) jetpack.jetpackAudioEmitter.volume = ConfigManager.Instance.setting_jetpack.Value; //need to change this to patch it on every render
        }
    }
}