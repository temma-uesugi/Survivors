using System;
using App.ActionInputs;
using App.Inputs;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Battle.Inputs
{
    /// <summary>
    /// メニュー操作
    /// </summary>
    public class MenuInputs : IActionInputs, IMenuActionInputs
    {
        private readonly CompositeDisposable _disposable = new();
        private BattleInputs.MenuActions _actions;
       
        private readonly Subject<Vector2> _move = new();
        private readonly Subject<Unit> _stopMove = new();
        public IObservable<Vector2> Move => _move;
        public IObservable<Unit> StopMove => _stopMove;
        
        private readonly Subject<Unit> _decide = new();
        public IObservable<Unit> Decide => _decide;
        
        private readonly Subject<Unit> _cancel = new();
        public IObservable<Unit> Cancel => _cancel;

        public IObservable<int> MoveCursor => _move.Select(val =>
        {
            if (val.y == 0)
            {
                return 0;
            }
            return val.y > 0 ? -1 : 1;
        }).AsObservable();
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MenuInputs(BattleInputs battleInputs)
        {
            _actions = battleInputs.Menu;

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