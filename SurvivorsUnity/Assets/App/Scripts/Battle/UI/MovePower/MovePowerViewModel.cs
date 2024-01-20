using System;
using App.AppCommon;
using App.Battle.Core;
using App.Battle.Facades;
using App.Battle.Units;
using App.Battle.Units.Ship;
using UniRx;
using VContainer;

namespace App.Battle.UI.MovePower
{
    /// <summary>
    /// MovePowerのViewModel
    /// </summary>
    [ContainerRegister(typeof(MovePowerViewModel))]
    public class MovePowerViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private IDisposable _movePowerDisposable;
        private readonly UnitManger _unitManager;

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
            UnitManger unitManager
        )
        {
            _unitManager = unitManager;
            BattleState.Facade.SelectedShipUnit.Subscribe(SelectShip).AddTo(_disposable);
        }

        /// <summary>
        /// 選択船更新
        /// </summary>
        private void SelectShip(ShipUnitModel shipUnitModel)
        {
            if (shipUnitModel == null)
            {
                _movePower.Value = 0;
                _onShipReleased.OnNext(Unit.Default);
                return;
            }
            
            _onChangeShip.OnNext(shipUnitModel.Status.MovePower.Max);
            _movePowerDisposable = shipUnitModel.MovePower.Subscribe(UpdateMovePower);
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