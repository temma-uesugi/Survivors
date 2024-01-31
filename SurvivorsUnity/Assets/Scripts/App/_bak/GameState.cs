// using System;
// using App.AppCommon;
// using App.Core;
// using App.Game.Map;
// using App.Game.Units;
// using App.Game.ValueObjects;
// using UniRx;
// using UnityEngine;
// using VContainer;
//
// namespace App.Game.Core
// {
//     /// <summary>
//     /// ゲームの状態管理
//     /// </summary>
//     [ContainerRegister(typeof(GameState), SceneType.Battle, SceneType.Test)]
//     public class GameState : IDisposable
//     {
//         private readonly CompositeDisposable _disposable = new();
//
//         //ラウンド数
//         public int RoundNo { get; private set; }
//         //残りラウンド数
//         public int RoundTurnRemaining { get; private set; }
//         //ターン数
//         public int TurnNo { get; private set; }
//         //行動しているユニットのID
//         public uint ActionUnitId { get; private set; }
//         //ターンの風
//         public WindValue TurnWind { get; private set; }
//         //ターンの天気
//         public WeatherValue TurnWeather { get; private set; }
//         //ターンの敵
//         public IEnemyTurnValue TurnEnemy { get; private set; }
//         //フェイズ
//         public PhaseType Phase { get; private set; }
//         public bool OnShipAction => ActionUnitId != GameConst.InvalidUnitId;
//
//         //フォーカスされているHexCell
//         private readonly ReactiveProperty<HexCell> _focusedHexCell = new();
//         public IReadOnlyReactiveProperty<HexCell> FocusedHexCell => _focusedHexCell;
//         //選択されている船Unit
//         private readonly ReactiveProperty<ShipUnitModel> _selectedShipUnit = new();
//         public IReadOnlyReactiveProperty<ShipUnitModel> SelectedShipUnit => _selectedShipUnit;
//         //フォーカスされているUnit
//         private readonly ReactiveProperty<IUnitModel> _focusedUnit = new();
//         public IReadOnlyReactiveProperty<IUnitModel> FocusedUnit => _focusedUnit;
//
//         /// <summary>
//         /// コンストラクタ
//         /// </summary>
//         [Inject]
//         public GameState()
//         {
//             // _cameraSize = new ReactiveProperty<float>(GameConst.DefaultBattleCameraSize);
//             // var bag = DisposableBag.CreateBuilder();
//             // eventSub.Subscribe(x =>
//             // {
//             //     switch (x)
//             //     {
//             //         case EventMessages.OnActionStartEvent evt:
//             //         {
//             //             ActionUnitId = evt.ShipUnitId;
//             //             break;
//             //         }
//             //     }
//             // }).AddTo(bag);
//             // _disposable = bag.Build();
//             // eventHub.Subscribe<BattleEvents.OnRoundStart>(_ =>
//             // {
//             //     OnRoundStart();
//             // }).AddTo(_disposable);
//             // eventHub.Subscribe<BattleEvents.OnTurnEnd>(_ =>
//             // {
//             //     OnTurnEnd();
//             // }).AddTo(_disposable);
//             // eventHub.Subscribe<BattleEvents.OnPhaseStart>(x =>
//             // {
//             //     OnPhaseStart(x.Phase);
//             // }).AddTo(_disposable);
//             // eventHub.Subscribe<BattleEvents.OnPhaseEnd>(x =>
//             // {
//             //     OnPhaseEnd(x.Phase, x.ActionId);
//             // }).AddTo(_disposable);
//         }
//        
//         /// <summary>
//         /// OnRoundStart
//         /// </summary>
//         private void OnRoundStart()
//         {
//             RoundTurnRemaining = GameConst.RoundTurnAmount;
//         }
//
//         /// <summary>
//         /// OnTurnStart
//         /// </summary>
//         private void OnTurnStart()
//         {
//             _selectedShipUnit.Value = null;
//             ActionUnitId = GameConst.InvalidUnitId;
//             TurnNo++;
//             //TODO
//             // TurnWind = wind;
//             // TurnWeather = weather;
//             // TurnEnemy = enemyTurnValue;
//         }
//
//         /// <summary>
//         /// OnTurnEnd
//         /// </summary>
//         private void OnTurnEnd()
//         {
//             RoundTurnRemaining--;
//         }
//
//         /// <summary>
//         /// OnPhaseStart
//         /// </summary>
//         private void OnPhaseStart(PhaseType startPhase)
//         {
//             Phase = startPhase;
//         }
//
//         /// <summary>
//         /// OnPhaseEnd
//         /// </summary>
//         private void OnPhaseEnd(PhaseType phase, uint actionUnitId)
//         {
//             if (phase == PhaseType.PlayerPhase)
//             {
//                 _selectedShipUnit.Value = null;
//                 ActionUnitId = GameConst.InvalidUnitId;
//             }
//         }
//
//         /// <summary>
//         /// ラウンド状況更新
//         /// </summary>
//         public void UpdateRoundStatus()
//         {
//             RoundTurnRemaining = GameConst.RoundTurnAmount;
//         }
//         
//         /// <summary>
//         /// フェイズ状況更新
//         /// </summary>
//         public void UpdatePhaseStatus(PhaseType phaseType)
//         {
//             Phase = phaseType;
//             _selectedShipUnit.Value = null;
//         }
//
//         /// <summary>
//         /// ターン終了
//         /// </summary>
//         public void EndTurn()
//         {
//             RoundTurnRemaining--;
//         }
//         
//         /// <summary>
//         /// ターン状況を更新
//         /// </summary>
//         public void UpdateTurnStatus(WindValue wind, WeatherValue weather, IEnemyTurnValue turnEnemy)
//         {
//             TurnWind = wind;
//             TurnWeather = weather;
//             TurnEnemy = turnEnemy;
//             TurnNo++;
//         }
//         
//         /// <summary>
//         /// フォーカスセルの更新
//         /// </summary>
//         public void UpdateFocusedHexCell(HexCell cell) => _focusedHexCell.Value = cell;
//
//         /// <summary>
//         /// 選択Shipの更新
//         /// </summary>
//         public void UpdateSelectedShipUnit(ShipUnitModel ship) => _selectedShipUnit.Value = ship;
//
//         /// <summary>
//         /// FocusUnitの更新
//         /// </summary>
//         public void UpdateFocusedUnit(IUnitModel unit) => _focusedUnit.Value = unit;
//         
//         /// <summary>
//         /// Dispose
//         /// </summary>
//         public void Dispose()
//         {
//             _disposable.Dispose();
//         }
//     }
// }