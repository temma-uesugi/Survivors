using System.Threading;
using App.AppCommon;
using App.Battle.Core;
using App.Battle.Map;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;
using DG.Tweening;

namespace App.Battle
{
    /// <summary>
    /// バトルカメラ
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(BattleCamera))]
    public class BattleCamera : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        
        public Camera MainCamera => camera;

        private readonly ReactiveProperty<Vector3> _cameraPosition = new();
        public IReactiveProperty<Vector3> Position => _cameraPosition;
        
        private MapManager _mapManager;
        private CancellationTokenSource _moveCtx;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            MapManager mapManager
        )
        {
            _mapManager = mapManager;
            camera.orthographicSize = BattleConst.CameraSize; 
        }
      
        /// <summary>
        /// 移動
        /// </summary>
        public async UniTask Move(Vector3 position, bool isAnim)
        {
            if (isAnim)
            {
                await MoveToByAnimationAsync(position);
            }
            else
            {
                SetPosition(position);
            }
        }
        
        /// <summary>
        /// ポジションのセット
        /// </summary>
        public void SetPosition(Vector3 pos)
        {
            var clumpPos = pos;
            // var clumpPos = _mapMoveController.ClumpInCamera(pos);
            clumpPos.z = -1;
            camera.transform.position = clumpPos;
            _cameraPosition.Value = clumpPos;
        }

        /// <summary>
        /// アニメーションしながら移動
        /// </summary>
        public async UniTask MoveToByAnimationAsync(Vector3 pos, float durationSec = 0.3f)
        {
            var clumpPos = pos;
            // var clumpPos = _mapMoveController.ClumpInCamera(pos);
            clumpPos.z = -1;
            _moveCtx?.Cancel();
            _moveCtx = new CancellationTokenSource();
            await transform.DOMove(clumpPos, durationSec)
                .OnUpdate(() =>
                {
                    _cameraPosition.Value = transform.position;
                })
                .SetEase(Ease.OutSine)
                .ToUniTask(cancellationToken: _moveCtx.Token);
        }

        /// <summary>
        /// スクリーン位置をMainCamera基準でワールド座標(z=0)に変換する
        /// </summary>
        public Vector3 ScreenToWorldPoint(Vector3 screenPos)
        {
            return camera.ScreenToWorldPoint(screenPos);
        }

        /// <summary>
        /// ワールド座標ををMainCamera基準でScreen座標に変換する
        /// </summary>
        public Vector2 WorldToScreenPoint(Vector3 worldPos)
        {
            return RectTransformUtility.WorldToScreenPoint(camera, worldPos);
        }

        /// <summary>
        /// anchoredPositionが画面内か
        /// </summary>
        public bool IsAnchoredPositionVisible(Vector2 anchoredPosition)
        {
            return anchoredPosition.x >= -Screen.width / 2f
                   && anchoredPosition.x <= Screen.width / 2f
                   && anchoredPosition.y >= -Screen.height / 2f
                   && anchoredPosition.y <= Screen.height / 2f;
        }
        
        /// <summary>
        /// CellからScreen座標
        /// </summary>
        public Vector2 CellToScreenPoint(HexCell hexCell)
        {
            return WorldToScreenPoint(hexCell.Position);
        }

        // /// <summary>
        // /// unit移動
        // /// </summary>
        // private void OnShipMoved(IUnitModel unitModel)
        // {
        //     SetPosition(unitModel.Position);
        // }
    }
}