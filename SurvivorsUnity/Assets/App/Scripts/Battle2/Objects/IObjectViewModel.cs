namespace App.Battle2.Objects
{
    /// <summary>
    /// ObjectViewModelのインターフェイス
    /// </summary>
    public interface IObjectViewModel<T1, T2> where T1 : IObjectView where T2 : IObjectModel
    {
        public T1 ObjectView { get;  }
        public T2 ObjectModel { get; }
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(T2 model);
    }
}