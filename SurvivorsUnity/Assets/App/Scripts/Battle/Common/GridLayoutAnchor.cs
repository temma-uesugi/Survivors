using System;
using System.Collections.Generic;
using App.AppCommon.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle.Common
{
    /// <summary>
    /// グリッドレイアウトのアンカー
    /// </summary>
    [RequireComponent(typeof(GridLayoutGroup), typeof(CanvasGroup))]
    [DefaultExecutionOrder(-9)]
    public class GridLayoutAnchor : MonoBehaviour
    {
        [SerializeField] private int amount;

        private GridLayoutGroup _gridLayout;
        private CanvasGroup _canvasGroup;

        private readonly List<Vector3> _positions = new();
        public Vector2 CellSize => _gridLayout.cellSize;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            _gridLayout = GetComponent<GridLayoutGroup>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(int itemAmount)
        {
            _gridLayout = GetComponent<GridLayoutGroup>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            UpdateGrid();

            var initAmount = Math.Max(itemAmount, amount);
            var childAmount = transform.childCount;
            if (childAmount < initAmount)
            {
                for (int i = childAmount; i < initAmount; i++)
                {
                    SetAnchor(i);
                }
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                _positions.Add(transform.GetChild(i).position);
            }
        }

        /// <summary>
        /// UpdateGrid
        /// </summary>
        private void UpdateGrid()
        {
            _gridLayout.CalculateLayoutInputHorizontal();
            _gridLayout.CalculateLayoutInputVertical();
            _gridLayout.SetLayoutHorizontal();
            _gridLayout.SetLayoutVertical();
        }

        /// <summary>
        /// 位置取得
        /// </summary>
        public Vector3 GetPosition(int index)
        {
            if (index < 0)
            {
                Log.Error("エラー");
                return Vector2.zero;
            }
            if (_positions.Count < index - 1)
            {
                for (int i = _positions.Count; i < index; i++)
                {
                    SetAnchor(i);
                }
                UpdateGrid();
                for (int i = _positions.Count; i < index; i++)
                {
                    _positions.Add(transform.GetChild(i).position);
                }
            }
            return _positions[index];
        }

        /// <summary>
        /// アンカーをamount分セット
        /// </summary>
        public void SetAnchors()
        {
            GetComponent<CanvasGroup>().alpha = 0.25f;
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < amount; i++)
            {
                SetAnchor(i);
            }
        }

        /// <summary>
        /// アンカーのセット
        /// </summary>
        private void SetAnchor(int index)
        {
            var anchor = new GameObject($"Anchor_{index}");
            anchor.transform.SetParent(transform);
            anchor.AddComponent<Image>();
            anchor.GetComponent<Image>().color = new Color(1, 1, 1);
            var anchorRect = anchor.GetComponent<RectTransform>();
            anchorRect.anchorMin = Vector2.zero;
            anchorRect.anchorMax = Vector2.one;

            var indexNo = new GameObject("IndexNo");
            indexNo.transform.SetParent(anchor.transform);
            indexNo.AddComponent<Text>();
            var indexNoRect = indexNo.GetComponent<RectTransform>();
            indexNoRect.anchorMin = Vector2.zero;
            indexNoRect.anchorMax = Vector2.one;
            indexNoRect.offsetMin = new Vector2(5, 5);
            indexNoRect.offsetMax = new Vector2(-5, -5);
            var noText = indexNo.GetComponent<Text>();
            noText.color = new Color(0, 0, 0);
            noText.resizeTextForBestFit = true;
            noText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            noText.alignment = TextAnchor.MiddleCenter;
            noText.text = (index + 1).ToString();
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// グリッドレイアウトのアンカーのEditor拡張
    /// </summary>
    [CustomEditor(typeof(GridLayoutAnchor))]
    public class GridLayoutAnchorEditor : UnityEditor.Editor
    {
        private GridLayoutAnchor Self => target as GridLayoutAnchor;

        /// <summary>
        /// 更新処理
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("配置"))
            {
                Self.SetAnchors();
            }
        }
    }
#endif
}