namespace App.Battle.Units
{
    /// <summary>
    /// IUnitViewModel
    /// </summary>
    public interface IUnitViewModel<T1, T2> where T1 : UnitViewBase where T2 : IUnitModel
    {
        public T1 UnitView { get;  }
        public T2 UnitModel { get; }
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(T2 model);
    }
}