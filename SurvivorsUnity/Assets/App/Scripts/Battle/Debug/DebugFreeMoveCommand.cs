using System;
using App.AppCommon;
using App.Battle.Core;
using App.Battle.Facades;
using App.Battle.Map;
using App.Battle.Units;
using App.Battle.Units.Ship;
using App.Battle.ValueObjects;
using UnityEngine;
using VContainer;

namespace App.Battle.Debug
{
    /// <summary>
    /// 方向キーによるデバッグの移動コマンド
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(DebugFreeMoveCommand))]
    public class DebugFreeMoveCommand : MonoBehaviour
    {
        public static bool IsActive = false;

        private IDisposable _disposable;

        private HexMapManager _mapManager;

        private IUnitView _unitView;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            HexMapManager hexMapManager
        )
        {
            _mapManager = hexMapManager;

            // var bag = DisposableBag.CreateBuilder();
            // _disposable = bag.Build();
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            if (_unitView == null)
            {
                return;
            }

            // if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Escape))
            // {
            //     _unitView = null;
            //     IsActive = false;
            //     return;
            // }
            //
            // var isShiftKey = Input.GetKey(KeyCode.LeftShift);
            // var keyDir = KeyDir.None;
            // if (Input.GetKeyDown(KeyCode.RightArrow))
            // {
            //     keyDir = KeyDir.Right;
            // }
            // else if (Input.GetKeyDown(KeyCode.LeftArrow))
            // {
            //     keyDir = KeyDir.Left;
            // }
            // if (Input.GetKeyDown(KeyCode.UpArrow))
            // {
            //     keyDir = KeyDir.Up;
            // }
            // else if (Input.GetKeyDown(KeyCode.DownArrow))
            // {
            //     keyDir = KeyDir.Down;
            // }
            //
            // if (keyDir == KeyDir.None)
            // {
            //     return;
            // }
            // if (isShiftKey)
            // {
            //     ChangeDir(keyDir);
            // }
            // else
            // {
            //     Move(keyDir);
            // }
        }

        /// <summary>
        /// 移動
        /// </summary>
        private void Move(KeyDir dir)
        {
            var moveGrid = dir switch
            {
                KeyDir.Right => new GridValue(1, 0),
                KeyDir.Left => new GridValue(-1, 0),
                KeyDir.Up => new GridValue(0, 1),
                KeyDir.Down => new GridValue(0, -1),
                _ => GridValue.GridZero,
            };
            var moveCell = _mapManager.GetCellByGrid(_unitView.Cell.Grid + moveGrid);
            if (moveCell == null)
            {
                return;
            }
            BattleMove.Facade.DebugInputMove(_unitView.UnitId, moveCell, _unitView.Direction);
        }

        /// <summary>
        /// 向き変更
        /// </summary>
        private void ChangeDir(KeyDir keyDir)
        {
            if (_unitView is not ShipUnitView)
            {
                return; 
            }
            var dir = DebugMoveKeyCommand.GetNextDirByArrowKey(keyDir, _unitView.Direction);
            if (dir == DirectionType.None)
            {
                return;
            }
            BattleMove.Facade.InputChangeDirection(_unitView.UnitId, dir);
        }

        // /// <summary>
        // /// ユニット選択
        // /// </summary>
        // private void UnitSelected(EventMessages.OnUnitSelectedEvent evt)
        // {
        //     // if (!Input.GetKey(KeyCode.LeftControl))
        //     // {
        //     //     return;
        //     // }
        //     if (_unitManger.TryGetShipViewById(evt.UnitId, out var ship))
        //     {
        //         _unitView = ship;
        //         IsActive = true;
        //     }
        // }

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            // _disposable.Dispose();
        }
    }
}