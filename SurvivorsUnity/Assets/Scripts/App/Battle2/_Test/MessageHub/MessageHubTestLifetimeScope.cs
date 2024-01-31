using App.Battle2.Core;
using VContainer;
using VContainer.Unity;

namespace App.Battle2._Test.MessageHub
{
    public class MessageHubTestLifetimeScope : BattleLifetimeScopeBase2
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