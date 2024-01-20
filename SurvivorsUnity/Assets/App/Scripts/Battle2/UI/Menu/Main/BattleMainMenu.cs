using System.Collections.Generic;
using App.AppCommon.UI;
using App.Battle2.Inputs;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle2.UI.Menu.Main
{
    /// <summary>
    /// バトルメインメニュー
    /// </summary>
    public class BattleMainMenu : MenuBase<BattleMenuItemType.MainMenu, MenuInputs>
    {
        [SerializeField] private BattleMainMenuItem itemPrefab;
        [SerializeField] private Transform listTrans;
        [SerializeField] private VerticalLayoutGroup layoutGroup;
        
        protected override LayoutGroup LayoutGroup => layoutGroup;

        /// <summary>
        /// Item作成
        /// </summary>
        protected override IEnumerable<MenuItemBase<BattleMenuItemType.MainMenu>> CreateItems(IEnumerable<BattleMenuItemType.MainMenu> items)
        {
            var list = new List<MenuItemBase<BattleMenuItemType.MainMenu>>();
            foreach (var t in items)
            {
                var item = Instantiate(itemPrefab, listTrans);
                item.Setup(new MenuItemRecord<BattleMenuItemType.MainMenu>(t));
                list.Add(item);
            }
            return list;
        }
    }
}