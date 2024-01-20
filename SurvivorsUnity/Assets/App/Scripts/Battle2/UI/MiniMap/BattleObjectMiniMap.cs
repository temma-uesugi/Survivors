using UnityEngine.UI;

namespace App.Battle2.UI.MiniMap
{

	/// <summary>
	/// ミニマップの障害物示用クラス
	/// </summary>
    public class BattleObjectMiniMap : BattleMiniMap
    {
		protected override void PopulateMesh(VertexHelper vh)
		{
			foreach(var e in miniMapManager.HexMap.ObstacleCells)
			{
				AddVertex(e.GridX, e.GridY, vh);
			}
		}
	}
}