using App.Battle.Core;
using App.Battle.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.Battle
{
    /// <summary>
    /// GameのDI
    /// </summary>
    public class BattleLifetimeScope : BattleLifetimeScopeBase
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
                pointsBuilder.Add<BattleService>();
            });
        }
    }
}