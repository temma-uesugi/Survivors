#if UNITY_EDITOR
using App.AppCommon.Core;
using UniRx;
using UnityEngine;

namespace App.TestScenes.InputSystem
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