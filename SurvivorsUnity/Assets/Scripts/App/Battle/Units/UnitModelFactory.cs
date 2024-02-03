using App.Battle.Core;
using App.Battle.Map;
using VContainer;

namespace App.Battle.Units
{
    /// <summary>
    /// UnitModelのFactory
    /// </summary>
    [ContainerRegister(typeof(UnitModelFactory))]
    public class UnitModelFactory
    {
        private readonly IObjectResolver _resolver;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public UnitModelFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
       
        /// <summary>
        /// 味方の作成
        /// </summary>
        public HeroUnitModel CreateHero(HeroCreateParam createParam)
        {
            // var mapMoveChecker = _resolver.Resolve<HexMapMoveChecker>();
            // var attackChecker = _resolver.Resolve<HexMapAttackChecker>();
            var mapManager = _resolver.Resolve<MapManager>();
            return new HeroUnitModel(createParam, mapManager);
        }
        
        /// <summary>
        /// 敵作成
        /// </summary>
        public EnemyUnitModel CreateEnemy(EnemyCreateParam param)
        {
            var mapManager = _resolver.Resolve<MapManager>();
            return new EnemyUnitModel(param, mapManager);
        }
    }
}