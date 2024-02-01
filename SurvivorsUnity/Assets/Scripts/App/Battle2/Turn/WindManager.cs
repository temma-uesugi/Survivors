using System;
using System.Collections.Generic;
using App.AppCommon;
using App.Battle2.Core;
using App.Battle2.Map;
using App.Battle2.ValueObjects;
using FastEnumUtility;
using Master.Constants;
using UniRx;

namespace App.Battle2.Turn
{
    /// <summary>
    /// 風管理
    /// </summary>
    [ContainerRegisterAttribute2(typeof(WindManager))]
    public class WindManager : ITurnManager<WindValue>
    {
        /// <summary>
        /// 風向き抽出テーブル
        /// </summary>
        private Dictionary<int, WindDirDrawingData> _directionDrawingTable = new()
        {
            {DirectionType.Right.ToInt32(), new(DirectionType.Right, GameConst.WindDirDrawDefaultWeight)},
            {DirectionType.TopRight.ToInt32(), new(DirectionType.TopRight, GameConst.WindDirDrawDefaultWeight)},
            {DirectionType.TopLeft.ToInt32(), new(DirectionType.TopLeft, GameConst.WindDirDrawDefaultWeight)},
            {DirectionType.Left.ToInt32(), new(DirectionType.Left, GameConst.WindDirDrawDefaultWeight)},
            {DirectionType.BottomLeft.ToInt32(), new(DirectionType.BottomLeft, GameConst.WindDirDrawDefaultWeight)},
            {DirectionType.BottomRight.ToInt32(), new(DirectionType.BottomRight, GameConst.WindDirDrawDefaultWeight)},
            {DirectionType.None.ToInt32(), new(DirectionType.None, GameConst.WindDirDrawDefaultWeight)},
        };

        /// <summary>
        /// 風速抽出テーブル
        /// </summary>
        private readonly WindStrDrawingData[] _strengthDrawingTable = new WindStrDrawingData[]
        {
            new WindStrDrawingData(1, 2),
            new WindStrDrawingData(2, 2),
            new WindStrDrawingData(3, 1)
        };

        private readonly CompositeDisposable _disposable = new();
        
        //風向きを変えるターン数
        private const int ChangeTurnAmount = 3;
        private int _setTurnNo;
        private readonly Queue<WindValue> _winds = new();

        // private readonly GameState _gameState;

        private readonly Subject<(int, WindValue)> _onAddTurnLine = new();
        public IObservable<(int turnLineIndex, WindValue data)> OnAddTurnLine => _onAddTurnLine;

        private readonly Subject<WindValue> _onTurnProceed = new();
        public IObservable<WindValue> OnTurnProceed => _onTurnProceed;

        public WindValue CurrentValue { get; private set; }
        
        // /// <summary>
        // /// コンストラクタ
        // /// </summary>
        // [Inject]
        // public WindManager(
        //     GameState gameState,
        //     EventHub eventHub
        // )
        // {
        //     _gameState = gameState;
        //
        //     // eventHub.Subscribe<BattleEvents.OnTurnStart>(_ =>
        //     // {
        //     //     OnTurnStart();
        //     // }).AddTo(_disposable);
        //     // eventHub.Subscribe<BattleEvents.OnTurnEnd>(_ =>
        //     // {
        //     //     OnTurnEnd();
        //     // }).AddTo(_disposable);
        // }

        /// <summary>
        /// セットアップ
        /// </summary>
        public void Setup()
        {
            _setTurnNo = 0;
            for (; _setTurnNo < GameConst.PredictedTurnAmount; _setTurnNo++)
            {
                var wind = SetWind();
                _onAddTurnLine.OnNext((_setTurnNo, wind));
            }
        }

        /// <summary>
        /// Windをセット
        /// </summary>
        private WindValue SetWind()
        {
            var drawDir = _directionDrawingTable.Values.DrawByWeight();
            int windStrength = 0;
            if (drawDir.Type != DirectionType.None)
            {
                var drawStr = _strengthDrawingTable.DrawByWeight();
                windStrength = drawStr.Strength;
            }

            var wind = new WindValue(drawDir.Type, windStrength);
            _winds.Enqueue(wind);

            if (_setTurnNo % ChangeTurnAmount == 0)
            {
                foreach (var item in _directionDrawingTable.Values)
                {
                    item.Weight = GameConst.WindDirDrawDefaultWeight;
                }
            }
            else
            {
                HexUtil2.ReflectDrawTable(drawDir.Type, ref _directionDrawingTable);
            }
            return wind;
        }

        /// <summary>
        /// 進行
        /// </summary>
        public WindValue Proceed()
        {
            //補充
            var newWind = SetWind();
            _onTurnProceed.OnNext(newWind);
            _setTurnNo++;
            
            CurrentValue = _winds.Dequeue();
            return CurrentValue;
        }
    }
}