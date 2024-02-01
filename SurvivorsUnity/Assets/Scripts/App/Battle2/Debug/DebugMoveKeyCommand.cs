using App.Battle2.Core;
using App.Battle2.Facades;
using Master.Constants;
using UnityEngine;
using VContainer;

namespace App.Battle2.Debug
{
    /// <summary>
    /// 方向キーによるデバッグの移動コマンド
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(DebugMoveKeyCommand))]
    public class DebugMoveKeyCommand : MonoBehaviour
    {
#if UNITY_EDITOR

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
        )
        {
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            if (DebugFreeMoveCommand.IsActive)
            {
                return;
            }

            var inputKeyDir = KeyDir.None;
            // if (Input.GetKeyDown(KeyCode.RightArrow))
            // {
            //     inputKeyDir = KeyDir.Right;
            // }
            // else if (Input.GetKeyDown(KeyCode.LeftArrow))
            // {
            //     inputKeyDir = KeyDir.Left;
            // }
            // if (Input.GetKeyDown(KeyCode.UpArrow))
            // {
            //     inputKeyDir = KeyDir.Up;
            // }
            // else if (Input.GetKeyDown(KeyCode.DownArrow))
            // {
            //     inputKeyDir = KeyDir.Down;
            // }
            //
            // if (inputKeyDir == KeyDir.None)
            // {
            //     return;
            // }

            // //選択している船があるかチェック
            // var shipUnitId = _gameState.ActionUnitId;
            // if (shipUnitId == GameConst.InvalidUnitId)
            // {
            //     if (_gameState.SelectUnitId == GameConst.InvalidUnitId)
            //     {
            //         return;
            //     }
            //     _eventMessageSystem.AddEvent(new EventMessages.OnActionStartEvent
            //     {
            //         UnitId = _gameState.SelectUnitId,
            //     });
            //     shipUnitId = _gameState.SelectUnitId;
            // }
            // var shipView = _unitManager.GetShipViewById(shipUnitId);
            // if (shipView == null)
            // {
            //     return;
            // }
            //
            // //シフトと同時押しで方向転換
            // if (Input.GetKey(KeyCode.LeftShift))
            // {
            //     InputChangeDirection(inputKeyDir, shipView.Direction, shipUnitId);
            // }
            // else
            // {
            //     InputMove(inputKeyDir, shipView.Direction, shipUnitId);
            // }
        }

        /// <summary>
        /// 移動入力
        /// </summary>
        private void InputMove(KeyDir keyDir, DirectionType curDir, uint shipUnitId)
        {
            var dir = DirectionType.None;
            if (curDir == DirectionType.Right)
            {
                dir = keyDir switch
                {
                    KeyDir.Right => DirectionType.Right,
                    KeyDir.Left => DirectionType.None,
                    KeyDir.Up => DirectionType.TopRight,
                    KeyDir.Down => DirectionType.BottomRight,
                    _=> DirectionType.None,
                };
            }
            else if (curDir == DirectionType.TopRight)
            {
                dir = keyDir switch
                {
                    KeyDir.Right => DirectionType.Right,
                    KeyDir.Left => DirectionType.TopLeft,
                    KeyDir.Up => DirectionType.TopRight,
                    KeyDir.Down => DirectionType.None,
                    _=> DirectionType.None,
                };
            }
            else if (curDir == DirectionType.TopLeft)
            {
                dir = keyDir switch
                {
                    KeyDir.Right => DirectionType.TopRight,
                    KeyDir.Left => DirectionType.Left,
                    KeyDir.Up => DirectionType.TopLeft,
                    KeyDir.Down => DirectionType.None,
                    _=> DirectionType.None,
                };
            }
            else if (curDir == DirectionType.Left)
            {
                dir = keyDir switch
                {
                    KeyDir.Right => DirectionType.None,
                    KeyDir.Left => DirectionType.Left,
                    KeyDir.Up => DirectionType.TopLeft,
                    KeyDir.Down => DirectionType.BottomLeft,
                    _=> DirectionType.None,
                };
            }
            else if (curDir == DirectionType.BottomLeft)
            {
                dir = keyDir switch
                {
                    KeyDir.Right => DirectionType.BottomRight,
                    KeyDir.Left => DirectionType.Left,
                    KeyDir.Up => DirectionType.None,
                    KeyDir.Down => DirectionType.BottomLeft,
                    _=> DirectionType.None,
                };
            }
            else if (curDir == DirectionType.BottomRight)
            {
                dir = keyDir switch
                {
                    KeyDir.Right => DirectionType.Right,
                    KeyDir.Left => DirectionType.BottomLeft,
                    KeyDir.Up => DirectionType.None,
                    KeyDir.Down => DirectionType.BottomRight,
                    _=> DirectionType.None,
                };
            }

            if (dir == DirectionType.None)
            {
                return;
            }
            
            BattleMove.Facade.InputMove(shipUnitId, dir);
        }

        /// <summary>
        /// 方向転換入力
        /// </summary>
        private void InputChangeDirection(KeyDir keyDir, DirectionType curDir, uint shipUnitId)
        {
            var dir = GetNextDirByArrowKey(keyDir, curDir);
            if (dir == DirectionType.None)
            {
                return;
            }
            
            BattleMove.Facade.InputChangeDirection(shipUnitId, dir);
        }

        /// <summary>
        /// 現在方向から方向キーでどの方向かを割り出す
        /// </summary>
        public static DirectionType GetNextDirByArrowKey(KeyDir keyDir, DirectionType curDir)
        {
            var dir = DirectionType.None;
            if (keyDir == KeyDir.Right)
            {
                dir = curDir switch
                {
                    DirectionType.Right => DirectionType.BottomRight,
                    DirectionType.TopRight => DirectionType.Right,
                    DirectionType.TopLeft => DirectionType.TopRight,
                    DirectionType.Left => DirectionType.TopLeft,
                    DirectionType.BottomLeft => DirectionType.Left,
                    DirectionType.BottomRight => DirectionType.BottomLeft,
                    _=> DirectionType.None,
                };
            }
            else if (keyDir == KeyDir.Left)
            {
                dir = curDir switch
                {
                    DirectionType.Right => DirectionType.TopRight,
                    DirectionType.TopRight => DirectionType.TopLeft,
                    DirectionType.TopLeft => DirectionType.Left,
                    DirectionType.Left => DirectionType.BottomLeft,
                    DirectionType.BottomLeft => DirectionType.BottomRight,
                    DirectionType.BottomRight => DirectionType.Right,
                    _=> DirectionType.None,
                };
            }
            else if (keyDir == KeyDir.Up)
            {
                dir = curDir switch
                {
                    DirectionType.Right => DirectionType.TopRight,
                    DirectionType.TopRight => DirectionType.None,
                    DirectionType.TopLeft => DirectionType.None,
                    DirectionType.Left => DirectionType.TopLeft,
                    DirectionType.BottomLeft => DirectionType.TopLeft,
                    DirectionType.BottomRight => DirectionType.TopRight,
                    _=> DirectionType.None,
                };
            }
            else if (keyDir == KeyDir.Down)
            {
                dir = curDir switch
                {
                    DirectionType.Right => DirectionType.BottomRight,
                    DirectionType.TopRight => DirectionType.BottomRight,
                    DirectionType.TopLeft => DirectionType.BottomLeft,
                    DirectionType.Left => DirectionType.BottomLeft,
                    DirectionType.BottomLeft => DirectionType.None,
                    DirectionType.BottomRight => DirectionType.None,
                    _=> DirectionType.None,
                };
            }
            return dir;
        }
#endif
    }
}