#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace Editor.MasterMemory
{
    /// <summary>
    /// SpreadSheet取得
    /// </summary>
    public class SpreadSheetGetter
    {
        private readonly SheetsService _service;
        private readonly Dictionary<string, string> _idMap;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SpreadSheetGetter(string keyPath, Dictionary<string, string> sheetIdMap)
        {
            _idMap = sheetIdMap;
            var stream = new FileStream(keyPath, FileMode.Open, FileAccess.Read);
            var credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);
            _service = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential
            });
        }

        /// <summary>
        /// 取得
        /// </summary>
        public Dictionary<string, string>[] Get(string category, string masterName)
        {
            if (!_idMap.TryGetValue(category, out var id))
            {
                return null;
            }
            var request = _service.Spreadsheets.Values.Get(id, masterName);
            var values = request.Execute().Values.ToList();

            List<Dictionary<string, string>> res = new();
            Dictionary<int, string?> keys = values[2]
                .Select((x, i) => (row: i, val: x.ToString()))
                .Where(x => !string.IsNullOrWhiteSpace(x.val))
                .ToDictionary(x => x.row, x => x.val);
            
            foreach (var row in values.Skip(3))
            {
                Dictionary<string, string> dic = new();
                foreach (var col in row.Select((x, i) => (val: x.ToString(), idx: i)))
                {
                    if (!keys.TryGetValue(col.idx, out var key))
                    {
                        continue;
                    }

                    var val = col.val;
                    if (key == null || val == null) continue;
                    dic.Add(key, val);
                }

                res.Add(dic);
            }

            return res.ToArray();
        }
    }
}

#endif