﻿using DoubleMa.Extensions;
using DoubleMa.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DoubleQoL.Game.Patcher {

    internal abstract class APatcher<P> where P : APatcher<P>, new() {
        private static P instance;
        public static P Instance => GetInstance();

        private static P GetInstance() {
            try { if (instance == null) instance = new P(); }
            catch (Exception e) { Log.Exception(e, e.Message); }
            return instance;
        }

        public string Category { get; }
        public string PatcherID { get; }
        public bool IsActive { get; private set; } = false;
        public abstract bool DefaultState { get; }
        public abstract bool Enabled { get; }

        protected Harmony harmony;
        protected List<MethodToPatch> MethodInfos { get; }

        public static readonly HarmonyMethod PrefixAllow = typeof(APatcher<P>).GetHarmonyMethod(nameof(MyPrefixAllow));
        public static readonly HarmonyMethod PrefixBlock = typeof(APatcher<P>).GetHarmonyMethod(nameof(MyPrefixBlock));
        public static readonly HarmonyMethod PostfixTrue = typeof(APatcher<P>).GetHarmonyMethod(nameof(MyPostfixTrue));
        public static readonly HarmonyMethod PostfixFalse = typeof(APatcher<P>).GetHarmonyMethod(nameof(MyPostfixFalse));
        public static readonly HarmonyMethod PostfixEmpty = typeof(APatcher<P>).GetHarmonyMethod(nameof(MyPostEmpty));

        private static bool MyPrefixAllow() => true;

        private static bool MyPrefixBlock() => false;

        private static void MyPostfixFalse(ref bool __result) => __result = false;

        private static void MyPostfixTrue(ref bool __result) => __result = true;

        private static void MyPostEmpty() {
        }

        public APatcher(string name) {
            Category = $"{name}PatcherCategory";
            PatcherID = $"com.{name.ToLower()}.patch";
            MethodInfos = new List<MethodToPatch>();
        }

        public void Init() {
            harmony = new Harmony(PatcherID);
            OnInit();
            Patch(DefaultState);
        }

        public virtual void OnInit() {
        }

        public void Toggle() => Patch(!IsActive);

        public void Activate() => Patch(true);

        public void Disable() => Patch(false);

        protected virtual void Patch(bool enable = false) {
            if (!Enabled || IsActive == enable) return;
            foreach (var m in MethodInfos) {
                var mt = m?.ToPatch;
                if (mt is null) continue;
                harmony.Unpatch(mt, HarmonyPatchType.All, PatcherID);
                if (enable) harmony.Patch(mt, m.Prefix, m.Postfix);
            }
            IsActive = enable;
        }

        protected void AddMethod(MethodInfo methodInfo, HarmonyMethod prefix, HarmonyMethod postfix) => MethodInfos.Add(new MethodToPatch(methodInfo, prefix, postfix));

        protected void AddMethod(Type t, string method, HarmonyMethod prefix, HarmonyMethod postfix) => AddMethod(t.GetHarmonyMethod(method).method, prefix, postfix);

        protected void AddMethod<T>(string method, HarmonyMethod prefix, HarmonyMethod postfix) => AddMethod(typeof(T), method, prefix, postfix);

        protected void AddMethod(MethodInfo methodInfo, HarmonyMethod postfix, bool allow = false) => AddMethod(methodInfo, allow ? PrefixAllow : PrefixBlock, postfix);

        protected void AddMethod(Type t, string method, HarmonyMethod postfix, bool allow = false) => AddMethod(t.GetHarmonyMethod(method).method, postfix, allow);

        protected void AddMethod<T>(string method, HarmonyMethod postfix, bool allow = false) => AddMethod(typeof(T), method, postfix, allow);

        internal class MethodToPatch {
            public MethodInfo ToPatch { get; }
            public HarmonyMethod Prefix { get; }
            public HarmonyMethod Postfix { get; }

            public MethodToPatch(MethodInfo toPatch, HarmonyMethod prefix, HarmonyMethod postfix) {
                ToPatch = toPatch;
                Prefix = prefix;
                Postfix = postfix;
            }
        }
    }
}