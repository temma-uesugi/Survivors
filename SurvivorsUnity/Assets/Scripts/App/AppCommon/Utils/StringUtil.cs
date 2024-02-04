using PlasticPipe.PlasticProtocol.Messages;

namespace App.AppCommon.Utils
{
    /// <summary>
    /// 文字列Util
    /// </summary>
    public static class StringUtil
    {
        //大文字・小文字の文字コードの始まり
        private const int UpperLetterCode = 65;
        private const int LowerLetterCode = 97;

        /// <summary>
        /// 数値をアルファベット文字に(0始まり)
        /// </summary>
        public static string IntToAlphabet(int num)
        {
            if (num < 26)
            {
                //26未満なら大文字
                num += UpperLetterCode;
            }
            else
            {
                //小文字に
                num += LowerLetterCode - 26;
            }
            return ((char)num).ToString();
        }

        /// <summary>
        /// 後ろから文字を検索して削除
        /// </summary>
        public static string RemoveFromEnd(this string self, string removeString)
        {
            var lastIndex = self.LastIndexOf(removeString);
            return (lastIndex != -1) ? self.Remove(lastIndex, removeString.Length) : self;
        }
    }
}