using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Renge.PPB {
    public static class CSVManager {
        private static Dictionary<string, Dictionary<string, string>> data;
        private const string RD = "\r\n";
        private const char CD = ';';
        public static void LoadCSVFromResources(string name) {
            string text = Resources.Load<TextAsset>(name).text;
            var rows = text.Split(new string[] { RD }, System.StringSplitOptions.None).ToList<string>();
            for (int i = 0; i < rows.Count; i++) {
                if (rows[i].Length > 0 && !char.IsLetter(rows[i][0]) && rows[i][0] != ';') {
                    string test = rows[i];
                    rows[i - 1] = rows[i - 1] + "\r\n" + rows[i];
                    rows.RemoveAt(i);
                    i--;
                }
            }

            var document = new string[rows.Count - 1][];
            for (int rowIndex = 0; rowIndex < rows.Count - 1; rowIndex++) {
                string row = rows[rowIndex];
                var columns = new List<string>();
                for (int i = 0; i < row.Length; i++) {
                    switch (row[i]) {
                        case CD:
                            columns.Add(row.Substring(0, i));
                            row = row.Remove(0, i + 1);
                            while (row.Length > 0 && row[0] == '"') {
                                int nextIndex = row.IndexOf($"\"{CD}");
                                if (nextIndex == -1) {
                                    columns.Add(row.Substring(1, row.Length - 2));
                                    row = row.Remove(0, row.Length);
                                } else {
                                    columns.Add(row.Substring(1, nextIndex - 1));
                                    row = row.Remove(0, nextIndex + 2);
                                }
                            }
                            i = -1;
                            break;
                        default:
                            break;
                    }
                }
                if (row.Length > 0) {
                    columns.Add(row);
                }
                document[rowIndex] = columns.ToArray();
            }

            data = new Dictionary<string, Dictionary<string, string>>();
            foreach (var row in document) {
                if (row[0] == "Name") continue;
                Dictionary<string, string> cols = new Dictionary<string, string>();
                for (int i = 1; i < row.Length; i++) {
                    cols[document[0][i]] = row[i];
                }
                data[row[0]] = cols;
            }
        }

        public static string GetCell(string row, string column, string fallbackCol) {
            bool result = data.TryGetValue(row, out var col);
            if (!result) return "";
            result = col.TryGetValue(column, out var cell);
            if (result) return cell;
            result = col.TryGetValue(fallbackCol, out cell);
            return result ? cell : "";
        }
    }
}