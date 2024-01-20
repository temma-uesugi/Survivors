using System;
using App.AppCommon.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.TestScenes.TextAnim
{
    public class TextAnimSample : MonoBehaviour
    {
        [SerializeField] private AppCommon.UI.TextAnim textAnim;

        private void Awake()
        {
            TextAsync().Forget();
        }

        private async UniTask TextAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            
            Log.Debug("Show 1");
            await textAnim.ShowAndHideAsync();
            Log.Debug("Hide 1");
            
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            
            Log.Debug("Show 2");
            await textAnim.ShowAndHideAsync();
            Log.Debug("Hide 2");
        }
    }
}