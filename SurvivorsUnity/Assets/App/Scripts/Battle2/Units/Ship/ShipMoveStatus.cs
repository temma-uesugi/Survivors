// using System;
//
// namespace App.Game.Objects.Ship
// {
//     /// <summary>
//     /// 移動ステータス
//     /// </summary>
//     public class ShipMoveStatus
//     {
//         //速度
//         public int MaxSpeed { get; init; }
//         private float _speed;
//         public int Speed => (int)_speed;
//         //方向
//         public DirectionType Direction;
//         //移動可能方向
//         public DirectionType MovableDir { get; private set; }
//
//         /// <summary>
//         /// コンストラクタ
//         /// </summary>
//         public ShipMoveStatus(int speed, DirectionType direction)
//         {
//             MaxSpeed = speed;
//             _speed = speed;
//             Direction = direction;
//             //TODO
//             MovableDir = direction;
//         }
//
//         /// <summary>
//         /// リセット
//         /// </summary>
//         public void Reset()
//         {
//             _speed = MaxSpeed;
//         }
//
//         /// <summary>
//         /// 速度減衰
//         /// </summary>
//         public void ReduceSpeed(float reduced)
//         {
//             _speed = Math.Max(0, _speed - reduced);
//         }
//
//         /// <summary>
//         /// 移動可能方向セット
//         /// </summary>
//         public void SetMovableDir(DirectionType movableDir)
//         {
//             MovableDir = movableDir;
//         }
//     }
// }