using System;
using System.Collections.Generic;
using App.ActionInputs;
using App.Battle2.Inputs;
using App.Inputs;
using Cysharp.Threading.Tasks;
using UniRx;

namespace App.Battle2.Facades
{
    /// <summary>
    /// バトル操作
    /// </summary>
    public class BattleOperation
    {
        [Flags]
        public enum ModeType
        {
            None = 0,
            Map = 1 << 0,
            Unit = 1 << 1,
            Menu = 1 << 2,
            Common = Map | Unit,
        }

        private static BattleOperation _instance;
        public static BattleOperation Facade => _instance ??= new BattleOperation();

        private readonly CompositeDisposable _disposable = new();

        public MapInputs Map { get; private set; }
        public UnitInputs Unit { get; private set; }
        public CommonInputs Common { get; private set; }
        public MenuInputs Menu { get; private set; }

        private readonly BattleInputs _battleInputs;
        private readonly Dictionary<ModeType, IActionInputs> _modeActionMap = new();

        private readonly Subject<ModeType> _modeUpdated = new();
        public IObservable<ModeType> ModeUpdated => _modeUpdated;

        public ModeType CurrentMode { get; private set; }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private BattleOperation()
        {
            _battleInputs = new BattleInputs();
            _battleInputs.Enable();

            Map = new MapInputs(_battleInputs);
            Unit = new UnitInputs(_battleInputs);
            Common = new CommonInputs(_battleInputs);
            Menu = new MenuInputs(_battleInputs);

            _modeActionMap.Add(ModeType.Map, Map);
            _modeActionMap.Add(ModeType.Unit, Unit);
            _modeActionMap.Add(ModeType.Common, Common);
            _modeActionMap.Add(ModeType.Menu, Menu);

            BattleState.Facade.SelectedShipUnit
                .Subscribe(x =>
                {
                    SwitchModeAsync(x == null ? ModeType.Map : ModeType.Unit).Forget();
                })
                .AddTo(_disposable);
        }

        /// <summary>
        /// ModeのSwitch
        /// </summary>
        public async UniTask SwitchModeAsync(ModeType mode)
        {
            _modeUpdated.OnNext(mode);
            _battleInputs.Disable();
            
            await UniTask.Yield();
           
            _battleInputs.Enable();
            foreach (var (type, action) in _modeActionMap)
            {
                if (mode == ModeType.None)
                {
                    action.SetEnable(false);
                    continue;
                }
                action.SetEnable(type.HasFlag(mode));
            }
            CurrentMode = mode;

            // await UniTask.Yield();
            //
            // if (_modeActionMap.TryGetValue(mode, out var activeAction))
            // {
            //     activeAction.SetEnable(true);     
            // }
        }
        
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _disposable.Dispose();
            foreach (var action in _modeActionMap.Values)
            {
                action.Dispose();
            }
        }
    }
}