using UnityEngine.UI;

namespace App.Battle2.UI.MiniMap
{
	/// <summary>
	/// ミニマップの敵表示用クラス
	/// </summary>
    public class BattleEnemyMiniMap : BattleMiniMap
    {
		protected override void PopulateMesh(VertexHelper vh)
		{
			foreach(var e in miniMapManager.Unit.AllAliveEnemies)
			{
				AddVertex(e.Cell.Value.GridX, e.Cell.Value.GridY, vh);
			}
		}
	}
}