using System;
using App.Battle.Core;
using App.Battle.Facades;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle
{
    /// <summary>
    /// バトルカメラのOperation
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(BattleCameraOperation))]
    public class BattleCameraOperation : MonoBehaviour
    {
        private IDisposable _updateDisposable;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct()
        {
            BattleOperation.Facade.Common.ZoomUpCamera.Subscribe(_ => OnZoomUp()).AddTo(this);
            BattleOperation.Facade.Common.ZoomDownCamera.Subscribe(_ => OnZoomDown()).AddTo(this);
        }
        
        /// <summary>
        /// OnZoomUp
        /// </summary>
        private void OnZoomUp()
        {
            BattleCamera.Instance.CameraZoomUp();
            _updateDisposable?.Dispose();
            _updateDisposable = Observable.Interval(TimeSpan.FromSeconds(0.2f))
                .TakeUntil(BattleOperation.Facade.Common.ZoomUpCameraStop)
                .Subscribe(_ =>
                {
                    BattleCamera.Instance.CameraZoomUp();
                })
                .AddTo(this);
        }
        
        /// <summary>
        /// OnZoomDown
        /// </summary>
        private void OnZoomDown()
        {
            BattleCamera.Instance.CameraZoomDown();
            _updateDisposable?.Dispose();
            _updateDisposable = Observable.Interval(TimeSpan.FromSeconds(0.2f))
                .TakeUntil(BattleOperation.Facade.Common.ZoomDownCameraStop)
                .Subscribe(_  =>
                {
                    BattleCamera.Instance.CameraZoomDown();
                })
                .AddTo(this);
        }
    }
}