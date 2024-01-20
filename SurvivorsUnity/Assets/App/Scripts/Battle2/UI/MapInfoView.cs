using System;
using App.Battle2.Core;
using UnityEngine;
using VContainer;

namespace App.Battle2.UI
{
    [ContainerRegisterMonoBehaviourAttribute2(typeof(MapInfoView))]
    public class MapInfoView : MonoBehaviour
    {
        private IDisposable _disposable;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
        )
        {
        }
       
        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            _disposable.Dispose(); 
        }
    }
}