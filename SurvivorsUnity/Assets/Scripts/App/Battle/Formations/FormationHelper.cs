using App.Battle.Map;

namespace App.Battle.Formations
{
    /// <summary>
    /// 陣形性能Provider
    /// </summary>
    public class FormationHelper
    {
        private readonly MapManager _mapManager;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormationHelper(MapManager mapManager)
        {
            _mapManager = mapManager;
        }
    }
}