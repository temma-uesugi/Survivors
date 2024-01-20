using App.Battle.Core;
using App.Battle.Map;
using App.Battle.Map.Cells;
using App.Battle.Units.Enemy;
using App.Battle.Units.Ship;
using VContainer;

namespace App.Battle.Common
{
    /// <summary>
    /// ModelのFactory(下手なやり方しかできなかった)
    /// </summary>
    [ContainerRegister(typeof(ModelFactory))]
    public class ModelFactory
    {
        private readonly IObjectResolver _resolver;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public ModelFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Shipの作成
        /// </summary>
        public ShipUnitModel CreateShip(ShipCreateParam createParam)
        {
            var mapMoveChecker = _resolver.Resolve<HexMapMoveChecker>();
            var attackChecker = _resolver.Resolve<HexMapAttackChecker>();
            return new ShipUnitModel(createParam, mapMoveChecker, attackChecker);
        }

        /// <summary>
        /// 敵作成
        /// </summary>
        public EnemyUnitModel CreateEnemy(EnemyCreateParam param)
        {
            var mapManager = _resolver.Resolve<HexMapManager>();
            return new EnemyUnitModel(param, mapManager);
        }
    }
}