using App.Battle2.Core;
using VContainer;
using VContainer.Unity;

namespace App.Battle2
{
    /// <summary>
    /// GameのDI
    /// </summary>
    public class BattleLifetimeScope2 : BattleLifetimeScopeBase2
    {
        // [SerializeField] private IconImageMaps iconImageMaps;

        /// <summary>
        /// 設定
        /// </summary>
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            //view
            // builder.RegisterComponent(iconImageMaps);

            builder.UseEntryPoints(Lifetime.Singleton, pointsBuilder =>
            {
                pointsBuilder.Add<BattleService2>();
            });
        }
    }
}