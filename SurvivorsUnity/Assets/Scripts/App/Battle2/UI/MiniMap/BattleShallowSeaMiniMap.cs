using UnityEngine.UI;

namespace App.Battle2.UI.MiniMap
{
	/// <summary>
	/// ミニマップの浅瀬表示用クラス
	/// </summary>
    public class BattleShallowSeaMiniMap : BattleMiniMap
    {
		protected override void PopulateMesh(VertexHelper vh)
		{
			foreach(var e in miniMapManager.HexMap.FordHexCells)
			{
				AddVertex(e.GridX, e.GridY, vh);
			}
		}
	}
}