using System.Collections.Generic;
using System.Linq;
using App.Battle2.Core;
using App.Battle2.Map;
using App.Battle2.Objects.Obstacle;
using App.Battle2.Utils;
using App.Battle2.ValueObjects;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle2.Objects
{
    /// <summary>
    /// Mapオブジェクト管理
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(MapObjectManger))]
    public class MapObjectManger : MonoBehaviour
    {
        [SerializeField] private Transform obstacleLayer;
        [SerializeField] private ObstacleViewModel obstaclePrefab;
        
        private HexMapManager _hexMapManager;
     
        private readonly ReactiveDictionary<uint, ObstacleModel> _obstacleModelMap = new();
        public IReadOnlyReactiveDictionary<uint, ObstacleModel> ObstacleModelMap => _obstacleModelMap;
        public IEnumerable<ObstacleModel> AllAliveObstacles => _obstacleModelMap.Values.Where(x => x.IsAlive);
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            HexMapManager hexMapManager
        )
        {
            _hexMapManager = hexMapManager;
        }
        
        /// <summary>
        /// 障害物作成
        /// </summary>
        public void CreateObstacle(
            GridValue grid
        )
        {
            var objectId = ObjectUtil.GetObjectId();
            var hexCell = _hexMapManager.GetCellByGrid(grid);
            var obstacleModel = new ObstacleModel(objectId, hexCell);
            var obstacle = Instantiate(obstaclePrefab, obstacleLayer);
            obstacle.Setup(obstacleModel);
            _obstacleModelMap.Add(objectId, obstacleModel);
            obstacleModel.OnDestroyed.Subscribe(objId => _obstacleModelMap.Remove(objId)).AddTo(this);
        }
        
        /// <summary>
        /// IDから障害物Model取得
        /// </summary>
        public ObstacleModel GetObstacleModelById(uint id, bool includeNotAlive = false)
        {
            if (!_obstacleModelMap.TryGetValue(id, out var obstacle))
            {
                return null;
            }

            if (!includeNotAlive && !obstacle.IsAlive)
            {
                return null;
            }

            return obstacle;
        }

        /// <summary>
        /// IDから障害物Model取得を試みる
        /// </summary>
        public bool TryGetObstacleModelById(uint id, out ObstacleModel obstacle, bool includeNotAlive = false)
        {
            obstacle = GetObstacleModelById(id, includeNotAlive);
            if (obstacle == null)
            {
                return false;
            }

            return true;
        }
    }
}