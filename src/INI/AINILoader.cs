using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace DoubleMa.INI {

    internal abstract class AINILoader<X> : IINILoader where X : AINILoader<X>, new() {
        private static X instance;
        public static X Instance => GetInstance();

        private static X GetInstance() {
            try { if (instance == null) instance = new X(); }
            catch (Exception e) { Logging.Log.Exception(e, e.Message); }
            return instance;
        }

        public abstract string path { get; }
        protected readonly Dictionary<string, Dictionary<string, (string Value, string Comment)>> data;

        public AINILoader() {
            data = LoadIni(path);
        }

        private Dictionary<string, Dictionary<string, (string Value, string Comment)>> LoadIni(string path) {
            var data = new Dictionary<string, Dictionary<string, (string Value, string Comment)>>(StringComparer.OrdinalIgnoreCase);
            if (!File.Exists(path)) return data;
            var lines = File.ReadAllLines(path);
            Dictionary<string, (string Value, string Comment)> currentSection = null;
            string currentSectionName;
            foreach (var line in lines) {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine)) continue;
                if (trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#")) continue;
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]")) {
                    currentSectionName = trimmedLine.Substring(1, trimmedLine.Length - 2).Trim();
                    if (!data.ContainsKey(currentSectionName)) {
                        currentSection = new Dictionary<string, (string Value, string Comment)>(StringComparer.OrdinalIgnoreCase);
                        data[currentSectionName] = currentSection;
                    }
                }
                else if (currentSection != null) {
                    var keyValue = trimmedLine.Split(new[] { '=' }, 2);
                    if (keyValue.Length == 2) {
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Trim();
                        currentSection[key] = (value, null);
                    }
                }
            }
            return data;
        }

        public XElement GetSectionOrCreate(XSectionWithComment x) {
            if (!data.ContainsKey(x.Tag)) data[x.Tag] = new Dictionary<string, (string Value, string Comment)>(StringComparer.OrdinalIgnoreCase);
            if (x.Comment != null) data[x.Tag]["_section_comment"] = (null, x.Comment);
            return new XElement(x.Tag);
        }

        public string GetValueOrCreate<T>(XKeyWithComment<T> x) where T : IComparable, IConvertible {
            if (!data.TryGetValue(x.SectionWithComment.Tag, out var sectionData)) {
                sectionData = new Dictionary<string, (string Value, string Comment)>(StringComparer.OrdinalIgnoreCase);
                data[x.SectionWithComment.Tag] = sectionData;
            }
            if (!sectionData.TryGetValue(x.Key, out var value)) {
                value = (x.DefaultValue.ToString(), x.Comment);
                sectionData[x.Key] = value;
            }
            if (x.Comment != null) sectionData[x.Key] = (value.Value, x.Comment);
            return value.Value;
        }

        public void Save() {
            var lines = new List<string>();
            foreach (var section in data) {
                if (section.Value.ContainsKey("_section_comment")) {
                    var comment = section.Value["_section_comment"].Comment;
                    if (!string.IsNullOrEmpty(comment)) AddCommentLines(lines, comment);
                }
                lines.Add($"[{section.Key}]");
                foreach (var keyValue in section.Value) {
                    if (keyValue.Key == "_section_comment") continue;
                    if (!string.IsNullOrEmpty(keyValue.Value.Comment)) AddCommentLines(lines, keyValue.Value.Comment);
                    lines.Add($"{keyValue.Key}={keyValue.Value.Value}");
                }
                lines.Add(string.Empty);
            }
            File.WriteAllLines(path, lines);
        }

        private void AddCommentLines(List<string> lines, string comment) {
            var commentLines = comment.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (var line in commentLines) lines.Add($"# {line}");
        }
    }
}