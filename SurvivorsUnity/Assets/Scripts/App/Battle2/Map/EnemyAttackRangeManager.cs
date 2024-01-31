using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.Battle2.Core;
using App.Battle2.Facades;
using App.Battle2.Libs;
using App.Battle2.Map.Cells;
using App.Battle2.Units;
using App.Battle2.Units.Enemy;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle2.Map
{
    /// <summary>
    /// 敵攻撃範囲管理
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(EnemyAttackRangeManager))]
    public class EnemyAttackRangeManager : MonoBehaviour
    {
        [SerializeField] private EnemyAttackCell enemyAttackCell;
        [SerializeField] private Transform displayLayer;

        private UnitManger2 unitManger2;
        private GameObjectPool<EnemyAttackCell> _cellPool;
        private readonly HashSet<EnemyAttackCell> _hexCellSets = new();

        private bool _isAllShown = false;
        private EnemyUnitModel2 _focusedEnemy = null;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            UnitManger2 unitManger2
        )
        {
            this.unitManger2 = unitManger2;
            _cellPool = new GameObjectPool<EnemyAttackCell>(enemyAttackCell, displayLayer);

            BattleState.Facade.FocusedUnit.Subscribe(OnFocusedUnit).AddTo(this);
            BattleOperation.Facade.Common.SwitchEnemyAttackRange.Subscribe(_ => SwitchAllShow()).AddTo(this);
        }

        /// <summary>
        /// ユニットフォーカス
        /// </summary>
        private void OnFocusedUnit(IUnitModel2 unitModel2)
        {
            if (unitModel2 is not EnemyUnitModel2 enemyModel)
            {
                //敵ユニットでない場合は
                //全表示中でなければ非表示にして処理を抜ける
                if (!_isAllShown)
                {
                    HideRanges(); 
                }
                _focusedEnemy = null;
                return;
            }
            
            _focusedEnemy = enemyModel;
            if (_isAllShown)
            {
                return;
            }
            ShowRanges(enemyModel.UnitId);
        }

        /// <summary>
        /// 全表示のSwitch
        /// </summary>
        private void SwitchAllShow()
        {
            _isAllShown = !_isAllShown;
            if (_isAllShown)
            {
                ShowRanges();
            }
            else
            {
                //表示OFF
                //Focusしている敵がいればその敵の表示
                //いなければ非表示
                if (_focusedEnemy != null)
                {
                    ShowRanges(_focusedEnemy.UnitId);
                }
                else
                {
                    HideRanges();
                }
            }
        }
        
        /// <summary>
        /// 表示
        /// </summary>
        private void ShowRanges(uint unitId = GameConst.InvalidUnitId)
        {
            //古いもの削除
            foreach (var oldCell in _hexCellSets)
            {
                _cellPool.Return(oldCell);
            }
            _hexCellSets.Clear();

            IEnumerable<HexCell> hexCells;
            if (unitId == GameConst.InvalidUnitId)
            {
                //ID指定なしは全敵
                hexCells = unitManger2.AllAliveEnemies
                    .SelectMany(x => x.AttackRangeCell.Value)
                    .Distinct();
            }
            else
            {
                if (!unitManger2.TryGetEnemyModelById(unitId, out var enemy))
                {
                    return; 
                }
                hexCells = enemy.AttackRangeCell.Value;
            }
            foreach (var hexCell in hexCells)
            {
                var cell = _cellPool.Rent();
                cell.transform.localPosition = hexCell.Position;
                _hexCellSets.Add(cell);
            }
            displayLayer.gameObject.SetActive(true);
        }

        /// <summary>
        /// 非表示
        /// </summary>
        private void HideRanges()
        {
            displayLayer.gameObject.SetActive(false);
        }
    }
}