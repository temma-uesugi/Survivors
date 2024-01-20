using System.Collections.Generic;
using System.Linq;
using App.AppCommon.Extensions;
using App.Battle.EnemyBots.NodeObjects;
using App.Battle.Map;
using App.Battle.Units;
using App.Battle.Units.Enemy;
using App.Battle.Units.Ship;

namespace App.Battle.EnemyBots
{
    /// <summary>
    /// ターゲット取得
    /// </summary>
    public class TargetChecker
    {
        private readonly HexMapManager _mapManager;
        private readonly UnitManger _unitManger;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TargetChecker(
            HexMapManager mapManager,
            UnitManger unitManger
        )
        {
            _mapManager = mapManager;
            _unitManger = unitManger;
        }

        //TODO 障害物も対象に取れるように
        /// <summary>
        /// ターゲット取得
        /// </summary>
        public ShipUnitModel GetTarget(ActionTargetType[] targetTypes, EnemyUnitModel unitModel, ShipUnitModel[] ships = null)
        {
            var candidates = ships ?? _unitManger.AllAliveShips.ToArray();
            if (!candidates.Any())
            {
                return null;
            }
            
            ShipUnitModel target = null;
            foreach (var targetType in targetTypes)
            {
                target =  GetTarget(targetType, unitModel, candidates);
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
        private ShipUnitModel GetTarget(ActionTargetType targetType, EnemyUnitModel unitModel, ShipUnitModel[] candidates)
        {
            return targetType switch
            {
                ActionTargetType.Near => PickNear(unitModel, candidates),
                ActionTargetType.Weak => PickWeak(unitModel, candidates),
                ActionTargetType.Defeat => PickCanDefeat(unitModel, candidates),
                ActionTargetType.Kill => PickCanDefeat(unitModel, candidates),
                _ => null
            };
        }

        /// <summary>
        /// 一番近く
        /// </summary>
        private ShipUnitModel PickNear(EnemyUnitModel unitModel, IEnumerable<ShipUnitModel> candidates)
        {
            return candidates
                .RandomSort()
                .OrderBy(x => (unitModel.Position - x.Position).sqrMagnitude)
                .First();
        }

        /// <summary>
        /// 一番弱っている
        /// </summary>
        private ShipUnitModel PickWeak(EnemyUnitModel unitModel, IEnumerable<ShipUnitModel> candidates)
        {
            return candidates
                .RandomSort()
                .OrderBy(x => x.ArmorPoint.Value + x.CrewPoint.Value)
                .First();
        }

        /// <summary>
        /// 倒せる
        /// </summary>
        private ShipUnitModel PickCanDefeat(EnemyUnitModel unitModel, IEnumerable<ShipUnitModel> candidates)
        {
            //TODO
            return candidates
                .RandomSort()
                .OrderBy(x => x.ArmorPoint.Value + x.CrewPoint.Value)
                .First();
        }
    }
}