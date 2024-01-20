// using System;
// using App.Core;
// using App.Game.Core;
// using App.Game.Units;
// using DG.Tweening;
// using UniRx;
// using UnityEngine;
// using VContainer;
//
// namespace App.Game.Map
// {
//     /// <summary>
//     /// マップの移動
//     /// </summary>
//     [ContainerRegisterMonoBehaviour(typeof(MapMoveController))]
//     public class MapMoveController : MonoBehaviour
//     {
//         [SerializeField] private BattleCamera battleCamera;
//         // [SerializeField] private ObservableEventTrigger eventTrigger;
//         [SerializeField] private BoxCollider2D boxCollider;
//
//         private IDisposable _disposable;
//
//         private Bounds _areaBounds;
//         private float _screenWidth;
//         private float _screenHeight;
//         private float _areaWidth;
//         private float _areaHeight;
//         private float _topLimit;
//         private float _bottomLimit;
//         private float _leftLimit;
//         private float _rightLimit;
//
//         //タップされたときのカーソルの位置
//         private Vector3 _dragStartPos;
//         //タップされた時のゲームオブジェクトの位置
//         private Vector3 _prevTapPos;
//         //カメラの初期位置
//         private Vector3 _cameraStartPos;
//         //慣性の速度
//         private Vector3 _inertia;
//         private bool _isDrag;
//         private Vector3 _dragPos;
//
//         //慣性移動の最大速度
//         private const float MaxInertiaMagnitude = 50f;
//         //慣性移動時間
//         private const float InertiaMoveDuration = 0.5f;
//         private Tweener _inertiaMoveTween;
//
//         private const float EdgeMargin = 5f;
//         private Vector2 _downStartPos;
//
//         private GameState _gameState;
//         private UnitManger _unitManger;
//         
//         /// <summary>
//         /// コンストラクタ
//         /// </summary>
//         [Inject]
//         public void Construct(
//             GameState gameState,
//             UnitManger unitManger
//         )
//         {
//             _gameState = gameState;
//             _unitManger = unitManger;
//         }
//         
//         /// <summary>
//         /// Setup
//         /// </summary>
//         public void Setup(Rect mapRect, Rect seaRect, HexCell minCell)
//         {
//             boxCollider.size = mapRect.size;
//             var offset = mapRect.size / 2 - (Vector2)(boxCollider.transform.position - minCell.Position);
//             boxCollider.offset = offset;
//             _areaBounds = new Bounds(seaRect.center, seaRect.size);
//
//             var screenMin = battleCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
//             var screenMax = battleCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
//             _screenWidth = screenMax.x - screenMin.x;
//             _screenHeight = screenMax.y - screenMin.y;
//             _areaWidth = _areaBounds.size.x;
//             _areaHeight = _areaBounds.size.y;
//
//             battleCamera.Position.Subscribe(_ => OnCameraPositionUpdate()).AddTo(this);
//             OnCameraPositionUpdate();
//
//             // eventTrigger.OnPointerDownAsObservable().Subscribe(evt =>
//             // {
//             //     _inertiaMoveTween?.Kill();
//             //     var pos = battleCamera.ScreenToWorldPoint(evt.position) - battleCamera.Position.Value;
//             //     _dragStartPos = pos;
//             //     _dragPos = pos;
//             //     _cameraStartPos = battleCamera.Position.Value;
//             //     _isDrag = true;
//             //     _downStartPos = evt.position;
//             // }).AddTo(this);
//             //
//             // eventTrigger.OnPointerUpAsObservable().Subscribe(evt =>
//             // {
//             //     var pos = battleCamera.ScreenToWorldPoint(evt.position) - battleCamera.Position.Value;
//             //     _inertia = (pos - _prevTapPos) / Time.deltaTime;
//             //     _isDrag = false;
//             //     MoveInertia(_inertia);
//             // }).AddTo(this);
//             //
//             // eventTrigger.OnDragAsObservable().Subscribe(evt =>
//             // {
//             //     var pos = battleCamera.ScreenToWorldPoint(evt.position) - battleCamera.Position.Value;
//             //     _dragPos = pos;
//             // }).AddTo(this);
//             //
//             // eventTrigger.OnPointerClickAsObservable().Subscribe(evt =>
//             // {
//             //     if ((evt.position - _downStartPos).sqrMagnitude >= 0.1f * 0.1f)
//             //     {
//             //         return;
//             //     }
//             //
//             //     if (!_gameState.OnShipAction || !_unitManger.TryGetShipModelById(_gameState.ActionUnitId, out var ship))
//             //     {
//             //         return;
//             //     }
//             //
//             //     SelectFacade.SelectUnit(_gameState.ActionUnitId);
//             // }).AddTo(this);
//         }
//
//         /// <summary>
//         /// カメラの位置更新
//         /// </summary>
//         private void OnCameraPositionUpdate()
//         {
//             //最後の+2f, +1fは奇数・偶数で値が変わる
//             _topLimit = _screenHeight < _areaHeight
//                 ? _areaBounds.max.y - _screenHeight / 2 + EdgeMargin
//                 : _areaBounds.max.y - EdgeMargin;
//             _bottomLimit = _screenHeight < _areaHeight
//                 ? _areaBounds.min.y + _screenHeight / 2 - EdgeMargin
//                 : _areaBounds.min.y + EdgeMargin;
//             _rightLimit = _screenWidth < _areaWidth
//                 ? _areaBounds.max.x - _screenWidth / 2 + EdgeMargin
//                 : _areaBounds.max.x - EdgeMargin;
//             _leftLimit = _screenWidth < _areaWidth
//                 ? _areaBounds.min.x + _screenWidth / 2 - EdgeMargin
//                 : _areaBounds.min.x + EdgeMargin;
//         }
//
//         /// <summary>
//         /// MoveInertia
//         /// </summary>
//         private void MoveInertia(Vector3 vector)
//         {
//             var magnitude = Mathf.Min(vector.magnitude, MaxInertiaMagnitude);
//             _inertiaMoveTween = DOVirtual
//                 .Float(magnitude, 0, InertiaMoveDuration, mag =>
//                 {
//                     var pos = battleCamera.Position.Value - vector.normalized * mag * Time.deltaTime;
//                     battleCamera.SetPosition(pos);
//                 })
//                 .SetEase(Ease.OutSine)
//                 .OnComplete(() =>
//                 {
//                     _inertiaMoveTween = null;
//                 });
//         }
//
//         /// <summary>
//         /// カメラ内に収める
//         /// </summary>
//         public Vector3 ClumpInCamera(Vector3 vector3)
//         {
//             var x = Mathf.Clamp(vector3.x, _leftLimit, _rightLimit);
//             var y = Mathf.Clamp(vector3.y, _bottomLimit, _topLimit);
//             return new Vector3(x, y, 0);
//         }
//
//         /// <summary>
//         /// Update
//         /// </summary>
//         private void Update()
//         {
//             if (_isDrag)
//             {
//                 // ドラッグ中はマウスの位置に追従
//                 var pos = _cameraStartPos - (_dragPos - _dragStartPos);
//                 battleCamera.SetPosition(pos);
//                 _prevTapPos = _dragPos;
//             }
//         }
//     }
// }