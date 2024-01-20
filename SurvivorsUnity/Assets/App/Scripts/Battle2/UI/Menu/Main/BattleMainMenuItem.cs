using App.AppCommon.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace App.Battle2.UI.Menu.Main
{
    /// <summary>
    /// メインメニュー
    /// </summary>
    public class BattleMainMenuItem : MenuItemBase<BattleMenuItemType.MainMenu>
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private GameObject focused;
        
        /// <summary>
        /// Setup
        /// </summary>
        public override void Setup(MenuItemRecord<BattleMenuItemType.MainMenu> record)
        {
            base.Setup(record);
            
            text.SetText(record.ItemType.ToString());
        }
        
        /// <summary>
        /// フォーカス
        /// </summary>
        public override async UniTask SetFocusAsync(bool isFocus)
        {
            await base.SetFocusAsync(isFocus);
            focused.SetActive(isFocus);
        }
    }
}