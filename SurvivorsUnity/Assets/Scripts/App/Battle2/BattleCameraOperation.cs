using System;
using App.Battle2.Core;
using App.Battle2.Facades;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle2
{
    /// <summary>
    /// バトルカメラのOperation
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(BattleCameraOperation))]
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
            BattleCamera2.Instance.CameraZoomUp();
            _updateDisposable?.Dispose();
            _updateDisposable = Observable.Interval(TimeSpan.FromSeconds(0.2f))
                .TakeUntil(BattleOperation.Facade.Common.ZoomUpCameraStop)
                .Subscribe(_ =>
                {
                    BattleCamera2.Instance.CameraZoomUp();
                })
                .AddTo(this);
        }
        
        /// <summary>
        /// OnZoomDown
        /// </summary>
        private void OnZoomDown()
        {
            BattleCamera2.Instance.CameraZoomDown();
            _updateDisposable?.Dispose();
            _updateDisposable = Observable.Interval(TimeSpan.FromSeconds(0.2f))
                .TakeUntil(BattleOperation.Facade.Common.ZoomDownCameraStop)
                .Subscribe(_  =>
                {
                    BattleCamera2.Instance.CameraZoomDown();
                })
                .AddTo(this);
        }
    }
}