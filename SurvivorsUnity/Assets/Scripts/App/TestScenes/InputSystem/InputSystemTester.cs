#if UNITY_EDITOR
using App.AppCommon.Core;
using UniRx;
using UnityEngine;

namespace App.Test.Scenes
{
    /// <summary>
    /// InputSystemTester
    /// </summary>
    public class InputSystemTester : MonoBehaviour
    {

        private void Awake()
        {
            var a = InputSystemManager.Instance;
            InputSystemManager.OnMove.Subscribe(x =>
            {
                Log.Debug("hoge", x);
            }).AddTo(this);
        }
    }
}
#endif