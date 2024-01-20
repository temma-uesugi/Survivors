using App.Battle.Core;
using VContainer;
using VContainer.Unity;

namespace App.Battle._Test.MessageHub
{
    public class MessageHubTestLifetimeScope : BattleLifetimeScopeBase
    {
        /// <summary>
        /// 設定
        /// </summary>
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            builder.UseEntryPoints(Lifetime.Singleton, pointsBuilder =>
            {
                pointsBuilder.Add<MessageHubTestService>();
            });
        }
    }
}