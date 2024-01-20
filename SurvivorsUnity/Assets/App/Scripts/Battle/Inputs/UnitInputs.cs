using System;
using App.ActionInputs;
using App.Inputs;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Battle.Inputs
{
    /// <summary>
    /// Unit操作
    /// </summary>
    public class UnitInputs : IActionInputs
    {
        private readonly CompositeDisposable _disposable = new();
        private BattleInputs.UnitActions _actions;

        private readonly Subject<Vector2> _move = new();
        private readonly Subject<Unit> _stopMove = new();
        public IObservable<Vector2> Move => _move;
        public IObservable<Unit> StopMove => _stopMove;
        
        private readonly Subject<Unit> _decide = new();
        public IObservable<Unit> Decide => _decide;
        
        private readonly Subject<Unit> _cancel = new();
        public IObservable<Unit> Cancel => _cancel;

        private readonly Subject<Unit> _skill = new();
        public IObservable<Unit> Skill => _skill;

        private readonly Subject<Unit> _switchTargetL = new();
        public IObservable<Unit> SwitchTargetL => _switchTargetL;
        private readonly Subject<Unit> _switchTargetR = new();
        public IObservable<Unit> SwitchTargetR => _switchTargetR;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UnitInputs(BattleInputs battleInputs)
        {
            _actions = battleInputs.Unit;

            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.Move.performed += h,
                h => _actions.Move.performed -= h
            )
            .Subscribe(x => _move.OnNext(x.ReadValue<Vector2>()))
            .AddTo(_disposable);
            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.Move.canceled += h,
                h => _actions.Move.canceled -= h
            )
            .Subscribe(_ => _stopMove.OnNext(Unit.Default))
            .AddTo(_disposable);
            
            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.Decide.performed += h,
                h => _actions.Decide.performed -= h
            )
            .Subscribe(_ => _decide.OnNext(Unit.Default))
            .AddTo(_disposable);
            
            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.Cancel.performed += h,
                h => _actions.Cancel.performed -= h
            )
            .Subscribe(_ => _cancel.OnNext(Unit.Default))
            .AddTo(_disposable);

            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.Skill.performed += h,
                h => _actions.Skill.performed -= h
            )
            .Subscribe(_ => _skill.OnNext(Unit.Default))
            .AddTo(_disposable);
            
            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.SwitchTargetL.performed += h,
                h => _actions.SwitchTargetL.performed -= h
            )
            .Subscribe(_ => _switchTargetL.OnNext(Unit.Default))
            .AddTo(_disposable);
            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.SwitchTargetR.performed += h,
                h => _actions.SwitchTargetR.performed -= h
            )
            .Subscribe(_ => _switchTargetR.OnNext(Unit.Default))
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