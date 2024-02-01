using App.AppCommon;
using App.Battle2.ValueObjects;
using Master.Constants;

namespace App.Battle2.Units.Ship
{
    //TODO ここマスタに移す
    /// <summary>
    /// 船ステータス
    /// </summary>
    public record ShipStatus(
        StatusValue<double> MovePower,
        StatusValue<int> ArmorPoint,
        StatusValue<int> CrewPoint,
        int ActionSpeed,
        BombStatus LeftBombStatus,
        BombStatus RightBombStatus
    )
    {
        /// <summary>
        /// ダミーステータス
        /// </summary>
        public static ShipStatus DummyStatus => new ShipStatus(
            new StatusValue<double>(0),
            new StatusValue<int>(0),
            new StatusValue<int>(0),
            0,
            BombStatus.DummyStatus,
            BombStatus.DummyStatus
        );
    }

    /// <summary>
    /// 砲台ステータス
    /// </summary>
    public record BombStatus(
        BombRangeType RangeType,
        int RangeDistance
    )
    {
         
        /// <summary>
        /// ダミーステータス
        /// </summary>
        public static BombStatus DummyStatus => new BombStatus(
            BombRangeType.Middle,
            GameConst.DefaultBombardRangeDistance
        );

        public bool IsAttackPossible { get; private set; } = true;

        /// <summary>
        /// 攻撃可能かどうかのフラグ設定
        /// </summary>
        public void SetAttackPossible(bool isAttackPossible)
        {
            IsAttackPossible = isAttackPossible;
        }
       
    }
}