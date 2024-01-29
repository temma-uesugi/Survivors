using System.Threading;
using App.AppCommon.Core;
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
        
        public static Camera MainCamera { get; private set; }

        private readonly ReactiveProperty<Vector3> _cameraPosition = new();
        public IReactiveProperty<Vector3> Position => _cameraPosition;
        
        private MapManager _mapManager;
        private CancellationTokenSource _moveCtx;
      
        //カメラ移動範囲制御
        private Bounds _areaBounds;
        private float _screenWidth;
        private float _screenHeight;
        private float _areaWidth;
        private float _areaHeight;
        private float _topLimit;
        private float _bottomLimit;
        private float _leftLimit;
        private float _rightLimit;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            MapManager mapManager
        )
        {
            _mapManager = mapManager;
            MainCamera = camera;
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            camera.orthographicSize = BattleConst.CameraSize; 
            // var screenMin = ScreenToWorldPoint(new Vector3(0, 0, 0));
            // var screenMax = ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            // _screenWidth = screenMax.x - screenMin.x;
            // _screenHeight = screenMax.y - screenMin.y;
            //
            var a = ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            Log.Debug("hoge 2", a, Screen.width, Screen.height);
            //
            // _areaWidth = _mapManager.MapRect.x;
            // _areaHeight = _mapManager.MapRect.y;
            //
            // Log.Debug("hoge", _mapManager.MapRect, a); 
            //
            // OnCameraPositionUpdate();
            // var clumpPos = ClumpInCamera(camera.transform.position);
            // clumpPos.z = -1;
            // //TODO
            // camera.transform.position = new Vector3(11.5f, 6, -1);
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
        public static Vector3 ScreenToWorldPoint(Vector3 screenPos)
        {
            return MainCamera.ScreenToWorldPoint(screenPos);
        }

        /// <summary>
        /// ワールド座標ををMainCamera基準でScreen座標に変換する
        /// </summary>
        public static Vector2 WorldToScreenPoint(Vector3 worldPos)
        {
            return RectTransformUtility.WorldToScreenPoint(MainCamera, worldPos);
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
        /// カメラの位置更新
        /// </summary>
        private void OnCameraPositionUpdate()
        {
            //最後の+2f, +1fは奇数・偶数で値が変わる
            _topLimit = _screenHeight < _areaHeight
                ? _mapManager.MapRect.max.y - _screenHeight / 2 + 2f
                : _mapManager.MapRect.max.y - 2f;
            _bottomLimit = _screenHeight < _areaHeight
                ? _mapManager.MapRect.min.y + _screenHeight / 2 - 2f
                : _mapManager.MapRect.min.y + 2f;
            _rightLimit = _screenWidth < _areaWidth
                ? _mapManager.MapRect.max.x - _screenWidth / 2 + 2f
                : _mapManager.MapRect.max.x - 2f;
            _leftLimit = _screenWidth < _areaWidth
                ? _mapManager.MapRect.min.x + _screenWidth / 2 - 2f
                : _mapManager.MapRect.min.x + 2f;
        }
       
        /// <summary>
        /// カメラ内に収める位置を計算
        /// </summary>
        public Vector3 ClumpInCamera(Vector3 vector3)
        {
            var x = Mathf.Clamp(vector3.x, _leftLimit, _rightLimit);
            var y = Mathf.Clamp(vector3.y, _bottomLimit, _topLimit);
            return new Vector3(x, y, 0);
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