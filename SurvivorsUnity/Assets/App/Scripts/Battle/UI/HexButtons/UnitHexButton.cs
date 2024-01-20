using App.Battle.Map.Cells;
using App.Battle.Units.Ship;
using UniRx;

namespace App.Battle.UI.HexButtons
{
    /// <summary>
    /// Unitが存在するHex用ボタン
    /// </summary>
    public class UnitHexButton : HexButton
    {
        public ShipUnitModel Model { get; private set; }
        private readonly CompositeDisposable _disposable = new();
        private HexCell _curCell;
        
        /// <summary>
        /// 位置更新
        /// </summary>
        public void SetCell(HexCell cell)
        {
            _curCell = cell;
            if (_curCell == null)
            {
                return;
            }
            SetPositionByCell();
        }

        /// <summary>
        /// Cellから位置更新
        /// </summary>
        private void SetPositionByCell()
        {
            var screenPos = BattleCamera.Instance.CellToScreenPoint(_curCell);
            SetPosition(screenPos);
        }
        
        /// <inheritdoc />
        protected override void OnUpdateCameraSizeRatio(float ratio)
        {
            base.OnUpdateCameraSizeRatio(ratio);
            if (_curCell == null)
            {
                return; 
            }
            SetPositionByCell();
        }
        
        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}