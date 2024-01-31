using UnityEngine.UI;

namespace App.Battle2.UI.MiniMap
{

	/// <summary>
	/// ミニマップのプレイヤ表示用クラス
	/// </summary>
    public class BattlePlayerMiniMap : BattleMiniMap
    {
		protected override void PopulateMesh(VertexHelper vh)
		{
			foreach(var e in miniMapManager.Unit.AllAliveShips)
			{
				AddVertex(e.Cell.Value.GridX, e.Cell.Value.GridY, vh);
			}
		}
	}
}