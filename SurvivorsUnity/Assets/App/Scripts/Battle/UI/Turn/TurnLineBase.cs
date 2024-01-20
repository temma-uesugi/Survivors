using System.Collections.Generic;
using App.AppCommon;
using App.AppCommon.Core;
using App.Battle.Common;
using App.Battle.Libs;
using App.Battle.Turn;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace App.Battle.UI.Turn
{
    /// <summary>
    /// ターンラインの基本
    /// </summary>
    public abstract class TurnLineBase<T, T2> : MonoBehaviour where T : ITurnValue where T2 : IconTurn<T>
    {
        [SerializeField] private GridLayoutAnchor layoutAnchor;
        [SerializeField] private Transform iconLayout;
       
        protected abstract T2 IconPrefab { get; }
        
        private readonly List<T2> _iconList = new();
        public IEnumerable<T2> AllIcon => _iconList;

        /// <summary>
        /// 画像取得
        /// </summary>
        protected abstract Sprite GetSprite(T data);

        /// <summary>
        /// ラベル取得
        /// </summary>
        protected abstract string GetLabel(T data);

        private GameObjectPool<T2> _objectPool;

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(int lineItemAmount, ITurnManager<T> turnManager)
        {
            layoutAnchor.Setup(lineItemAmount);
            _objectPool = new GameObjectPool<T2>(IconPrefab, iconLayout);

            turnManager.OnAddTurnLine.Subscribe(x =>
            {
                if (x.data is IEmptyTurnValue)
                {
                    AddEmptyTurnIcon(x.turnLineIndex);
                }
                else
                {
                    AddTurnIcon(x.turnLineIndex, x.data);
                }
            }).AddTo(this);
            turnManager.OnTurnProceed
                .Skip(1) //初回無視
                .Subscribe(TurnProceed).AddTo(this);
        }

        /// <summary>
        /// ターンアイコンの追加
        /// </summary>
        private void AddTurnIcon(int turnLineIndex, T data)
        {
            var pos = layoutAnchor.GetPosition(turnLineIndex);
            var iconObj = _objectPool.Rent();
            var sprite = GetSprite(data);
            var label = GetLabel(data);
            iconObj.Setup(pos, layoutAnchor.CellSize, sprite, data, label);
            _iconList.Add(iconObj);
        }

        /// <summary>
        /// 空のターンアイコンの追加
        /// </summary>
        private void AddEmptyTurnIcon(int turnLineIndex)
        {
            var pos = layoutAnchor.GetPosition(turnLineIndex);
            var iconObj = _objectPool.Rent();
            iconObj.SetupEmpty(pos, layoutAnchor.CellSize);
            _iconList.Add(iconObj);
        }

        /// <summary>
        /// ターン更新
        /// </summary>
        private void TurnProceed(T newData)
        {
            // Log.Debug("TurnProceed");
            //新アイコン追加
            if (newData is not IEmptyTurnValue)
            {
                AddTurnIcon(GameConst.PredictedTurnAmount, newData);
            }
            for (int i = _iconList.Count - 1; i >= 0; i--)
            {
                var icon = _iconList[i];
                if (i == 0)
                {
                    UniTask.Void(async () =>
                    {
                        await icon.Disappear();
                        _iconList.Remove(icon);
                        _objectPool.Return(icon);
                    });
                }
                else
                {
                    var pos = layoutAnchor.GetPosition(i - 1);
                    icon.MoveTo(pos);
                }
            }
        }
       
        /// <summary>
        /// ターンの変更
        /// </summary>
        public void ChangeTurnValue(T2 icon, T data)
        {
            var sprite = GetSprite(data);
            var label = GetLabel(data);
            icon.Setup(sprite, label);
        }

        /// <summary>
        /// ターンをEmptyに
        /// </summary>
        public void ChangeTurnEmpty(T2 icon)
        {
            icon.ChangeEmpty();
        }
        
    }
}