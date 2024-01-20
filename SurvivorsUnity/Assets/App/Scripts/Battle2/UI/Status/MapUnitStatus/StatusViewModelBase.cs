using App.Battle2.Units;
using UniRx;
using UnityEngine;

namespace App.Battle2.UI.Status.MapUnitStatus
{
    /// <summary>
    /// ステータスViewModelの基底
    /// </summary>
    [RequireComponent(typeof(StatusViewBase))]
    public abstract class StatusViewModelBase<T1, T2> : MonoBehaviour
        where T1 : IUnitModel
        where T2 : StatusViewBase
    {
        protected T1 Model { get; private set; }
        protected T2 View { get; private set;  }
        private BattleCamera2 battleCamera2;
        
        /// <summary>
        /// Setup
        /// </summary>
        public virtual void Setup(T1 unitModel, BattleCamera2 battleCamera2)
        {
            Model = unitModel;
            View = (T2)GetComponent<StatusViewBase>();
            View.gameObject.SetActive(true);
            this.battleCamera2 = battleCamera2;
            
            unitModel.Cell
                .Where(_ => gameObject.activeSelf)
                .SubscribeWithState(this, (cell, self) =>
                {
                    self.View.SetPosition(self.battleCamera2.CellToScreenPoint(cell));
                })
                .AddTo(this);
           
            //カメラのUpdate
            battleCamera2.Position
                .Where(_ => gameObject.activeSelf)
                .SubscribeWithState(this, (_, self) =>
                {
                    self.View.SetPosition(self.battleCamera2.CellToScreenPoint(self.Model.Cell.Value));
                }).AddTo(this);

            this.battleCamera2.CameraSizeRatio
                .SubscribeWithState(this, (x, self) =>
                {
                    self.View.UpdateScale(x);
                    self.View.SetPosition(self.battleCamera2.CellToScreenPoint(self.Model.Cell.Value));
                }).AddTo(this);
        }
    }
}