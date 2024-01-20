using System;
using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.Battle.Core;
using VContainer;
using Cysharp.Threading.Tasks;
using UniRx;

namespace App.Battle.UI.BattleLog
{
    /// <summary>
    /// バトルログSystem
    /// </summary>
    [ContainerRegister(typeof(BattleLogManager))]
    public class BattleLogManager
    {
        private readonly CompositeDisposable _disposable = new();
        private readonly BattleLogView _logView;

        private readonly List<string> _logList = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public BattleLogManager(
            BattleEventHub eventHub,
            BattleLogView logView
        )
        {
            _logView = logView;
            _logView.SetupAsync().Forget();

            eventHub.Subscribe<BattleEvents.OnMessageLog>(MessageLog).AddTo(_disposable);
            eventHub.Subscribe<BattleEvents.OnShipAttacked>(ShipAttacked).AddTo(_disposable);
            eventHub.Subscribe<BattleEvents.OnEnemyAttacked>(EnemyAttacked).AddTo(_disposable);
            
            // UniTask.Void(async () =>
            // {
            //     UniTask.Delay(TimeSpan.FromSeconds(3));
            //     var list = Enumerable.Range(0, 20);
            //     int i = 0;
            //     foreach (var v in list)
            //     {
            //         await UniTask.Delay(TimeSpan.FromSeconds(1));
            //         await _logView.AddLine($"バトルログ{i}");
            //         i++;
            //     }
            // });
        }

        
        /// <summary>
        /// OnMessageLog
        /// </summary>
        private void MessageLog(BattleEvents.OnMessageLog evt)
        {
            _logList.Add(evt.Text);    
            _logView.AddLine(evt.Text).Forget();
        }

        /// <summary>
        /// 船攻撃
        /// </summary>
        private void ShipAttacked(BattleEvents.OnShipAttacked evt)
        {
            OnAttackResult(evt.Args); 
        }
        
        /// <summary>
        /// 敵攻撃
        /// </summary>
        private void EnemyAttacked(BattleEvents.OnEnemyAttacked evt)
        {
            OnAttackResult(evt.Args); 
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
            _logList.Add(log);    
            _logView.AddLine(log).Forget();
        }
    }
}