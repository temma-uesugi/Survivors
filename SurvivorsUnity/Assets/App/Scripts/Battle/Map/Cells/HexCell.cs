using App.AppCommon;
using App.Battle.ValueObjects;
using UnityEngine;

namespace App.Battle.Map.Cells
{
    /// <summary>
    /// Hexセル
    /// </summary>
    public abstract class HexCell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        protected SpriteRenderer SpriteRenderer => spriteRenderer;

        public enum Type
        {
            Sea = 1,
            Ford = 2,
            Ground = 3,
        }
        public abstract Type CellType { get; }

        public GridValue Grid { get; private set; }
        public int GridX => Grid.X;
        public int GridY => Grid.Y;

        public Vector3 Position => transform.position;

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(int x, int y)
        {
            transform.localScale = Vector3.one * GameConst.DefaultHexImageScale;
            Grid = new GridValue(x, y);
        }

        /// <summary>
        /// カラーを変更
        /// </summary>
        public void ChangeColor(Color color)
        {
            spriteRenderer.color = color;
        }

        public bool EqualByGrid(GridValue gridValue)
        {
            return GridX == gridValue.X && GridY == gridValue.Y;
        }
    }
}