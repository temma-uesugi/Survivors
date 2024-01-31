using FastEnumUtility;

namespace App.AppCommon.UI
{
    /// <summary>
    /// マスタテキストの文字列
    /// </summary>
    public enum MasterStringType
    {
        [Label("ターン", Lang.Jp)]
        Turn,

        [Label("ラウンド", Lang.Jp)]
        Round,

        [Label("味方攻撃フェイズ", Lang.Jp)]
        AllyPhase,

        [Label("敵攻撃フェイズ", Lang.Jp)]
        EnemyPhase,

        [Label("切り込み", Lang.Jp)]
        AttackToShip,

        [Label("砲撃", Lang.Jp)]
        BombardToShip,

        [Label("突撃", Lang.Jp)]
        AssaultToShip,

        [Label("撃破", Lang.Jp)]
        Defeated,
        
        [Label("残りPass回数", Lang.Jp)]
        RemainingPassCount,
        
        [Label("残り移動力", Lang.Jp)]
        RemainingMovePower,
        
        [Label("波:低", Lang.Jp)]
        WaveHeight1,
        
        [Label("波:中", Lang.Jp)]
        WaveHeight2,
        
        [Label("波:高", Lang.Jp)]
        WaveHeight3,
    }
}