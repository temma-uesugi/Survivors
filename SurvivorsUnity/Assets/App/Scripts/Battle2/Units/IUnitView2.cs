using App.AppCommon;
using App.Battle2.Map.Cells;

namespace App.Battle2.Units
{
    /// <summary>
    /// UnitViewのインターフェイス
    /// </summary>
    public interface IUnitView2
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
        /// 向き
        /// </summary>
        DirectionType Direction { get; } 
       
        bool IsAlive { get; } 
        
        /// <summary>
        /// Cellにセット
        /// </summary>
        void SetToCell(HexCell cell);

        /// <summary>
        /// 向きを設定
        /// </summary>
        void SetDirection(DirectionType dir);
        
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