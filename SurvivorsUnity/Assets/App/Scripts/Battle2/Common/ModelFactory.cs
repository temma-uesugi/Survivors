using App.Battle2.Core;
using App.Battle2.Map;
using App.Battle2.Units.Enemy;
using App.Battle2.Units.Ship;
using VContainer;

namespace App.Battle2.Common
{
    /// <summary>
    /// ModelのFactory(下手なやり方しかできなかった)
    /// </summary>
    [ContainerRegisterAttribute2(typeof(ModelFactory))]
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