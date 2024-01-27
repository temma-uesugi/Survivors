using App.AppCommon;
using App.Battle.Map;

namespace App.Battle.Units
{
    /// <summary>
    /// UnitViewのインターフェイス
    /// </summary>
    public interface IUnitView
    {
        /// <summary>
        /// UnitId
        /// </summary>
        uint UnitId { get; }

        /// <summary>
        /// Cell
        /// </summary>
        HexCell Cell { get; } 
        
        /// <summary>
        /// Cellにセット
        /// </summary>
        void SetToCell(HexCell cell);

        /// <summary>
        /// 初期化
        /// </summary>
        void Init(uint unitId, HexCell initCell, string imageId);

        /// <summary>
        /// 死亡
        /// </summary>
        void OnDefeated();

        /// <summary>
        /// Activeかのセット
        /// </summary>
        void SetActive(bool isActive);
    }
}