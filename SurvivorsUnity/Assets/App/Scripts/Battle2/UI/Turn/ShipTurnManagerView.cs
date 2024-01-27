using System.Collections.Generic;
using App.Battle2.Units;
using App.Battle2.Units.Ship;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace App.Battle2.UI.Turn
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
        public async UniTask SetupAsync(UnitManger2 unitManger2)
        {
            await anchor.SetupAsync();
            anchor.gameObject.SetActive(false);
            foreach (var ship in unitManger2.AllAliveShips)
            {
                AddShip(ship);
            }
            unitManger2.ShipModelMap
                .ObserveAdd()
                .Subscribe(x => AddShip(x.Value)).AddTo(this);
        }

        /// <summary>
        /// 船追加
        /// </summary>
        private void AddShip(ShipUnitModel2 shipUnitModel2)
        {
            var icon = Instantiate(iconPrefab, lineLayer);
            icon.Setup(shipUnitModel2.UnitId, shipUnitModel2.Label, shipUnitModel2.NextActionTurns.Value);
            SetPosition(icon);
            
            //購読処理
            shipUnitModel2.NextActionTurns.SubscribeWithState(icon, (t, i) =>
            {
                i.SetTurn(t);
                RemovePervPosition(i);
                SetPosition(i);
            }).AddTo(shipUnitModel2.ModelDisposable);
            shipUnitModel2.NextTurnSchedule.SubscribeWithState(icon, (x, i) =>
            {
                if (x.isOn)
                {
                    SetSchedule(i, x.trun); 
                }
                else
                {
                    i.ClearSchedule(); 
                }
            }).AddTo(shipUnitModel2.ModelDisposable);
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