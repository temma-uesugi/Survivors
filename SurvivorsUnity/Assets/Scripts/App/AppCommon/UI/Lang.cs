using System.Linq;
using FastEnumUtility;

namespace App.AppCommon.UI
{
    /// <summary>
    /// 言語関係
    /// </summary>
    public static class Lang
    {
        public enum Type
        {
            Jp = 1,
            En = 2,
        }
        public const int Jp = (int)Type.Jp;
        public const int En = (int)Type.En;

        private static Type _type = Type.Jp;

        /// <summary>
        /// 言語設定
        /// </summary>
        public static void SetLang(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// マスタ文字列取得
        /// </summary>
        public static string GetMasterString(MasterStringType type) => type.GetLabel(_type.ToInt32());

        /// <summary>
        /// テキスト取得
        /// </summary>
        public static string GetText(string text, params MasterStringType[] types)
        {
            var masterStr = types.Select(GetMasterString).OfType<object>().ToArray();
            return string.Format(text, masterStr);
        }
    }
}