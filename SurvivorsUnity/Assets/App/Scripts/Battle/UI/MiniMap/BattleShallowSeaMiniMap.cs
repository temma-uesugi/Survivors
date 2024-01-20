using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle.UI
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