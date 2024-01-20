using UnityEngine;
using UnityEngine.UI;

namespace App.Battle.UI
{
    public abstract class BattleMiniMap : Graphic
    {
		[SerializeField]
		private Texture2D _mainTexture = null;
		/// <summary>テクスチャ</summary>
		public override Texture mainTexture{get{return _mainTexture;}}

		[SerializeField]
		private float iconScale = 1.0f;

		private BattleMiniMapManager _miniMapManager = null;
		/// <summary>マネージャー</summary>
		protected BattleMiniMapManager miniMapManager{get{return _miniMapManager;}}


		protected void AddVertex(int x, int y, VertexHelper vh)
		{
			// マスサイズ
			Vector2 cellSize = miniMapManager.cellSize;
			// 
			Vector2 half = cellSize * 0.5f;
			// 表示位置中央
			Vector2 p = cellSize * new Vector2(x, y) + half;
			// 表示サイズ
			half *= iconScale;


			int vertexCount = vh.currentVertCount;

			vh.AddVert(p + new Vector2(-half.x, -half.y), Color.white, new Vector2(   0,    0));
			vh.AddVert(p + new Vector2(-half.x,  half.y), Color.white, new Vector2(   0, 1.0f));
			vh.AddVert(p + new Vector2( half.x,  half.y), Color.white, new Vector2(1.0f, 1.0f));
			vh.AddVert(p + new Vector2( half.x, -half.y), Color.white, new Vector2(1.0f,    0));

			vh.AddTriangle(vertexCount + 0, vertexCount + 1, vertexCount + 2);
			vh.AddTriangle(vertexCount + 0, vertexCount + 2, vertexCount + 3);
		}


		protected abstract void PopulateMesh(VertexHelper vh);

		protected sealed override void OnPopulateMesh(VertexHelper vh)
		{
#if UNITY_EDITOR
			if(Application.isPlaying == false)return;
#endif
			vh.Clear();
			PopulateMesh(vh);
		}

		/// <summary>初期化</summary>
		public void Initialize(BattleMiniMapManager miniMapManager)
		{
			_miniMapManager = miniMapManager;
		}
    }
}
