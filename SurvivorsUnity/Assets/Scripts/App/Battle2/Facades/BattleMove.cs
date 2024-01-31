using System.Linq;
using App.AppCommon;
using App.Battle2.Core;
using App.Battle2.Interfaces;
using App.Battle2.Map;
using App.Battle2.Map.Cells;
using App.Battle2.Units;
using App.Battle2.Units.Enemy;
using App.Battle2.Units.Ship;
using Cysharp.Threading.Tasks;
using VContainer;

namespace App.Battle2.Facades
{
    /// <summary>
    /// 移動関連
    /// </summary>
    [ContainerRegisterAttribute2(typeof(BattleMove))]
    public class BattleMove
    {
        public static BattleMove Facade { get; private set; }
        
        private readonly UnitManger2 unitManger2;
        private readonly HexMapManager _mapManager;
        private readonly HexMapMoveChecker _moveChecker;
        private readonly BattleEventHub2 eventHub2;
       
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public BattleMove(
            UnitManger2 unitManger2,
            HexMapManager mapManager,
            HexMapMoveChecker moveChecker,
            BattleEventHub2 eventHub2
        )
        {
            this.unitManger2 = unitManger2;
            _mapManager = mapManager;
            _moveChecker = moveChecker;
            this.eventHub2 = eventHub2;

            Facade = this;
        }
        
        /// <summary>
        /// 移動引数
        /// </summary>
        public record MoveArgs(uint EnemyId, IAttackTargetModel Target, int MovePower = 0, int StepAhead = 0);
       
        /// <summary>
        /// 敵移動
        /// </summary>
        public async UniTask EnemyMoveAsync(MoveArgs args)
        {
            var enemy = unitManger2.GetEnemyModelById(args.EnemyId);
            var path = MapRoutSearch.FindPath(enemy.Cell.Value, args.Target.Cell.Value);
            var moveCnt = (args.MovePower, args.StepAhead) switch
            {
                (var x and > 0 , _) => args.MovePower,
                (0 , var y and > 0) => path.Count - args.StepAhead,
                _=> path.Count
            };
            await enemy.MoveCellsAsync(path.Take(moveCnt));
        }

        /// <summary>
        /// 移動入力
        /// </summary>
        public void InputMove(uint shipUnitId, DirectionType dir)
        {
            if (!unitManger2.TryGetShipModelById(shipUnitId, out var ship) || ship.IsActionEnd.Value)
            {
                return;
            }

            //移動力チェック
            var needPower = _moveChecker.CalcMovePower(ship.Cell.Value,
                ship.Direction.Value, dir);

            var nextCell =
                _mapManager.GetNextCellByDir(ship.Cell.Value, dir);

            ship.Move(nextCell, dir, needPower);
            // _eventHub.Publish(new BattleEvents.OnShipMovedEvent
            // {
            //     Ship = ship,
            // });
        }

        /// <summary>
        /// 方向転換入力
        /// </summary>
        public void InputChangeDirection(uint shipUnitId, DirectionType dir)
        {
            if (!unitManger2.TryGetShipModelById(shipUnitId, out var ship) || ship.IsActionEnd.Value)
            {
                return;
            }
            
            //移動力チェック
            var needPower = _moveChecker.CalcMovePower(ship.Cell.Value,
                ship.Direction.Value, dir);

            var curCell = ship.Cell.Value;
            ship.Move(curCell, dir, needPower);
            // _eventHub.Publish(new BattleEvents.OnShipMovedEvent
            // {
            //     Ship = ship,
            // });
        }

        /// <summary>
        /// デバッグ移動入力
        /// </summary>
        public void DebugInputMove(uint unitId, HexCell moveCell, DirectionType dir)
        {
    #if UNITY_EDITOR
            if (!unitManger2.TryGetUnit(unitId, out var unit))
            {
                return; 
            }

            if (unit is ShipUnitModel2 ship)
            {
                ship.Move(moveCell, dir, 0);
                // _eventHub.Publish(new BattleEvents.OnShipMovedEvent
                // {
                //     Ship = ship,
                // });
            }
            else if (unit is EnemyUnitModel2 enemy)
            {
                enemy.Move(moveCell);
                eventHub2.Publish(new BattleEvents2.OnEnemyMovedEvent
                {
                    Enemy = enemy,
                });
            }
    #endif
        }
    }
}