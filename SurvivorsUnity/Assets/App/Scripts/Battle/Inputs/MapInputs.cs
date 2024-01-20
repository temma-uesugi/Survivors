using System;
using App.ActionInputs;
using App.Inputs;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Battle.Inputs
{
    /// <summary>
    /// Map操作
    /// </summary>
    public class MapInputs : IActionInputs
    {
        private readonly CompositeDisposable _disposable = new();
        private BattleInputs.MapActions _actions;
       
        private readonly Subject<Vector2> _move = new();
        private readonly Subject<Unit> _stopMove = new();
        public IObservable<Vector2> Move => _move;
        public IObservable<Unit> StopMove => _stopMove;
      
        private readonly Subject<Unit> _decide = new();
        public IObservable<Unit> Decide => _decide;

        private readonly Subject<Unit> _skill = new();
        public IObservable<Unit> Skill => _skill;
        
        private readonly Subject<Unit> _switchUnitL = new();
        public IObservable<Unit> SwitchUnitL => _switchUnitL;
        private readonly Subject<Unit> _switchUnitR = new();
        public IObservable<Unit> SwitchUnitR => _switchUnitR;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapInputs(BattleInputs battleInputs)
        {
            _actions = battleInputs.Map;

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
                h => _actions.Skill.performed += h,
                h => _actions.Skill.performed -= h
            )
            .Subscribe(_ => _skill.OnNext(Unit.Default))
            .AddTo(_disposable);
            
            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.SwitchUnitL.performed += h,
                h => _actions.SwitchUnitL.performed -= h
            )
            .Subscribe(_ => _switchUnitL.OnNext(Unit.Default))
            .AddTo(_disposable);
            Observable.FromEvent<InputAction.CallbackContext>(
                h => _actions.SwitchUnitR.performed += h,
                h => _actions.SwitchUnitR.performed -= h
            )
            .Subscribe(_ => _switchUnitR.OnNext(Unit.Default))
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