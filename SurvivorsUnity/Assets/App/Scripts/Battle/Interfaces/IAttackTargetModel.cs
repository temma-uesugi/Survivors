using App.AppCommon;
using App.Battle.Map.Cells;
using App.Battle.ValueObjects;
using UniRx;
using UnityEngine;

namespace App.Battle.Interfaces
{

    //Note: ObjectとUnit両方の大元のクラスを作ってやった方がよいかも？ 
    
    /// <summary>
    /// 攻撃対象
    /// </summary>
    public interface IAttackTargetModel
    {
        /// <summary>
        /// Hexセル位置
        /// </summary>
        IReadOnlyReactiveProperty<HexCell> Cell { get; }

        /// <summary>
        /// Grid
        /// </summary>
        GridValue Grid { get; }

        /// <summary>
        /// Position
        /// </summary>
        Vector3 Position { get; }
        
        /// <summary>
        /// ダメージを与える
        /// </summary>
        void DealDamage(int damage, AttackType attackType);
        
        /// <summary>
        /// 被ダメ計算
        /// </summary>
        int CalcDamaged(float damage, AttackType attackType);
        
        /// <summary>
        /// ID
        /// </summary>
        uint Id { get; }
        
        /// <summary>
        /// Label
        /// </summary>
        string Label { get; }
    }
}