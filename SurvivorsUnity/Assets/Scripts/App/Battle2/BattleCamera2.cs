using System.Threading;
using App.Battle2.Core;
using App.Battle2.Map;
using App.Battle2.Map.Cells;
using App.Battle2.UI;
using App.Battle2.Units;
using Constants;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle2
{
    /// <summary>
    /// バトルカメラカメラ
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(BattleCamera2))]
    public class BattleCamera2 : MonoBehaviour
    {
        [SerializeField] private new Camera camera;

        public static BattleCamera2 Instance { get; private set; }

        public Camera MainCamera => camera;
        private RectTransform _uiRect;
        private HexMapManager _mapManager;

        private readonly ReactiveProperty<Vector3> _cameraPosition = new();
        public IReactiveProperty<Vector3> Position => _cameraPosition;

        private CancellationTokenSource _moveCtx;

        private readonly ReactiveProperty<float> _cameraSizeRatio = new ReactiveProperty<float>(GameConst.DefaultBattleCameraSize);
        public IReadOnlyReactiveProperty<float> CameraSizeRatio => _cameraSizeRatio;

        private float _cameraSize = GameConst.DefaultBattleCameraSize;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            BattleUI ui,
            HexMapManager mapManager
        )
        {
            Instance = this;
            _cameraPosition.Value = camera.transform.position;
            _uiRect = ui.RectTrans;
            _mapManager = mapManager;

            SetCameraSize(GameConst.DefaultBattleCameraSize);
        }

        /// <summary>
        /// カメラサイズセット
        /// </summary>
        public void SetCameraSize(float size)
        {
            camera.orthographicSize = size; 
            _cameraSizeRatio.Value = GameConst.DefaultBattleCameraSize / size;
        }
        
        /// <summary>
        /// カメラサイズ縮小
        /// </summary>
        public void CameraZoomDown()
        {
            _cameraSize = Mathf.Clamp(camera.orthographicSize - 0.5f, GameConst.DefaultBattleCameraSize, GameConst.BattleCameraSizeLimit);
            SetCameraSize(_cameraSize);
        }

        /// <summary>
        /// カメラサイズ拡大
        /// </summary>
        public void CameraZoomUp()
        {
            _cameraSize = Mathf.Clamp(camera.orthographicSize + 0.5f, GameConst.DefaultBattleCameraSize, GameConst.BattleCameraSizeLimit);
            SetCameraSize(_cameraSize);
        }

        /// <summary>
        /// 敵フェイズのカメラへ
        /// </summary>
        public void SetToEnemyPhaseCamera()
        {
            SetCameraSize(GameConst.BattleCameraSizeLimit);
            SetPosition(_mapManager.Center);
        }

        /// <summary>
        /// 敵フェイズのカメラから戻す
        /// </summary>
        public void ResetFromEnemyPhaseCamera()
        {
            SetCameraSize(_cameraSize); 
        }
        
        /// <summary>
        /// 移動
        /// </summary>
        public static async UniTask Move(Vector3 position, bool isAnim)
        {
            if (isAnim)
            {
                await Instance.MoveToByAnimationAsync(position);
            }
            else
            {
                Instance.SetPosition(position);
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

        /// <summary>
        /// unit移動
        /// </summary>
        private void OnShipMoved(IUnitModel2 unitModel2)
        {
            SetPosition(unitModel2.Position);
        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            _moveCtx?.Cancel();
        }
    }
}