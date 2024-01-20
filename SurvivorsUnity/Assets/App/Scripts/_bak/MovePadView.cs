// using App.AppCommon;
// using App.Core;
// using App.Game.Core;
// using App.Game.Map;
// using App.Game.Units;
// using UniRx;
// using UnityEngine;
//
//
// namespace App.Game.UI
// {
//     /// <summary>
//     /// MovePad
//     /// </summary>
//     [ContainerRegisterMonoBehaviour(typeof(MovePadView))]
//     public class MovePadView : MonoBehaviour
//     {
//         [SerializeField] private MoveButton btnRight;
//         [SerializeField] private MoveButton btnTopRight;
//         [SerializeField] private MoveButton btnTopLeft;
//         [SerializeField] private MoveButton btnLeft;
//         [SerializeField] private MoveButton btnBottomLeft;
//         [SerializeField] private MoveButton btnBottomRight;
//         [SerializeField] private BattleInputTrigger inputTrigger;
//
//         /// <summary>
//         /// Setup
//         /// </summary>
//         public void Setup(
//             MoveCommander commander
//         )
//         {
//             commander.OnShipSelected.Subscribe(OnShipSelected).AddTo(this);
//             commander.OnPositionChanged.Subscribe(_ => OnPositionChanged()).AddTo(this);
//
//             //コマンド
//             inputTrigger.OnMove
//                 .Select(HexUtil.InputVectorToMoveDir)
//                 .Select(x => HexUtil.InputDirToMoveDir(x, commander.CurrentDir))
//                 .Subscribe(dir =>
//                 {
//                     switch (dir)
//                     {
//                         case DirectionType.Right:
//                             if (commander.CommandRight.Executable.Value)
//                             {
//                                 commander.CommandRight.Execute();
//                             }
//                             break;
//                         case DirectionType.TopRight:
//                             if (commander.CommandTopRight.Executable.Value)
//                             {
//                                 commander.CommandTopRight.Execute();
//                             }
//                             break;
//                         case DirectionType.TopLeft:
//                             if (commander.CommandTopLeft.Executable.Value)
//                             {
//                                 commander.CommandTopLeft.Execute();
//                             }
//                             break;
//                         case DirectionType.Left:
//                             if (commander.CommandLeft.Executable.Value)
//                             {
//                                 commander.CommandLeft.Execute();
//                             }
//                             break;
//                         case DirectionType.BottomLeft:
//                             if (commander.CommandBottomLeft.Executable.Value)
//                             {
//                                 commander.CommandBottomLeft.Execute();
//                             }
//                             break;
//                         case DirectionType.BottomRight:
//                             if (commander.CommandBottomRight.Executable.Value)
//                             {
//                                 commander.CommandBottomRight.Execute();
//                             }
//                             break;
//                     }
//                 }).AddTo(this);
//             // btnRight.OnClick
//             //     .Where(_ => commander.CommandRight.Executable.Value)
//             //     .Subscribe(_ => { commander.CommandRight.Execute(); }).AddTo(this);
//             // btnTopRight.OnClick
//             //     .Where(_ => commander.CommandTopRight.Executable.Value)
//             //     .Subscribe(_ => { commander.CommandTopRight.Execute(); }).AddTo(this);
//             // btnTopLeft.OnClick
//             //     .Where(_ => commander.CommandTopLeft.Executable.Value)
//             //     .Subscribe(_ => { commander.CommandTopLeft.Execute(); }).AddTo(this);
//             // btnLeft.OnClick
//             //     .Where(_ => commander.CommandLeft.Executable.Value)
//             //     .Subscribe(_ => { commander.CommandLeft.Execute(); }).AddTo(this);
//             // btnBottomLeft.OnClick
//             //     .Where(_ => commander.CommandBottomLeft.Executable.Value)
//             //     .Subscribe(_ => { commander.CommandBottomLeft.Execute(); }).AddTo(this);
//             // btnBottomRight.OnClick
//             //     .Where(_ => commander.CommandBottomRight.Executable.Value)
//             //     .Subscribe(_ => { commander.CommandBottomRight.Execute(); }).AddTo(this);
//
//             commander.CommandRight.Value
//                 .Subscribe(btnRight.UpdateStatus)
//                 .AddTo(this);
//             commander.CommandTopRight.Value
//                 .Subscribe(btnTopRight.UpdateStatus)
//                 .AddTo(this);
//             commander.CommandTopLeft.Value
//                 .Subscribe(btnTopLeft.UpdateStatus)
//                 .AddTo(this);
//             commander.CommandLeft.Value
//                 .Subscribe(btnLeft.UpdateStatus)
//                 .AddTo(this);
//             commander.CommandBottomLeft.Value
//                 .Subscribe(btnBottomLeft.UpdateStatus)
//                 .AddTo(this);
//             commander.CommandBottomRight.Value
//                 .Subscribe(btnBottomRight.UpdateStatus)
//                 .AddTo(this);
//
//             commander.IsMovable.Subscribe(x =>
//             {
//                 gameObject.SetActive(x);
//             }).AddTo(this);
//         }
//
//         /// <summary>
//         /// Ship選択
//         /// </summary>
//         private void OnShipSelected(ShipUnitModel model)
//         {
//             btnRight.BindShip(model);
//             btnTopRight.BindShip(model);
//             btnTopLeft.BindShip(model);
//             btnLeft.BindShip(model);
//             btnBottomLeft.BindShip(model);
//             btnBottomRight.BindShip(model);
//         }
//
//         /// <summary>
//         /// 位置更新
//         /// </summary>
//         private void OnPositionChanged()
//         {
//             // btnRight.UpdatePosition();
//             // btnTopRight.UpdatePosition();
//             // btnTopLeft.UpdatePosition();
//             // btnLeft.UpdatePosition();
//             // btnBottomLeft.UpdatePosition();
//             // btnBottomRight.UpdatePosition();
//         }
//     }
// }