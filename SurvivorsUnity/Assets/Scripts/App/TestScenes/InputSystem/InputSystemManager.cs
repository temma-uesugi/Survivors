using System;
using App.ActionInputs;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.TestScenes.InputSystem
{
    /// <summary>
    /// InputSystemManager
    /// </summary>
    public class InputSystemManager
    {
        private static InputSystemManager _instance;
        public static InputSystemManager Instance => _instance ??= new InputSystemManager();

        private static readonly CompositeDisposable _disposable = new();

        public static IObservable<Unit> OnEast => _instance._onEast;
        public static IObservable<Unit> OnSouth => _instance._onSouth;
        public static IObservable<Unit> OnWest => _instance._onWest;
        public static IObservable<Unit> OnNorth => _instance._onNorth;
        public static IObservable<Unit> OnL => _instance._onL;
        public static IObservable<Unit> OnR => _instance._onR;
        public static IObservable<Vector2> OnMove => _instance._onMove;
        public static IObservable<Unit> OnMoveStop => _instance._onMoveStop;
        public static IObservable<Unit> OnTriggerL => _instance._onTriggerL;
        public static IObservable<Unit> OnTriggerR => _instance._onTriggerR;
        public static IObservable<Unit> OnRightStickUp => _instance._onRightStickUp;
        public static IObservable<Unit> OnRightStickDown => _instance._onRightStickDown;
        public static IObservable<Unit> OnRightStickUpStop => _instance._onRightStickUpStop;
        public static IObservable<Unit> OnRightStickDownStop => _instance._onRightStickDownStop;
        
        private IObservable<Unit> _onEast;
        private IObservable<Unit> _onSouth;
        private IObservable<Unit> _onWest;
        private IObservable<Unit> _onNorth;
        private IObservable<Unit> _onL;
        private IObservable<Unit> _onR;
        private IObservable<Vector2> _onMove;
        private IObservable<Unit> _onMoveStop;
        private IObservable<Unit> _onTriggerL;
        private IObservable<Unit> _onTriggerR;
        private IObservable<Unit> _onRightStickUp;
        private IObservable<Unit> _onRightStickDown;
        private IObservable<Unit> _onRightStickUpStop;
        private IObservable<Unit> _onRightStickDownStop;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private InputSystemManager()
        {
            var battleInput = new BattleInputs();
            battleInput.Enable();
        
            _onEast = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.East.performed += h,
                h => battleInput.Battle.East.performed -= h
            )
            .AsUnitObservable();
            _onSouth = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.South.performed += h,
                h => battleInput.Battle.South.performed -= h
            )
            .AsUnitObservable();
            _onWest = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.West.performed += h,
                h => battleInput.Battle.West.performed -= h
            )
            .AsUnitObservable();
            _onNorth = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.North.performed += h,
                h => battleInput.Battle.North.performed -= h
            )
            .AsUnitObservable();
            
            _onL = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.L.performed += h,
                h => battleInput.Battle.L.performed -= h
            )
            .AsUnitObservable();
            _onR = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.R.performed += h,
                h => battleInput.Battle.R.performed -= h
            )
            .AsUnitObservable();
            
            _onMove = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.Move.performed += h,
                h => battleInput.Battle.Move.performed -= h
            )
            .Select(x => x.ReadValue<Vector2>())
            .AsObservable();
            _onMoveStop = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.Move.canceled += h,
                h => battleInput.Battle.Move.canceled -= h
            )
            .AsUnitObservable();
            
            _onTriggerL = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.TriggerL.performed += h,
                h => battleInput.Battle.TriggerL.performed -= h
            )
            .AsUnitObservable();
            _onTriggerR = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.TriggerR.performed += h,
                h => battleInput.Battle.TriggerR.performed -= h
            )
            .AsUnitObservable();
            
            _onRightStickUp = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.RightStickUp.performed += h,
                h => battleInput.Battle.RightStickUp.performed -= h
            )
            .AsUnitObservable();
            _onRightStickUpStop = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.RightStickUp.canceled += h,
                h => battleInput.Battle.RightStickUp.canceled -= h
            )
            .AsUnitObservable();
            _onRightStickDown = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.RightStickDown.performed += h,
                h => battleInput.Battle.RightStickDown.performed -= h
            )
            .AsUnitObservable();
            _onRightStickDownStop = Observable.FromEvent<InputAction.CallbackContext>(
                h => battleInput.Battle.RightStickDown.canceled += h,
                h => battleInput.Battle.RightStickDown.canceled -= h
            )
            .AsUnitObservable();
        }
    }
}