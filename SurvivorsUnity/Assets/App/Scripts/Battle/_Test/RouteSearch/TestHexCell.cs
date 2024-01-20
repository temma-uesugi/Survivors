using App.Battle.Map.Cells;
using UnityEngine;

namespace App.Battle._Test.RouteSearch
{
    /// <summary>
    /// テスト用のHexCell
    /// </summary>
    public class TestHexCell : SeaHexCell
    {
        // [SerializeField] private ObservableEventTrigger eventTrigger;

        // public IObservable<TestHexCell> OnClick => eventTrigger.OnPointerClickAsObservable().Select(_ => this);

        /// <summary>
        /// StartIconのセット
        /// </summary>
        public void SetStart()
        {
            SpriteRenderer.color = Color.blue;
        }

        /// <summary>
        /// GoalIconのセット
        /// </summary>
        public void SetGoal()
        {
            SpriteRenderer.color = Color.red;
        }

        /// <summary>
        /// RouteIconのセット
        /// </summary>
        public void SetRoute()
        {
            SpriteRenderer.color = Color.green;
        }

        /// <summary>
        /// Clear
        /// </summary>
        public void Clear()
        {
            SpriteRenderer.color = Color.white;
        }
    }
}