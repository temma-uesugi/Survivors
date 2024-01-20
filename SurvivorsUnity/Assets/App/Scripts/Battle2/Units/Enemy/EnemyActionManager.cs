using System;
using System.Collections.Generic;
using System.Linq;
using App.Battle2.Core;
using App.Battle2.Facades;
using App.Battle2.Map;
using Cysharp.Threading.Tasks;
using VContainer;

namespace App.Battle2.Units.Enemy
{
    /// <summary>
    /// 敵の行動管理
    /// </summary>
    [ContainerRegisterAttribute2(typeof(EnemyActionManager))]
    public class EnemyActionManager
    {
        private readonly UnitManger _unitManger;
        private readonly HexMapManager _mapManager;
        private List<EnemyUnitModel> _turnEnemyList;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public EnemyActionManager(
            UnitManger unitManger,
            HexMapManager mapManager
        )
        {
            _unitManger = unitManger;
            _mapManager = mapManager;
        }

        /// <summary>
        /// 敵の行動開始
        /// </summary>
        public async UniTask StartEnemyActionAsync()
        {
            _turnEnemyList = _unitManger.AllAliveEnemies.Where(x => x.IsActionable).Reverse().ToList();
            foreach (var enemy in _turnEnemyList)
            {
                enemy.ResetActionCount(); 
            }
            while (_turnEnemyList.Any())
            {
                await ActionAsync();
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            }
        }

        /// <summary>
        /// 行動
        /// </summary>
        private async UniTask ActionAsync()
        {
            var tasks = new List<UniTask>();
            for (int i = _turnEnemyList.Count - 1; i >= 0; i--)
            {
                var enemy = _turnEnemyList[i];
                if (enemy.TargetUnit == null)
                {
                    _turnEnemyList.RemoveAt(i); 
                    continue;
                }

                if (HexUtil2.InRange(enemy.Cell.Value, enemy.TargetUnit.Value.Cell.Value, enemy.AttackRange))
                {
                    //攻撃範囲内
                    if (enemy.AttackCountRest == 0)
                    {
                        //攻撃回数なければ終了
                        _turnEnemyList.RemoveAt(i); 
                        continue;
                    }
                    tasks.Add(AttackAsync(enemy));
                }
                else
                {
                    //移動 
                    if (enemy.MovePowerRest == 0)
                    {
                        _turnEnemyList.RemoveAt(i); 
                        continue;
                    }
                    tasks.Add(MoveAsync(enemy));
                }
            }

            if (tasks.Any())
            {
                await UniTask.WhenAll(tasks);
            }
        }

        /// <summary>
        /// 攻撃
        /// </summary>
        private async UniTask AttackAsync(EnemyUnitModel enemy)
        {
            await BattleAttack.Facade.EnemyAttackAsync(enemy, enemy.TargetUnit.Value);
            enemy.Attacked();
        }

        /// <summary>
        /// 移動
        /// </summary>
        private async UniTask MoveAsync(EnemyUnitModel enemy)
        {
            //TODO
            //ユニットが重ならないように
            var moveGrid = MapRoutSearch.FindPath(enemy.Cell.Value, enemy.TargetUnit.Value.Cell.Value).Skip(1).First();
            var cell = _mapManager.GetCellByGrid(moveGrid);
            enemy.Move(cell);
        }
    }
}