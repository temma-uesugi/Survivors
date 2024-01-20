using UnityEngine;

namespace App.Battle.Libs
{
    /// <summary>
    /// 円弧の描画
    /// </summary>
    /// <see href="https://gologius.hatenadiary.com/entry/2017/03/13/004826"/>
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class ArcDraw  : MonoBehaviour
    {
        public float radius = 10.0f;
        public float startDegree = 10.0f;
        public float endDegree = 170.0f;
        public int triangleNum = 5;

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            MeshFilter m = this.GetComponent<MeshFilter>();
            m.mesh = CreateMesh();
        }

        /// <summary>
        /// Meshの作成
        /// </summary>
        private Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            //頂点座標計算
            Vector3[] vertices = new Vector3[2 + triangleNum];
            Vector2[] uv = new Vector2[2 + triangleNum];
            vertices[0] = new Vector3(0f, 0f, 0f);
            uv[0] = new Vector2(0.5f, 0.5f);
            float deltaRad = Mathf.Deg2Rad * ((endDegree - startDegree) / (float) triangleNum);
            for (int i = 1; i < 2 + triangleNum; i++)
            {
                float x = Mathf.Cos(deltaRad * (i - 1) + (Mathf.Deg2Rad * startDegree));
                float y = Mathf.Sin(deltaRad * (i - 1) + (Mathf.Deg2Rad * startDegree));
                vertices[i] = new Vector3(
                    x * radius,
                    y * radius,
                    0.0f);
                uv[i] = new Vector2(x * 0.5f + 0.5f, y * 0.5f + 0.5f);
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            //三角形を構成する頂点のindexを，順に設定していく
            int[] triangles = new int[3 * triangleNum];
            for (int i = 0; i < triangleNum; i++)
            {
                triangles[(i * 3)] = 0;
                triangles[(i * 3) + 1] = i + 1;
                triangles[(i * 3) + 2] = i + 2;
            }

            mesh.triangles = triangles;
            return mesh;
        }
    }
}