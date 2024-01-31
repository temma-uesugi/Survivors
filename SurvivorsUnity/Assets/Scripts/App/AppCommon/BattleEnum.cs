using System;

namespace App.AppCommon
{
    /// <summary>
    /// 操作モード
    /// </summary>
    public enum OperationModeType
    {
        None = 0,
        Map = 1,
        Unit = 2,
        Common = 9,
    }
  
    /// <summary>
    /// Mapセルタイプ
    /// </summary>
    public enum MapCellType
    {
        Normal = 1,
        //茂み
        Bushes = 2,
        //森
        Forest = 3,
        //岩場
        RockyArea = 4,
        //砂漠
        Desert = 5,
        //川
        River = 6,
        //沼
        Swamp = 7,
        //侵入不可
        NoEnter = 9
    }
    
    /// <summary>
    /// ターンタイプ
    /// </summary>
    public enum PhaseType
    {
        None = 0,
        PlayerPhase = 1,
        EnemyPhase = 2,
    }

    /// <summary>
    /// 砲撃角度
    /// </summary>
    public enum BombRangeType
    {
        Narrow = 60,
        Middle = 90,
        Wide = 120,
    }
  
    /// <summary>
    /// 砲台サイド
    /// </summary>
    public enum BombSide
    {
        Right = -1,
        Left = 1,
    }
    
    /// <summary>
    /// 方向タイプ
    /// </summary>
    [Flags]
    public enum DirectionType
    {
        None = 0,
        Right = 1 << 0,
        TopRight = 1 << 1,
        TopLeft = 1 << 2,
        Left = 1 << 3,
        BottomLeft = 1 << 4,
        BottomRight = 1 << 5,
        All = Right | TopRight | TopLeft | Left | BottomLeft | BottomRight,
        DiagonalTop = TopRight | TopLeft,
        DiagonalBottom = BottomLeft | BottomRight,
        DiagonalRight = TopRight | BottomRight,
        DiagonalLeft = TopLeft | BottomLeft,
        AllRight = DiagonalRight | Right,
        AllLeft = DiagonalLeft | Left,
        Diagonal = TopRight | TopLeft | BottomRight | BottomLeft,
        Horizontal = Right | Left,
    }

    /// <summary>
    /// 天気タイプ
    /// </summary>
    [Flags]
    public enum WeatherType
    {
        None = 0,
        //晴れ
        Sun = 1 << 0,
        //雨
        Rain = 1 << 1,
        //曇り
        Cloud = 1 << 2,
        //霧
        Fog = 1 << 3,
        //嵐
        Storm = 1 << 4,
        //雷雨
        Thunderstorm = 1 << 5,
    }

    /// <summary>
    /// 方向キー
    /// </summary>
    public enum KeyDir
    {
        None,
        Right,
        Left,
        Up,
        Down
    }

    /// <summary>
    /// ダメージタイプ
    /// </summary>
    public enum AttackType
    {
        Invalid = 0,
        Slash = 1,
        Bomb = 2,
        Assault = 3,
        EnemyAttack = 4,
    }
    
    /// <summary>
    /// 入力方向Type
    /// </summary>
    public enum InputDirectionType
    {
        None = 0,
        Right = 1,
        TopRight = 2,
        Top = 3,
        TopLeft = 4,
        Left = 5,
        BottomLeft = 6,
        Bottom = 7,
        BottomRight = 8,
    }

    /// <summary>
    /// 敵がActiveになるタイプ
    /// </summary>
    public enum EnemyActiveConditionType
    {
        //条件なし=常に
        None = 1, 
        //ターン経過
        ProgressTurn = 2,
        //範囲に入る
        InRange = 3,
        //攻撃範囲に入る
        InAttackRange = 4,
        //攻撃を受けた
        Attacked = 5,
        //特定の天気で
        ByWeather = 6,
    }

    /// <summary>
    /// 敵が非Activeになるタイプ
    /// </summary>
    public enum EnemyInactiveConditionType
    {
        //非アクティブにならない
        None = 0,
        //Active条件と逆
        Invert = 1,    
        //ターン経過
        ProgressTurn = 2,    
        //範囲外
        OutRange = 3,
        //攻撃範囲外
        OutAttackRange = 4,
        //特定の天気で
        ByWeather = 6,
        //行動回数
        ActionCount = 7,
        //攻撃回数
        AttackCount = 8,
    }
    
    /// <summary>
    /// スキル効果タイプ
    /// </summary>
    public enum SkillEffectType
    {
        //直接攻撃
        DirectAttack = 1,
        //遠隔攻撃
        RangedAttack = 2,
    }

    /// <summary>
    /// スキル効果範囲タイプ
    /// </summary>
    public enum SkillEffectRangeType
    {
        //対象
        Target = 1,
        //対象の周囲
        TargetAround = 2,
        //対象と対象の範囲
        TargetAndAround = 3,
        //発動者本人
        Executor = 11,
        //発動者の周囲
        ExecutorAround = 12,
        //発動者と発動者の範囲
        ExecutorAndAround = 13,
        //相手サイド全て
        OpponentAll = 21,
        //味方サイド全て
        AllyAll = 22,
    }
}