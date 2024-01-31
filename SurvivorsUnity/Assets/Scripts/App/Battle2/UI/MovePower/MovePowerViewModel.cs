using System;
using App.Battle2.Core;
using App.Battle2.Facades;
using App.Battle2.Units;
using App.Battle2.Units.Ship;
using UniRx;
using VContainer;

namespace App.Battle2.UI.MovePower
{
    /// <summary>
    /// MovePowerのViewModel
    /// </summary>
    [ContainerRegisterAttribute2(typeof(MovePowerViewModel))]
    public class MovePowerViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private IDisposable _movePowerDisposable;
        private readonly UnitManger2 _unitManager;

        //船変更
        private readonly Subject<double> _onChangeShip = new();
        public IObservable<double> OnChangeShip => _onChangeShip;
        //更新
        private readonly ReactiveProperty<double> _movePower = new ReactiveProperty<double>();
        public IReadOnlyReactiveProperty<double> MovePower => _movePower;

        private readonly Subject<Unit> _onShipActionEnd = new();
        public IObservable<Unit> OnShipActionEnd => _onShipActionEnd;

        private readonly Subject<Unit> _onShipReleased = new();
        public IObservable<Unit> OnShipReleased => _onShipReleased;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public MovePowerViewModel(
            UnitManger2 unitManager
        )
        {
            _unitManager = unitManager;
            BattleState.Facade.SelectedShipUnit.Subscribe(SelectShip).AddTo(_disposable);
        }

        /// <summary>
        /// 選択船更新
        /// </summary>
        private void SelectShip(ShipUnitModel2 shipUnitModel2)
        {
            if (shipUnitModel2 == null)
            {
                _movePower.Value = 0;
                _onShipReleased.OnNext(Unit.Default);
                return;
            }
            
            _onChangeShip.OnNext(shipUnitModel2.Status.MovePower.Max);
            _movePowerDisposable = shipUnitModel2.MovePower.Subscribe(UpdateMovePower);
        }

        /// <summary>
        /// 船速度のUpdate
        /// </summary>
        private void UpdateMovePower(double movePower)
        {
            _movePower.Value = movePower;
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            _disposable.Dispose();
            _movePowerDisposable?.Dispose();
        }
    }
}