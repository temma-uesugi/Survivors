namespace App.Battle2.Units
{
    /// <summary>
    /// IUnitViewModel
    /// </summary>
    public interface IUnitViewModel2<T1, T2> where T1 : IUnitView2 where T2 : IUnitModel2
    {
        public T1 UnitView { get;  }
        public T2 UnitModel { get; }
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(T2 model);
    }
}