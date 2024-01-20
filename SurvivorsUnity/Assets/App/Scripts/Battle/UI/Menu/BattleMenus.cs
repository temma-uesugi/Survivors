using App.AppCommon.UI;
using App.Battle.Facades;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Battle.UI.Menu
{
    /// <summary>
    /// バトルメニュー
    /// </summary>
    [DefaultExecutionOrder(-99)]
    public class BattleMenus : MonoBehaviour
    {
        private static BattleMenus _instance;
        public static BattleMenus Facade => _instance;
        
        [SerializeField] private BattleMainMenu mainMenu;
        // [SerializeField] private BattleAttackMenu attackMenu;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            _instance = this;
            
            mainMenu.Setup(BattleOperation.Facade.Menu);
            // attackMenu.Setup(BattleOperation.Facade.Menu);
        }

        /// <summary>
        /// MainMenuを開く
        /// </summary>
        public async UniTask<BattleMenuItemType.MainMenu> OpenMainMenuAsync(BattleMenuItemType.MainMenu itemType)
        {
            var beforeMode = BattleOperation.Facade.CurrentMode; 
            await BattleOperation.Facade.SwitchModeAsync(BattleOperation.ModeType.Menu);
            var res = await mainMenu.OpenAsync(itemType);
            await BattleOperation.Facade.SwitchModeAsync(beforeMode);
            return res;
        }
        
        // /// <summary>
        // /// AttackMenuを開く
        // /// </summary>
        // public async UniTask<BattleMenuItemType.AttackMenu> OpenAttackMenuAsync(BattleMenuItemType.AttackMenu itemType)
        // {
        //     await BattleOperation.Facade.SwitchModeAsync(BattleOperation.ModeType.Menu);
        //     var res = await attackMenu.OpenAsync(itemType);
        //     await BattleOperation.Facade.SwitchModeAsync(BattleOperation.ModeType.Unit);
        //     return res;
        // }
    }
}