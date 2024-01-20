using App.Battle2.Map;
using VContainer;
using VContainer.Unity;

namespace App.Battle2._Test.RouteSearch
{
    public class RouteSearchLifetimeScope : LifetimeScope
    {

        /// <summary>
        /// 設定
        /// </summary>
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
           
            builder.RegisterComponentInHierarchy<HexMapManager>();
            builder.RegisterComponentInHierarchy<TestCamera>();
            builder.RegisterComponentInHierarchy<RouteSearchTester>();
            
            builder.UseEntryPoints(Lifetime.Singleton, pointsBuilder =>
            {
                pointsBuilder.Add<RouteSearchService>();
            });
        }
    }
}