namespace App.AppCommon.Libs
{
    /// <summary>
    /// シングルトンの継承元クラス 継承先でもconstructできてしまうので注意
    /// </summary>
    public class SingletonBase<T> where T : class, new()
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        public static T Instance => instance ??= new();
        protected static T instance;
    }
}