using System;

namespace DoubleMa.Logging {

    public static class Log {
        private const string Prefix = "[MuteMe_Mod]: ";

        private static string addPrefix(this string message) => $"{Prefix}{message}";

        private static void log(object message, Action<string> callback = null) {
            //Console.WriteLine(message?.ToString().addPrefix());
            callback?.Invoke(message?.ToString().addPrefix());
        }

        private static void log(Exception e, object message, Action<Exception> callback = null) {
            //Console.WriteLine(message?.ToString().addPrefix());
            callback?.Invoke(e);
        }

        public static void Info(object message) => log(message, UnityEngine.Debug.Log);

        public static void Warning(object message) => log(message, UnityEngine.Debug.LogWarning);

        public static void Error(object message) => log(message, UnityEngine.Debug.LogError);

        public static void Exception(Exception e, object message) => log(e, message, UnityEngine.Debug.LogException);
    }
}