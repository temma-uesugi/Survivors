using System;
using App.AppCommon;
using App.Battle2.Core;
using App.Battle2.Units;
using Master.Constants;
using UniRx;
using VContainer;

namespace App.Battle2.UI.BattleLog
{
    /// <summary>
    /// バトルログSystem
    /// </summary>
    [ContainerRegisterAttribute2(typeof(BattleLogSystem2))]
    public class BattleLogSystem2 : IDisposable
    {
        private readonly CompositeDisposable _disposable = new();

        private UnitManger2 unitManger2;
        private BattleLogPastView2 _pastView2;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            UnitManger2 unitManger2,
            BattleLogPastView2 pastView2,
            BattleEventHub2 eventHub2
        )
        {
            this.unitManger2 = unitManger2;
            _pastView2 = pastView2;
            
            _pastView2.Setup();
            eventHub2.Subscribe<BattleEvents2.OnMessageLog>(AddLog).AddTo(_disposable);
            eventHub2.Subscribe<BattleEvents2.OnShipAttacked>(evt =>
            {
                OnAttackResult(evt.Args);
            }).AddTo(_disposable);
            eventHub2.Subscribe<BattleEvents2.OnEnemyAttacked>(evt =>
            {
                OnAttackResult(evt.Args);
            }).AddTo(_disposable);
        }

        /// <summary>
        /// ログ追加
        /// </summary>
        private void AddLog(BattleEvents2.OnMessageLog evt) => AddLog(evt.Text);

        /// <summary>
        /// ログ追加
        /// </summary>
        private void AddLog(string log)
        {
            _pastView2.AddLog(log);
        }
        
        /// <summary>
        /// 攻撃結果
        /// </summary>
        private void OnAttackResult(BattleEvents2.AttackArgs args)
        {
            var attacker = args.AttackerUnit;
            var target = args.TargetUnit;
            var log = args.Type switch
            {
                AttackType.Bomb => $"[Bomb] {attacker.Label} -> {target.Label} {args.Damage}Damaged",
                AttackType.Slash => $"[Slash] {attacker.Label} -> {target.Label} {args.Damage}Damaged",
                AttackType.Assault => $"[Assault] {attacker.Label} -> {target.Label} {args.Damage}Damaged",
                AttackType.EnemyAttack => $"[EnemyAttack] {attacker.Label} -> {target.Label}, {args.Damage}Damaged",
                _ => string.Empty,
            };
                
            if (string.IsNullOrEmpty(log))
            {
                return;
            }
            AddLog(log);
        }
        
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}