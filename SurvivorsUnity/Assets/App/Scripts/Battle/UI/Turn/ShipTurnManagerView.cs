using System.Collections.Generic;
using App.AppCommon.Core;
using App.Battle.Units;
using App.Battle.Units.Ship;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace App.Battle.UI.Turn
{
    /// <summary>
    /// 船ターンView
    /// </summary>
    public class ShipTurnManagerView : MonoBehaviour
    {
        [SerializeField] private ShipTurnIconAnchor anchor;
        [SerializeField] private ShipTurnIcon iconPrefab;
        [SerializeField] private Transform lineLayer;
        [SerializeField] private CanvasGroup anchorCanvasGroup;

        private readonly Dictionary<uint, ShipTurnIcon> _iconDic = new();
        private readonly Dictionary<int, List<ShipTurnIcon>> _shipTurnDic = new();

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            anchorCanvasGroup.alpha = 0;
        }
        
        /// <summary>
        /// Setup
        /// </summary>
        public async UniTask SetupAsync(UnitManger unitManger)
        {
            await anchor.SetupAsync();
            anchor.gameObject.SetActive(false);
            foreach (var ship in unitManger.AllAliveShips)
            {
                AddShip(ship);
            }
            unitManger.ShipModelMap
                .ObserveAdd()
                .Subscribe(x => AddShip(x.Value)).AddTo(this);
        }

        /// <summary>
        /// 船追加
        /// </summary>
        private void AddShip(ShipUnitModel shipUnitModel)
        {
            var icon = Instantiate(iconPrefab, lineLayer);
            icon.Setup(shipUnitModel.UnitId, shipUnitModel.Label, shipUnitModel.NextActionTurns.Value);
            SetPosition(icon);
            
            //購読処理
            shipUnitModel.NextActionTurns.SubscribeWithState(icon, (t, i) =>
            {
                i.SetTurn(t);
                RemovePervPosition(i);
                SetPosition(i);
            }).AddTo(shipUnitModel.ModelDisposable);
            shipUnitModel.NextTurnSchedule.SubscribeWithState(icon, (x, i) =>
            {
                if (x.isOn)
                {
                    SetSchedule(i, x.trun); 
                }
                else
                {
                    i.ClearSchedule(); 
                }
            }).AddTo(shipUnitModel.ModelDisposable);
        }

        /// <summary>
        /// 位置取得
        /// </summary>
        private Vector3 GetPosition(int turn, out List<ShipTurnIcon> list)
        {
            if (!_shipTurnDic.TryGetValue(turn, out list))
            {
                list = new List<ShipTurnIcon>();
            }
            var index = list.Count;
            var pos = anchor[turn, index];
            return pos;
        }
        
        /// <summary>
        /// Turnのセット
        /// </summary>
        private void SetPosition(ShipTurnIcon icon)
        {
            var turn = icon.Turn;
            var pos = GetPosition(turn, out var list);
            icon.SetPosition(pos);
            
            list.Add(icon);
            _shipTurnDic[turn] = list;
        }

        /// <summary>
        /// 前回の位置から抜く
        /// </summary>
        private void RemovePervPosition(ShipTurnIcon icon)
        {
            var prevTurn = icon.PrevTurn;
            if (!_shipTurnDic.TryGetValue(prevTurn, out var prevList))
            {
                return;
            }

            if (!prevList.Remove(icon))
            {
                return; 
            }
            for (int i = 0; i < prevList.Count; i++)
            {
                var pos = anchor[prevTurn, i];
                prevList[i].SetPosition(pos);
            }
        }

        /// <summary>
        /// スケジュール設定
        /// </summary>
        private void SetSchedule(ShipTurnIcon icon, int turn)
        {
            var pos = GetPosition(turn, out _);
            icon.SetSchedule(pos);
        }
        
        /// <summary>
        /// 船削除
        /// </summary>
        private void RemoveShip()
        {
            
        }
    }
}