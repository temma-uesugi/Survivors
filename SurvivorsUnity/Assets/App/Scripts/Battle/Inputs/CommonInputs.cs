using System;
using App.ActionInputs;
using App.Inputs;
using UniRx;
using UnityEngine.InputSystem;

namespace App.Battle.Inputs
{
    /// <summary>
    /// バトル全般操作
    /// </summary>
    public class CommonInputs : IActionInputs
    {
        private readonly CompositeDisposable _disposable = new();
        private BattleInputs.CommonActions _actions;
        
        private readonly Subject<Unit> _zoomUpCamera = new();
        public IObservable<Unit> ZoomUpCamera => _zoomUpCamera;
        private readonly Subject<Unit> _zoomUpCameraStop = new();
        public IObservable<Unit> ZoomUpCameraStop => _zoomUpCamera;
        
        private readonly Subject<Unit> _zoomDownCamera = new();
        public IObservable<Unit> ZoomDownCamera => _zoomDownCamera;
        private readonly Subject<Unit> _zoomDownCameraStop = new();
        public IObservable<Unit> ZoomDownCameraStop => _zoomDownCameraStop;
        
        private readonly Subject<Unit> _switchEnemyAttackRange = new();
        public IObservable<Unit> SwitchEnemyAttackRange => _switchEnemyAttackRange;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonInputs(BattleInputs battleInputs)
        {
            _actions = battleInputs.Common; 
            
            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.ZoomUpCamera.performed += h,
                h => _actions.ZoomUpCamera.performed -= h
            )
            .Subscribe(_ => _zoomUpCamera.OnNext(Unit.Default))
            .AddTo(_disposable);
            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.ZoomUpCamera.canceled += h,
                h => _actions.ZoomUpCamera.canceled -= h
            )
            .Subscribe(_ => _zoomUpCameraStop.OnNext(Unit.Default))
            .AddTo(_disposable);
            
            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.ZoomDownCamera.performed += h,
                h => _actions.ZoomDownCamera.performed -= h
            )
            .Subscribe(_ => _zoomDownCamera.OnNext(Unit.Default))
            .AddTo(_disposable);
            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.ZoomDownCamera.canceled += h,
                h => _actions.ZoomDownCamera.canceled -= h
            )
            .Subscribe(_ => _zoomDownCameraStop.OnNext(Unit.Default))
            .AddTo(_disposable);
            
            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.SwitchEnemyAttackRange.performed += h,
                h => _actions.SwitchEnemyAttackRange.performed -= h
            )
            .Subscribe(_ => _switchEnemyAttackRange.OnNext(Unit.Default))
            .AddTo(_disposable);
        }

        /// <summary>
        /// 有効/無効切り替え
        /// </summary>
        public void SetEnable(bool isEnable)
        {
            if (isEnable)
            {
                _actions.Enable(); 
            }
            else
            {
                _actions.Disable();
            }
        }
        
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _disposable.Dispose(); 
        }
    }
}