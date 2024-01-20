using App.Battle2.Facades;
using UniRx;
using UnityEngine.UI;

namespace App.Battle2.UI.MiniMap
{

	/// <summary>
	/// ミニマップのプレイヤカーソル表示用クラス
	/// </summary>
    public class BattleFocusMiniMap : BattleMiniMap
    {
		protected override void Awake()
		{
			base.Awake();
			BattleState.Facade.FocusedHexCell.Subscribe( x => SetVerticesDirty() ).AddTo(this);
		}

		protected override void PopulateMesh(VertexHelper vh)
		{
			Map.Cells.HexCell cell = BattleState.Facade.FocusedHexCell.Value;
			AddVertex(cell.GridX, cell.GridY, vh);
		}
	}
}