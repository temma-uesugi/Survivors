// using System.Collections.Generic;
// using App.Game;
// using App.Game.Map;
// using App.Game.ValueObjects;
// using UniRx;
// using UniRx.Triggers;
// using Unity.VisualScripting;
// using UnityEngine;
//
// namespace App.Test.MapMove
// {
//     /// <summary>
//     /// マップ移動テスト
//     /// </summary>
//     public class MapMoveTester : MonoBehaviour
//     {
//         [SerializeField] private BattleCamera battleCamera;
//         [SerializeField] private MapMoveController mapMoveController;
//         [SerializeField] private HexMapManager mapManager;
//
//         /// <summary>
//         /// Awake
//         /// </summary>
//         private void Awake()
//         {
//             mapManager.Setup(20, 15);
//             battleCamera.SetPosition(mapManager.Center);
//
//             var list = new List<HexCell>();
//             list.Add(mapManager.GetCellByGrid(new GridValue(3, 3)));
//             list.Add(mapManager.GetCellByGrid(new GridValue(3, 8)));
//             list.Add(mapManager.GetCellByGrid(new GridValue(3, 12)));
//             list.Add(mapManager.GetCellByGrid(new GridValue(10, 3)));
//             list.Add(mapManager.GetCellByGrid(new GridValue(10, 8)));
//             list.Add(mapManager.GetCellByGrid(new GridValue(10, 12)));
//             list.Add(mapManager.GetCellByGrid(new GridValue(17, 3)));
//             list.Add(mapManager.GetCellByGrid(new GridValue(17, 8)));
//             list.Add(mapManager.GetCellByGrid(new GridValue(17, 12)));
//             foreach (var cell in list)
//             {
//                 cell.ChangeColor(Color.red);
//                 cell.AddComponent<BoxCollider2D>();
//                 cell.AddComponent<ObservableEventTrigger>();
//                 // var eventTrigger = cell.GetComponent<ObservableEventTrigger>();
//                 // eventTrigger.OnPointerClickAsObservable().SubscribeWithState(cell, (_, sandbox) =>
//                 // {
//                 //     OnCellTap(sandbox);
//                 // }).AddTo(this);
//             }
//         }
//
//         /// <summary>
//         /// OnCellTap
//         /// </summary>
//         private void OnCellTap(HexCell cell)
//         {
//             var pos = mapMoveController.ClumpInCamera(cell.Position);
//             battleCamera.MoveToByAnimationAsync(pos);
//         }
//     }
// }