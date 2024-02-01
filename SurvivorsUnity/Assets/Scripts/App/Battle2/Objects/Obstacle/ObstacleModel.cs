using System;
using App.AppCommon;
using App.Battle2.Interfaces;
using App.Battle2.Map.Cells;
using App.Battle2.ValueObjects;
using Master.Constants;
using UniRx;
using UnityEngine;

namespace App.Battle2.Objects.Obstacle
{
    /// <summary>
    /// 障害物Model
    /// </summary>
    public class ObstacleModel : IObjectModel, IAttackTargetModel, IBlockBombModel
    {
        public uint ObjectId { get; }
        public uint Id => ObjectId;
        public string Label => "障害物";
      
        public bool IsAlive { get; private set; }
     
        public int CalcDamaged(float damage, AttackType attackType) => 1;
        
        //位置
        private readonly ReactiveProperty<HexCell> _cell;
        public IReadOnlyReactiveProperty<HexCell> Cell => _cell;
        public GridValue Grid => _cell.Value.Grid;
        public Vector3 Position => _cell.Value.Position;

        private readonly Subject<uint> _onDestroyed = new();
        public IObservable<uint> OnDestroyed => _onDestroyed;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ObstacleModel(
            uint objectId,
            HexCell cell
        )
        {
            _cell = new (cell);
            ObjectId = objectId;
            IsAlive = true;
        }

        /// <summary>
        /// ダメージを与える
        /// </summary>
        public void DealDamage(int damage, AttackType attackType)
        {
            if (attackType != AttackType.Bomb) return;
            IsAlive = false;
            _onDestroyed.OnNext(ObjectId);
        }
        
    }
}