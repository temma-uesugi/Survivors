using System;

namespace App.AppCommon.UI.Menu
{
    /// <summary>
    /// バトルのメニューアイテム
    /// </summary>
    public static class BattleMenuItemType
    {
        /// <summary>
        /// メインメニュー
        /// </summary>
        [Flags]
        public enum MainMenu
        {
            [IgnoreMenuItem]
            None = 0,
            //ユニットスキル
            [Order(0)] UnitSkill = 1 << 0,
            //提督スキル
            [Order(1)] AdmiralSkill = 1 << 1,
            //砲弾変更
            [Order(2)] ShellChange = 1 << 2,
            //ターン終了
            [Order(3)] EndTurn = 1 << 9,
            //Mapアイテム
            [IgnoreMenuItem]
            MapItems = AdmiralSkill | EndTurn,
            //ユニットアイテム
            [IgnoreMenuItem]
            UnitItems = UnitSkill | AdmiralSkill | ShellChange | EndTurn,
            // //提督スキルアイテム
            // [IgnoreMenuItem]
            // AdmiralSkillItems = AdmiralSkill,
        }
        //
        // /// <summary>
        // /// 攻撃メニュー
        // /// </summary>
        // [Flags]
        // public enum AttackMenu
        // {
        //     [IgnoreMenuItem]
        //     None = 0,
        //     //砲撃スキル
        //     BombSkill = 1 << 0,
        //     //砲弾変更
        //     ShellChange = 1 << 1,
        //     //切り込みスキル
        //     SlashSkill = 1 << 2,
        //     //突撃スキル
        //     AssaultSkill = 1 << 3,
        //     [IgnoreMenuItem]
        //     BombItems = BombSkill | ShellChange,
        //     [IgnoreMenuItem]
        //     SlashItems = SlashSkill,
        //     [IgnoreMenuItem]
        //     AssaultItems = AssaultSkill,
        // }
    }
}