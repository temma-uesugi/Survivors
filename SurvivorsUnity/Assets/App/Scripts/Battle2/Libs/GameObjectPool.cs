using System;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;

namespace App.Battle2.Libs
{
    /// <summary>
    /// GameObject用オブジェクトプール
    /// </summary>
    public class GameObjectPool<T> : ObjectPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;

        /// <summary> インスタンス生成時 </summary>
        public IObservable<T> OnCreateInstance => _onCreateInstance;
        private readonly Subject<T> _onCreateInstance = new();

        /// <summary> 返却時 </summary>
        public IObservable<T> OnReturn => _onReturn;
        private readonly Subject<T> _onReturn = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameObjectPool(T prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        /// <summary>
        /// 生成が必要な際に呼ばれる
        /// </summary>
        protected override T CreateInstance()
        {
            var obj = GameObject.Instantiate(_prefab, _parent);
            _onCreateInstance.OnNext(obj);
            return obj;
        }

        /// <summary>
        /// 返却時によばれる
        /// </summary>
        protected override void OnBeforeReturn(T instance)
        {
            base.OnBeforeReturn(instance);
            _onReturn.OnNext(instance);
        }
    }
}