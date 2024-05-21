using System;
using System.Text.RegularExpressions;

namespace DoubleMa.Extensions {

    internal static class RandomExtention {

        public static int Between(this int x, int min, int max) => Math.Max(min, Math.Min(max, x));

        public static float Between(this float x, float min, float max) => Math.Max(min, Math.Min(max, x));

        public static string CleanString(this string str) => Regex.Replace(str, "[^a-zA-Z0-9]", "");
    }
}