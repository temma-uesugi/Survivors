using System;
using System.Collections.Generic;
using App.AppCommon;
using App.Battle2.Core;
using App.Battle2.ValueObjects;
using Constants;
using UniRx;

namespace App.Battle2.Turn
{
    /// <summary>
    /// 天気管理
    /// </summary>
    [ContainerRegisterAttribute2(typeof(WeatherManager))]
    public class WeatherManager : ITurnManager<WeatherValue>
    {
        /// <summary>
        /// 風向き抽出テーブル
        /// </summary>
        private readonly HashSet<WeatherDrawingData> _drawingTable = new()
        {
            new(WeatherType.Sun, 4), //晴れ
            new(WeatherType.Rain, 2), // 雨
            new(WeatherType.Cloud, 4), //曇り
            new(WeatherType.Fog, 2), //霧
            new(WeatherType.Storm, 1), //嵐
            new(WeatherType.Thunderstorm, 1), //雷雨
        };

        private readonly CompositeDisposable _disposable = new();
        
        private readonly Queue<WeatherValue> _weathers = new();
        private int _setTurnNo;

        // private readonly GameState _gameState;

        private readonly Subject<(int, WeatherValue)> _onAddTurnLine = new();
        public IObservable<(int turnLineIndex, WeatherValue data)> OnAddTurnLine => _onAddTurnLine;

        private readonly Subject<WeatherValue> _onTurnProceed = new();
        public IObservable<WeatherValue> OnTurnProceed => _onTurnProceed;

        public WeatherValue CurrentValue { get; private set; }

        // /// <summary>
        // /// コンストラクタ
        // /// </summary>
        // [Inject]
        // public WeatherManager(
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
                var weather = SetWeather();
                _onAddTurnLine.OnNext((_setTurnNo, weather));
            }
        }

        /// <summary>
        /// 天気をセット
        /// </summary>
        private WeatherValue SetWeather()
        {
            var drawWeather = _drawingTable.DrawByWeight();
            var weather = new WeatherValue(drawWeather.Type);
            _weathers.Enqueue(weather);
            return weather;
        }

        /// <summary>
        /// 進行
        /// </summary>
        public WeatherValue Proceed()
        {
            //補充
            var newWeather = SetWeather();
            _onTurnProceed.OnNext(newWeather);
            _setTurnNo++;
            
            CurrentValue = _weathers.Dequeue();
            return CurrentValue;
        }
    }
}