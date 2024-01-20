// using System;
// using System.Collections.Generic;
// using System.Linq;
// using App.Core;
// using App.Game.Libs;
// using App.Game.Objects;
// using App.Game.Units;
// using App.Libs;
// using UniRx;
// using UniRx.Toolkit;
// using UnityEngine;
// using VContainer;
//
// namespace App.Game.UI
// {
//     /// <summary>
//     /// 攻撃PadView
//     /// </summary>
//     [ContainerRegisterMonoBehaviour(typeof(AttackPadView))]
//     public class AttackPadView : MonoBehaviour
//     {
//         [SerializeField] private BombButton bombButtonPrefab;
//         [SerializeField] private AttackButton attackButtonPrefab;
//         [SerializeField] private AssaultButton assaultButton;
//
//         private GameObjectPool<BombButton> _bombBtnPool;
//         private readonly List<BombButton> _bombBtnList = new();
//
//         private GameObjectPool<AttackButton> _attackBtnPool;
//         private readonly List<AttackButton> _attackBtnList = new();
//
//         private readonly List<IDisposable> _disposableList = new();
//
//         /// <summary>
//         /// Construct
//         /// </summary>
//         public void Setup(AttackCommander commander)
//         {
//             var trans = gameObject.transform;
//             _bombBtnPool = new GameObjectPool<BombButton>(bombButtonPrefab, trans);
//             _attackBtnPool = new GameObjectPool<AttackButton>(attackButtonPrefab, trans);
//
//             //ボタンにExecuteを結び付け
//             _bombBtnPool.OnCreateInstance.Subscribe(btn =>
//             {
//                 // var disposable = btn.OnClick.SubscribeWithState(btn, (_, o) =>
//                 // {
//                 //     commander.CommandBomb.Execute(o.Model.UnitId);
//                 // });
//                 // _disposableList.Add(disposable);
//             }).AddTo(this);
//             _attackBtnPool.OnCreateInstance.Subscribe(btn =>
//             {
//                 // var disposable = btn.OnClick.SubscribeWithState(btn, (_, o) =>
//                 // {
//                 //     commander.CommandAttack.Execute(o.Model.UnitId);
//                 // });
//                 // _disposableList.Add(disposable);
//             }).AddTo(this);
//             // assaultButton.OnClick
//             //     .Where(_ => commander.CommandAssault.Executable.Value)
//             //     .Subscribe(_ =>
//             //     {
//             //         commander.CommandAssault.Execute();
//             //     }).AddTo(this);
//
//             commander.OnPositionChanged.Subscribe(_ => OnPositionChanged()).AddTo(this);
//
//             commander.CommandBomb.Value
//                 .Subscribe(BombBtnUpdate)
//                 .AddTo(this);
//             commander.CommandAttack.Value
//                 .Subscribe(AttackBtnUpdate)
//                 .AddTo(this);
//             commander.CommandAssault.Value
//                 .Subscribe(AssaultBtnUpdate)
//                 .AddTo(this);
//
//             commander.OnActivate.Subscribe(gameObject.SetActive).AddTo(this);
//         }
//
//         /// <summary>
//         /// 砲撃ボタンの更新
//         /// </summary>
//         private void BombBtnUpdate(List<IUnitModel> targetList)
//         {
//             //一旦全てOFF
//             foreach (var btn in _bombBtnList)
//             {
//                 _bombBtnPool.Return(btn);
//             }
//             _bombBtnList.Clear();
//
//             if (!targetList.Any())
//             {
//                 return;
//             }
//
//             foreach (var target in targetList)
//             {
//                 var btn = _bombBtnPool.Rent();
//                 // btn.BindShip(target);
//                 btn.SetActive(true);
//                 _bombBtnList.Add(btn);
//             }
//         }
//
//         /// <summary>
//         /// 攻撃ボタンの更新
//         /// </summary>
//         private void AttackBtnUpdate(List<IUnitModel> targetList)
//         {
//             //一旦全てOFF
//             foreach (var btn in _attackBtnList)
//             {
//                 _attackBtnPool.Return(btn);
//             }
//             _attackBtnList.Clear();
//
//             if (!targetList.Any())
//             {
//                 return;
//             }
//
//             foreach (var target in targetList)
//             {
//                 var btn = _attackBtnPool.Rent();
//                 btn.BindShip(target);
//                 btn.SetActive(true);
//                 _attackBtnList.Add(btn);
//             }
//         }
//
//         /// <summary>
//         /// 突撃ボタンの更新
//         /// </summary>
//         private void AssaultBtnUpdate(IUnitModel unitModel)
//         {
//             if (unitModel == null)
//             {
//                 assaultButton.SetActive(false);
//                 return;
//             }
//             assaultButton.SetActive(true);
//             assaultButton.BindShip(unitModel);
//         }
//
//         /// <summary>
//         /// 位置更新
//         /// </summary>
//         private void OnPositionChanged()
//         {
//             foreach (var btn in _bombBtnList)
//             {
//                 btn.UpdatePosition();
//             }
//             foreach (var btn in _attackBtnList)
//             {
//                 btn.UpdatePosition();
//             }
//             if (assaultButton.Model != null)
//             {
//                 assaultButton.UpdatePosition();
//             }
//         }
//
//         /// <summary>
//         /// OnDestroy
//         /// </summary>
//         private void OnDestroy()
//         {
//             foreach (var disposable in _disposableList)
//             {
//                 disposable.Dispose();
//             }
//         }
//     }
// }