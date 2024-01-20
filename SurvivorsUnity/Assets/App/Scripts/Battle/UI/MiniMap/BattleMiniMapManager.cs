﻿using UnityEngine;
using App.Battle.Units;
using App.Battle.Core;
using App.Battle.Map;
using VContainer;

using UniRx;
using Cysharp.Threading.Tasks;

namespace App.Battle.UI
{
	[ContainerRegisterMonoBehaviour(typeof(BattleMiniMapManager))]
    public class BattleMiniMapManager : MonoBehaviour
    {
		private UnitManger _unit;
        /// <summary>UnitManager</summary>
        public UnitManger Unit{get{return _unit;}}

        private HexMapManager _hexMap;
        /// <summary>HexMapManager</summary>
        public HexMapManager HexMap{get{return _hexMap;}}

        private MapIconManager _mapIcon;
        /// <summary>MapIconManager</summary>
        public MapIconManager MapIcon{get{return _mapIcon;}}

        private BattleEventHub _eventHub;
        /// <summary>BattleEventHub</summary>
        public BattleEventHub EventHub{get{return _eventHub;}}

        [SerializeField]
        private Vector2 _cellSize = new Vector2(32.0f, 32.0f);
        /// <summary>マスのサイズ</summary>
        public Vector2 cellSize{get{return _cellSize;}}

        [SerializeField]
        private BattleEnemyMiniMap _enemyMiniMap = null;

        [SerializeField]
        private BattlePlayerMiniMap _playerMiniMap = null;

        [SerializeField]
        private BattleObjectMiniMap _objectMiniMap = null;

        [SerializeField]
        private BattleShallowSeaMiniMap _shallowSeaMiniMap = null;

        [SerializeField]
        private BattleFocusMiniMap _focusMiniMap = null;

        [SerializeField]
        private BattleLandMiniMap _landMiniMap = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(UnitManger unitManager, HexMapManager hexMapManager, MapIconManager mapIconManager, BattleEventHub eventHub)
        {
            _unit = unitManager;
            _hexMap = hexMapManager;
            _mapIcon = mapIconManager;
            _eventHub = eventHub;
        }

        /// <summary>再描画</summary>
        public void Repaing()
        {
            RepaingAsync().Forget();
        }

        /// <summary>再描画</summary>
        public UniTask RepaingAsync()
        {
            // 敵表示用マップ更新
            _enemyMiniMap.SetVerticesDirty();
            // プレイヤ表示用マップ更新
            _playerMiniMap.SetVerticesDirty();
            // オブジェクト表示用マップ更新
            _objectMiniMap.SetVerticesDirty();
            // 浅瀬表示用マップ更新
            _shallowSeaMiniMap.SetVerticesDirty();
            // 陸地表示用マップ更新
            _landMiniMap.SetVerticesDirty();

            return UniTask.CompletedTask;
        }

		private void Start()
		{
			// マップのサイズ調
            RectTransform t = (RectTransform)transform;
            t.sizeDelta = _cellSize * new Vector2(_hexMap.Width, _hexMap.Height);

            // 敵表示用マップ初期化
            _enemyMiniMap.Initialize(this);
            // プレイヤ表示用マップ初期化
            _playerMiniMap.Initialize(this);
            // オブジェクト表示用マップ初期化
            _objectMiniMap.Initialize(this);
            // 浅瀬表示用マップ初期化
            _shallowSeaMiniMap.Initialize(this);
            // 陸地初期化
            _landMiniMap.Initialize(this);
            // フォーカス用マップ初期
            _focusMiniMap.Initialize(this);

            // ユニット移動時に更新
            _eventHub.Subscribe<BattleEvents.OnEnemyMovedEvent>(v=>RepaingAsync()).AddTo(this);
            _eventHub.Subscribe<BattleEvents.OnShipMovedEvent>(v=>RepaingAsync()).AddTo(this);
            _eventHub.Subscribe<BattleEvents.OnPhaseEndAsync>(v=>RepaingAsync()).AddTo(this);
		}
	}
}
