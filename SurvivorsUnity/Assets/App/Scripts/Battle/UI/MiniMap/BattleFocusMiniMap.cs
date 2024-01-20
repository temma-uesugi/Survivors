using App.Battle.Facades;
using App.Battle.Map.Cells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace App.Battle.UI
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