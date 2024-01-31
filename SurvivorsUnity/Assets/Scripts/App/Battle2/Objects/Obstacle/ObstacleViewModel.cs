using UniRx;
using UnityEngine;

namespace App.Battle2.Objects.Obstacle
{
    [RequireComponent(typeof(ObstacleView))]
    public class ObstacleViewModel : MonoBehaviour, IObjectViewModel<ObstacleView, ObstacleModel>
    {
        public ObstacleView ObjectView { get; private set; }
        public ObstacleModel ObjectModel { get; private set; }

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            ObjectView = GetComponent<ObstacleView>();
        }
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(ObstacleModel obstacleModel)
        {
            ObjectModel = obstacleModel;
            ObjectView.Init(obstacleModel.ObjectId, obstacleModel.Cell.Value);

            obstacleModel.Cell.Subscribe(ObjectView.SetToCell).AddTo(this);
            obstacleModel.OnDestroyed.Subscribe(_ => ObjectView.OnDestroyed()).AddTo(this);
        }
    }
}