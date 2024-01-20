using App.Battle2.Map;
using VContainer;
using VContainer.Unity;

namespace App.Battle2._Test.RouteSearch
{
    /// <summary>
    /// 経路探索テストサービス
    /// </summary>
    public class RouteSearchService : IStartable
    {
        private readonly HexMapManager _map;
        private readonly TestCamera _testCamera;
        private readonly RouteSearchTester _tester;

        [Inject]
        public RouteSearchService(
            HexMapManager map,
            TestCamera testCamera,
            RouteSearchTester tester
        )
        {
            _map = map;
            _testCamera = testCamera;
            _tester = tester;
        }
        
        /// <summary>
        /// Start
        /// </summary>
        public void Start()
        {
            _tester.Setup(16, 8);
            _testCamera.SetPosition(_map.Center);
        }
    }
}