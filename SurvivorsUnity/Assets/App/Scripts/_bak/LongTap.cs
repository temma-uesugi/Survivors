// using System;
// using Cysharp.Threading.Tasks;
// using UniRx;
// using UniRx.Triggers;
// using UnityEngine;
//
// namespace App.UI
// {
//     /// <summary>
//     /// 長押し
//     /// </summary>
//     [DefaultExecutionOrder(-9)]
//     // [RequireComponent(typeof(ObservableEventTrigger))]
//     public class LongTap : MonoBehaviour
//     {
//         private const float MoveThreshold = 50f;
//         private const float TapDurationSec = 1f;
//         
//         // private ObservableEventTrigger _eventTrigger;
//         private bool _isLongTaped = false;
//         private bool _isPressed = false;
//         private bool _isMoving = false;
//         private Vector2 _pressStartPosition;
//         private IDisposable _updateDisposable;
//
//         private readonly Subject<Unit> _onLongTap = new();
//         public IObservable<Unit> OnLongTap => _onLongTap;
//
//         /// <summary>
//         /// Awake
//         /// </summary>
//         private void Awake()
//         {
//             // _eventTrigger = GetComponent<ObservableEventTrigger>();
//             Setup();
//         }
//       
//         /// <summary>
//         /// Setup
//         /// </summary>
//         private void Setup()
//         {
//             // _eventTrigger.OnPointerDownAsObservable()
//             //     .Subscribe(e =>
//             //     {
//             //         _isLongTaped = false;
//             //         _isPressed = true;
//             //         _isMoving = false;
//             //         _pressStartPosition = e.position;
//             //         _updateDisposable = Observable.Timer(TimeSpan.FromSeconds(TapDurationSec))
//             //             .RepeatUntilDestroy(gameObject)
//             //             .Subscribe(_ =>
//             //             {
//             //                 if (_isPressed && !_isMoving)
//             //                 {
//             //                     _isLongTaped = true; 
//             //                     _onLongTap.OnNext(Unit.Default);
//             //                     _updateDisposable.Dispose();
//             //                 }
//             //             });
//             //     })
//             //     .AddTo(this);
//             //
//             // _eventTrigger.OnPointerUpAsObservable()
//             //     .Subscribe(_ =>
//             //     {
//             //         _isPressed = false;
//             //         _isMoving = false;
//             //         
//             //         //LongTapが成立していたら、他コンポーネントのタップなどのイベントが発生しないように、_eventTriggerを一瞬enabledにする
//             //         if (_isLongTaped)
//             //         {
//             //             _isLongTaped = false;
//             //             _eventTrigger.enabled = false;
//             //             UniTask.Void(async () =>
//             //             {
//             //                 await UniTask.Yield();
//             //                 _eventTrigger.enabled = true;
//             //             });
//             //         }
//             //     })
//             //     .AddTo(this);
//             //
//             // _eventTrigger.OnDragAsObservable()
//             //     .Subscribe(e =>
//             //     {
//             //         if (_isPressed && !_isMoving)
//             //         {
//             //             var moveDistance = Vector2.Distance(e.position, _pressStartPosition);
//             //             if (moveDistance > MoveThreshold)
//             //             {
//             //                 _isMoving = true;
//             //             }
//             //         }
//             //     })
//             //     .AddTo(this);
//         }
//
//         /// <summary>
//         /// OnDestroy
//         /// </summary>
//         private void OnDestroy()
//         {
//             _updateDisposable?.Dispose();
//         } 
//     }
// }