using System.Collections.Generic;
using System.Linq;
using App.AppCommon.Extensions;
using App.Battle2.EnemyBots.NodeObjects;
using App.Battle2.Map;
using App.Battle2.Units;
using App.Battle2.Units.Enemy;
using App.Battle2.Units.Ship;

namespace App.Battle2.EnemyBots
{
    /// <summary>
    /// ターゲット取得
    /// </summary>
    public class TargetChecker
    {
        private readonly HexMapManager _mapManager;
        private readonly UnitManger2 unitManger2;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TargetChecker(
            HexMapManager mapManager,
            UnitManger2 unitManger2
        )
        {
            _mapManager = mapManager;
            this.unitManger2 = unitManger2;
        }

        //TODO 障害物も対象に取れるように
        /// <summary>
        /// ターゲット取得
        /// </summary>
        public ShipUnitModel2 GetTarget(ActionTargetType[] targetTypes, EnemyUnitModel2 unitModel2, ShipUnitModel2[] ships = null)
        {
            var candidates = ships ?? unitManger2.AllAliveShips.ToArray();
            if (!candidates.Any())
            {
                return null;
            }
            
            ShipUnitModel2 target = null;
            foreach (var targetType in targetTypes)
            {
                target =  GetTarget(targetType, unitModel2, candidates);
                if (target != null)
                {
                    break;
                }
            }
            return target;
        }
        
        /// <summary>
        /// 対象
        /// </summary>
        private ShipUnitModel2 GetTarget(ActionTargetType targetType, EnemyUnitModel2 unitModel2, ShipUnitModel2[] candidates)
        {
            return targetType switch
            {
                ActionTargetType.Near => PickNear(unitModel2, candidates),
                ActionTargetType.Weak => PickWeak(unitModel2, candidates),
                ActionTargetType.Defeat => PickCanDefeat(unitModel2, candidates),
                ActionTargetType.Kill => PickCanDefeat(unitModel2, candidates),
                _ => null
            };
        }

        /// <summary>
        /// 一番近く
        /// </summary>
        private ShipUnitModel2 PickNear(EnemyUnitModel2 unitModel2, IEnumerable<ShipUnitModel2> candidates)
        {
            return candidates
                .RandomSort()
                .OrderBy(x => (unitModel2.Position - x.Position).sqrMagnitude)
                .First();
        }

        /// <summary>
        /// 一番弱っている
        /// </summary>
        private ShipUnitModel2 PickWeak(EnemyUnitModel2 unitModel2, IEnumerable<ShipUnitModel2> candidates)
        {
            return candidates
                .RandomSort()
                .OrderBy(x => x.ArmorPoint.Value + x.CrewPoint.Value)
                .First();
        }

        /// <summary>
        /// 倒せる
        /// </summary>
        private ShipUnitModel2 PickCanDefeat(EnemyUnitModel2 unitModel2, IEnumerable<ShipUnitModel2> candidates)
        {
            //TODO
            return candidates
                .RandomSort()
                .OrderBy(x => x.ArmorPoint.Value + x.CrewPoint.Value)
                .First();
        }
    }
}