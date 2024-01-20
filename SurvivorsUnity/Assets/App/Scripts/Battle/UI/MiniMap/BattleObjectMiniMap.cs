using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle.UI
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