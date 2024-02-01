using Master.Battle.Core;
using VContainer;
using VContainer.Unity;

namespace Master.Battle
{
    /// <summary>
    /// BattleのDI
    /// </summary>
    public class BattleLifetimeScope : BattleLifetimeScopeBase
    {
        /// <summary>
        /// 設定
        /// </summary>
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.UseEntryPoints(Lifetime.Singleton, pointsBuilder =>
            {
                pointsBuilder.Add<BattleService>();
            });
        }
    }
}