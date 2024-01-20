using System.Collections.Generic;
using System.Linq;
using App.Battle.Map;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace App.Battle._Test.RouteSearch
{
    /// <summary>
    /// テスター
    /// </summary>
    public class RouteSearchTester : MonoBehaviour
    {
        private HexMapManager _map;
        
        private TestHexCell[] _cells;
        private TestHexCell _currentStartCell;
        private TestHexCell _currentGoalCell;
        private HashSet<TestHexCell> _routeCells = new();
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Constructor(HexMapManager mapManager)
        {
            _map = mapManager;
            new MapRoutSearch().Construct(mapManager, null);
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(int x, int y)
        {
            // _cells = _map.SetupForTest(x, y);

            // _cells
            //     .Select(c => c.OnClick)
            //     .Merge()
            //     .Subscribe(OnClick)
            //     .AddTo(this);
        }

        /// <summary>
        /// セルのクリック
        /// </summary>
        private void OnClick(TestHexCell cell)
        {
            if (_currentStartCell == null)
            {
                _currentStartCell = cell;
                _currentStartCell.SetStart();
                return;
            }
            
            if (cell == _currentStartCell)
            {
                return;
            }

            Reset();
            _currentGoalCell = cell;
            _currentGoalCell.SetGoal();

            // var step = MapRoutSearch.HeuristicDistance(_currentStartCell, _currentGoalCell);
            var routes = MapRoutSearch.FindPath(_currentStartCell, _currentGoalCell);
            if (routes == null || !routes.Any())
            {
                return;
            }
          
            _routeCells = routes
                .Select(x => _map.GetCellByGrid(x))
                .Cast<TestHexCell>()
                .ToHashSet();
            foreach (var r in _routeCells)
            {
                if (r == null || r == _currentStartCell || r == _currentGoalCell) continue;
                r.SetRoute();
            }
        }

        /// <summary>
        /// Reset
        /// </summary>
        private void Reset()
        {
            foreach (var r in _cells)
            {
                if (r == null || r == _currentStartCell) continue;
                r.Clear();
            }
        }
        
#if UNITY_EDITOR
        public void ResetAll()
        {
            _currentStartCell = null;
            Reset();
        }
#endif
    }
    
    
#if UNITY_EDITOR
    [CustomEditor(typeof(RouteSearchTester))]
    public class RouteSearchTesterTester : Editor
    {
        private RouteSearchTester _target => (RouteSearchTester) target;


        /// <summary>
        /// 更新処理
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Reset"))
            {
                _target.ResetAll();
            }
        }
    }
#endif
}
