using System;
using App.AppCommon;
using App.Battle.Map.Cells;
using App.Battle.Units;
using App.Battle.Units.Ship;
using App.Battle.ValueObjects;
using UniRx;

namespace App.Battle.Facades
{
    /// <summary>
    /// バトル状況
    /// </summary>
    public class BattleState
    {
        private static BattleState _instance;
        public static BattleState Facade => _instance ??= new BattleState();

        //ターン数
        public int TurnNo { get; private set; }
        //行動しているユニットのID
        public uint ActionUnitId { get; private set; }
        //ターンの風
        private readonly ReactiveProperty<WindValue> _turnWind = new();
        public IReadOnlyReactiveProperty<WindValue> TurnWind => _turnWind;
        //ターンの天気
        private readonly ReactiveProperty<WeatherValue> _turnWeather = new();
        public IReadOnlyReactiveProperty<WeatherValue> TurnWeather => _turnWeather;
        //フェイズ
        public PhaseType Phase { get; private set; }
        public bool OnShipAction => ActionUnitId != GameConst.InvalidUnitId;

        //フォーカスされているHexCell
        private readonly ReactiveProperty<HexCell> _focusedHexCell = new();
        public IReadOnlyReactiveProperty<HexCell> FocusedHexCell => _focusedHexCell;
        //選択されている船Unit
        private readonly ReactiveProperty<ShipUnitModel> _selectedShipUnit = new();
        public IReadOnlyReactiveProperty<ShipUnitModel> SelectedShipUnit => _selectedShipUnit;
        //フォーカスされているUnit
        private readonly ReactiveProperty<IUnitModel> _focusedUnit = new();
        public IReadOnlyReactiveProperty<IUnitModel> FocusedUnit => _focusedUnit;
        //船移動
        public IObservable<ShipUnitModel> OnShipMoved => _selectedShipUnit
            .Where(x => x != null)
            .SelectMany(x => x.Cell.Skip(1))
            .Select(_ => _selectedShipUnit.Value);

        /// <summary>
        /// フェイズ状況更新
        /// </summary>
        public void UpdatePhaseStatus(PhaseType phaseType)
        {
            Phase = phaseType;
            _selectedShipUnit.Value = null;
        }

        /// <summary>
        /// ターン終了
        /// </summary>
        public void EndTurn()
        {
        }
        
        /// <summary>
        /// ターン状況を更新
        /// </summary>
        public void UpdateTurnStatus(WindValue wind, WeatherValue weather)
        {
            _turnWind.Value = wind;
            _turnWeather.Value = weather;
            TurnNo++;
        }
        
        /// <summary>
        /// フォーカスセルの更新
        /// </summary>
        public void UpdateFocusedHexCell(HexCell cell) => _focusedHexCell.Value = cell;

        /// <summary>
        /// 選択Shipの更新
        /// </summary>
        public void UpdateSelectedShipUnit(ShipUnitModel ship) => _selectedShipUnit.Value = ship;

        /// <summary>
        /// FocusUnitの更新
        /// </summary>
        public void UpdateFocusedUnit(IUnitModel unit) => _focusedUnit.Value = unit;
    }
}