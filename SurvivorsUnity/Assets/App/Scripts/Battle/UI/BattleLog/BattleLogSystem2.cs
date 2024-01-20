using System;
using App.AppCommon;
using App.Battle.Core;
using App.Battle.Units;
using UniRx;
using VContainer;

namespace App.Battle.UI.BattleLog
{
    /// <summary>
    /// バトルログSystem
    /// </summary>
    [ContainerRegister(typeof(BattleLogSystem2))]
    public class BattleLogSystem2 : IDisposable
    {
        private readonly CompositeDisposable _disposable = new();

        private UnitManger _unitManger;
        private BattleLogPastView2 _pastView2;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            UnitManger unitManger,
            BattleLogPastView2 pastView2,
            BattleEventHub eventHub
        )
        {
            _unitManger = unitManger;
            _pastView2 = pastView2;
            
            _pastView2.Setup();
            eventHub.Subscribe<BattleEvents.OnMessageLog>(AddLog).AddTo(_disposable);
            eventHub.Subscribe<BattleEvents.OnShipAttacked>(evt =>
            {
                OnAttackResult(evt.Args);
            }).AddTo(_disposable);
            eventHub.Subscribe<BattleEvents.OnEnemyAttacked>(evt =>
            {
                OnAttackResult(evt.Args);
            }).AddTo(_disposable);
        }

        /// <summary>
        /// ログ追加
        /// </summary>
        private void AddLog(BattleEvents.OnMessageLog evt) => AddLog(evt.Text);

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
        private void OnAttackResult(BattleEvents.AttackArgs args)
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