using HarmonyLib;
using UnityEngine;

namespace muteme {

    [HarmonyPatch]
    internal class StartPatch {

        [HarmonyPatch(typeof(GameRoot), "Awake"), HarmonyPostfix]
        private static void Awake() {
            GameObject obj = new GameObject("MuteMe");
            obj.transform.parent = GameRoot.getSingleton().transform;
            obj.AddComponent<MuteMe>();
        }
    }
}