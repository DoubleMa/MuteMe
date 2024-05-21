using DoubleMa.INI.Config;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DoubleMa.INI.config {

    internal class ConfigManager {
        public static readonly ConfigManager Instance = new ConfigManager();

        private readonly KeyCode[][] AcceptedKeyCodes = new KeyCode[][]
        {
            new KeyCode[] { KeyCode.None, KeyCode.Backspace, KeyCode.Delete, KeyCode.Tab, KeyCode.Return, KeyCode.Pause, KeyCode.Escape, KeyCode.Space },
            new KeyCode[] { KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.KeypadPeriod, KeyCode.KeypadDivide, KeyCode.KeypadMultiply, KeyCode.KeypadMinus, KeyCode.KeypadPlus, KeyCode.KeypadEnter, KeyCode.KeypadEquals },
            new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.Insert, KeyCode.Home, KeyCode.End, KeyCode.PageUp, KeyCode.PageDown },
            new KeyCode[] { KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4, KeyCode.F5, KeyCode.F6, KeyCode.F7, KeyCode.F8, KeyCode.F9, KeyCode.F10, KeyCode.F11, KeyCode.F12, KeyCode.F13, KeyCode.F14, KeyCode.F15 },
            new KeyCode[] { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 },
            new KeyCode[] { KeyCode.Exclaim, KeyCode.DoubleQuote, KeyCode.Hash, KeyCode.Dollar, KeyCode.Percent, KeyCode.Ampersand, KeyCode.Quote, KeyCode.LeftParen, KeyCode.RightParen, KeyCode.Asterisk, KeyCode.Plus, KeyCode.Comma, KeyCode.Minus, KeyCode.Period, KeyCode.Slash, KeyCode.Colon, KeyCode.Semicolon, KeyCode.Less, KeyCode.Equals, KeyCode.Greater, KeyCode.Question, KeyCode.At },
            new KeyCode[] { KeyCode.LeftBracket, KeyCode.Backslash, KeyCode.RightBracket, KeyCode.Caret, KeyCode.Underscore, KeyCode.BackQuote },
            new KeyCode[] { KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z },
            new KeyCode[] { KeyCode.LeftCurlyBracket, KeyCode.RightCurlyBracket, KeyCode.Tilde, KeyCode.Numlock, KeyCode.CapsLock, KeyCode.ScrollLock },
            new KeyCode[] { KeyCode.RightShift, KeyCode.LeftShift, KeyCode.RightControl, KeyCode.LeftControl, KeyCode.RightAlt, KeyCode.LeftAlt, KeyCode.LeftMeta, KeyCode.LeftCommand, KeyCode.LeftApple, KeyCode.LeftWindows, KeyCode.RightMeta, KeyCode.RightCommand, KeyCode.RightApple, KeyCode.RightWindows },
            new KeyCode[] { KeyCode.AltGr, KeyCode.Help, KeyCode.Print, KeyCode.Break, KeyCode.Menu },
            new KeyCode[] { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2, KeyCode.Mouse3, KeyCode.Mouse4, KeyCode.Mouse5, KeyCode.Mouse6 }
        };

        public XSectionWithComment Settings;
        public XSectionWithComment KeyCodes;
        public XKeyWithComment<float> setting_jetpack { get; }
        public XKeyWithComment<float> setting_miner { get; }
        public XKeyWithComment<KeyCode> KeyCodes_mutejetpack { get; }
        public XKeyWithComment<KeyCode> KeyCodes_muteminer { get; }
        public XKeyWithComment<KeyCode> KeyCodes_Dev { get; }

        private ConfigManager() {
            var loader = ConfigLoader.Instance;
            Settings = new XSectionWithComment(loader, "Settings");
            setting_jetpack = new XKeyWithComment<float>(loader, Settings, "jetpack_volume", new float[] { 0f, 1f }, 0.5f, "Change the volume of the jetpack.\nAccepted values: 0 - 1\nDefault: 0.5\nComing Soon");
            setting_miner = new XKeyWithComment<float>(loader, Settings, "miner_volume", new float[] { 0f, 1f }, 0.5f, "Change the volume of the mine (Drill and Laser).\nAccepted values: 0 - 1\nDefault: 0.5\nComing Soon");
            KeyCodes = new XSectionWithComment(loader, "KeyCodes", GenerateAcceptedKeyCodesComment(AcceptedKeyCodes));
            var fAcceptedKeyCodes = AcceptedKeyCodes.SelectMany(x => x).ToArray();
            KeyCodes_mutejetpack = new XKeyWithComment<KeyCode>(loader, KeyCodes, "mute_jetpack", fAcceptedKeyCodes, KeyCode.Keypad9, "KeyCode to mute or unmute the jetpack.\nDefault: Keypad9");
            KeyCodes_muteminer = new XKeyWithComment<KeyCode>(loader, KeyCodes, "mute_miner", fAcceptedKeyCodes, KeyCode.Keypad8, "KeyCode to mute or unmute the miner (Drill and Laser).\nDefault: Keypad8");
            KeyCodes_Dev = new XKeyWithComment<KeyCode>(loader, KeyCodes, "dev", fAcceptedKeyCodes, KeyCode.F10, "Enable/Disable dev mode (ignore this, it's only for testing)");
            loader.Save();
        }

        private string GenerateAcceptedKeyCodesComment(KeyCode[][] acceptedKeyCodes) {
            var sb = new StringBuilder();
            sb.AppendLine("Set the keycode for each key.");
            sb.AppendLine("Accepted values (it's case-sensitive so make sure to copy paste the key name from this list, otherwise the default value will be selected):");
            sb.AppendLine();
            foreach (var keyGroup in acceptedKeyCodes) sb.AppendLine("\t" + string.Join(", ", keyGroup.Select(k => k.ToString())));
            return sb.ToString();
        }
    }
}