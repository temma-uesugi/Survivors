using System.Collections.Generic;
using App.Battle.Core;
using App.Battle.Map;
using App.Battle.Units;
using App.Battle2.ValueObjects;
using Master.Constants;
using UnityEngine;

namespace App.Battle.Formations
{
    /// <summary>
    /// 陣形
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(Formation))]
    public class Formation : MonoBehaviour
    {
        private MapManager _mapManager;

        public GridValue BaseGrid { get; private set; }
        private readonly Dictionary<int, HeroUnitModel> _heroMap = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public void Construct(
            MapManager mapManager
        )
        {
            _mapManager = mapManager;
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            BaseGrid = new GridValue(GameConst.InitX, GameConst.InitY);
        }
        
        /// <summary>
        /// Heroの追加
        /// </summary>
        public void AddHero(int index, HeroUnitModel heroModel)
        {
            _heroMap.Add(index, heroModel); 
        }
    }
}