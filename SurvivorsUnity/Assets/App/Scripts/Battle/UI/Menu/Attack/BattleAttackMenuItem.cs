// using App.AppCommon.UI;
// using Cysharp.Threading.Tasks;
// using TMPro;
// using UnityEngine;
//
// namespace App.Battle.UI.Menu
// {
//     /// <summary>
//     /// バトル攻撃メニューアイテム
//     /// </summary>
//     public class BattleAttackMenuItem : MenuItemBase<BattleMenuItemType.AttackMenu>
//     {
//         [SerializeField] private TextMeshProUGUI text;
//         [SerializeField] private GameObject focused;
//         
//         /// <summary>
//         /// Setup
//         /// </summary>
//         public override void Setup(MenuItemRecord<BattleMenuItemType.AttackMenu> record)
//         {
//             base.Setup(record);
//             
//             text.SetText(record.ItemType.ToString());
//         }
//         
//         /// <summary>
//         /// フォーカス
//         /// </summary>
//         public override async UniTask SetFocusAsync(bool isFocus)
//         {
//             await base.SetFocusAsync(isFocus);
//             focused.SetActive(isFocus);
//         }
//     }
// }