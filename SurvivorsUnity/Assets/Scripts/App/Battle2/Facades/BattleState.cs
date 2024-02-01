using System;
using Constants;
using App.Battle2.Map.Cells;
using App.Battle2.Units;
using App.Battle2.Units.Ship;
using App.Battle2.ValueObjects;
using UniRx;

namespace App.Battle2.Facades
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
        private readonly ReactiveProperty<ShipUnitModel2> _selectedShipUnit = new();
        public IReadOnlyReactiveProperty<ShipUnitModel2> SelectedShipUnit => _selectedShipUnit;
        //フォーカスされているUnit
        private readonly ReactiveProperty<IUnitModel2> _focusedUnit = new();
        public IReadOnlyReactiveProperty<IUnitModel2> FocusedUnit => _focusedUnit;
        //船移動
        public IObservable<ShipUnitModel2> OnShipMoved => _selectedShipUnit
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
        public void UpdateSelectedShipUnit(ShipUnitModel2 ship) => _selectedShipUnit.Value = ship;

        /// <summary>
        /// FocusUnitの更新
        /// </summary>
        public void UpdateFocusedUnit(IUnitModel2 unit) => _focusedUnit.Value = unit;
    }
}