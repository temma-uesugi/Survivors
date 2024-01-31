using App.AppCommon;
using UnityEngine;

namespace App.Battle.Map
{
    /// <summary>
    /// 通常Cell
    /// </summary>
    public class NormalCell : HexCell
    {
        public override MapCellType CellType => MapCellType.Normal;

        [SerializeField] private Sprite[] sprites;

        /// <summary>
        /// Awake
        /// </summary>
        public void Awake()
        {
            SpriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }
}