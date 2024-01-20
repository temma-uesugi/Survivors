// using App.AppCommon.UI;
// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections.Generic;
// using App.Battle.Inputs;
//
// namespace App.Battle.UI.Menu
// {
//     /// <summary>
//     /// バトル攻撃メニュー
//     /// </summary>
//     public class BattleAttackMenu : MenuBase<BattleMenuItemType.AttackMenu, MenuInputs>
//     {
//         [SerializeField] private BattleAttackMenuItem itemPrefab;
//         [SerializeField] private Transform listTrans;
//         [SerializeField] private VerticalLayoutGroup layoutGroup;
//         
//         protected override LayoutGroup LayoutGroup => layoutGroup;
//
//         /// <summary>
//         /// Setup
//         /// </summary>
//         public override void Setup(MenuInputs actionInputs)
//         {
//             base.Setup(actionInputs);
//             
//             
//         }
//         
//         /// <summary>
//         /// Item作成
//         /// </summary>
//         protected override IEnumerable<MenuItemBase<BattleMenuItemType.AttackMenu>> CreateItems(IEnumerable<BattleMenuItemType.AttackMenu> items)
//         {
//             var list = new List<MenuItemBase<BattleMenuItemType.AttackMenu>>();
//             foreach (var t in items)
//             {
//                 var item = Instantiate(itemPrefab, listTrans);
//                 item.Setup(new MenuItemRecord<BattleMenuItemType.AttackMenu>(t));
//                 list.Add(item);
//             }
//             return list;
//         }
//     }
// }