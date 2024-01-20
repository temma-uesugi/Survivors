using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle.UI
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