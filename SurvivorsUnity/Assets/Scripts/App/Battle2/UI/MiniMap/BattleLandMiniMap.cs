using UnityEngine.UI;

namespace App.Battle2.UI.MiniMap
{
	/// <summary>
	/// ミニマップの陸地表示用クラス
	/// </summary>
    public class BattleLandMiniMap : BattleMiniMap
    {
		protected override void PopulateMesh(VertexHelper vh)
		{
			foreach(var e in miniMapManager.HexMap.LandCells)
			{
				AddVertex(e.GridX, e.GridY, vh);
			}
		}
	}
}