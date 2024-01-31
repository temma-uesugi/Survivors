using System;
using App.AppCommon;
using App.Battle2.Map.Cells;
using App.Battle2.ValueObjects;
using UniRx;
using UnityEngine;

namespace App.Battle2.Units
{
    /// <summary>
    /// UnitのModelインターフェイス
    /// </summary>
    public interface IUnitModel2 : IDisposable
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
        /// Hexセル位置
        /// </summary>
        IReadOnlyReactiveProperty<DirectionType> Direction { get; }

        /// <summary>
        /// 生存しているか
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// UnitId
        /// </summary>
        uint UnitId { get; }

        /// <summary>
        /// Label
        /// </summary>
        string Label { get; }

        /// <summary>
        /// ダメージを与える
        /// </summary>
        void DealDamage(int damage, AttackType attackType);

        /// <summary>
        /// ダメージ計算
        /// </summary>
        DamageCalculator DamageCalculator { get; }

        /// <summary>
        /// 撃破された
        /// </summary>
        IObservable<uint> OnDefeated { get; }

        /// <summary>
        /// 復活した
        /// </summary>
        IObservable<uint> OnRevived { get; }

        /// <summary>
        /// ターン開始
        /// </summary>
        void OnTurnStart();
        
        /// <summary>
        /// ターン終了
        /// </summary>
        void OnTurnEnd();
        
        /// <summary>
        /// 選択中
        /// </summary>
        IReadOnlyReactiveProperty<bool> IsSelected { get; }

        /// <summary>
        /// 行動済み
        /// </summary>
        IReadOnlyReactiveProperty<bool> IsActionEnd { get; }
       
        /// <summary>
        /// ModelのDisposable
        /// </summary>
        CompositeDisposable ModelDisposable { get; }
    }
}