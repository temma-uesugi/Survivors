using System.Linq;
using App.AppCommon;
using App.Battle.Core;
using App.Battle.Interfaces;
using App.Battle.Map;
using App.Battle.Map.Cells;
using App.Battle.Units;
using App.Battle.Units.Enemy;
using App.Battle.Units.Ship;
using Cysharp.Threading.Tasks;
using VContainer;

namespace App.Battle.Facades
{
    /// <summary>
    /// 移動関連
    /// </summary>
    [ContainerRegister(typeof(BattleMove))]
    public class BattleMove
    {
        public static BattleMove Facade { get; private set; }
        
        private readonly UnitManger _unitManger;
        private readonly HexMapManager _mapManager;
        private readonly HexMapMoveChecker _moveChecker;
        private readonly BattleEventHub _eventHub;
       
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public BattleMove(
            UnitManger unitManger,
            HexMapManager mapManager,
            HexMapMoveChecker moveChecker,
            BattleEventHub eventHub
        )
        {
            _unitManger = unitManger;
            _mapManager = mapManager;
            _moveChecker = moveChecker;
            _eventHub = eventHub;

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
            var enemy = _unitManger.GetEnemyModelById(args.EnemyId);
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
            if (!_unitManger.TryGetShipModelById(shipUnitId, out var ship) || ship.IsActionEnd.Value)
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
            if (!_unitManger.TryGetShipModelById(shipUnitId, out var ship) || ship.IsActionEnd.Value)
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
            if (!_unitManger.TryGetUnit(unitId, out var unit))
            {
                return; 
            }

            if (unit is ShipUnitModel ship)
            {
                ship.Move(moveCell, dir, 0);
                // _eventHub.Publish(new BattleEvents.OnShipMovedEvent
                // {
                //     Ship = ship,
                // });
            }
            else if (unit is EnemyUnitModel enemy)
            {
                enemy.Move(moveCell);
                _eventHub.Publish(new BattleEvents.OnEnemyMovedEvent
                {
                    Enemy = enemy,
                });
            }
    #endif
        }
    }
}