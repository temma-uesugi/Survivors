using App.Battle.Units;
using UniRx;
using UnityEngine;

namespace App.Battle.UI.Status
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
        private BattleCamera _battleCamera;
        
        /// <summary>
        /// Setup
        /// </summary>
        public virtual void Setup(T1 unitModel, BattleCamera battleCamera)
        {
            Model = unitModel;
            View = (T2)GetComponent<StatusViewBase>();
            View.gameObject.SetActive(true);
            _battleCamera = battleCamera;
            
            unitModel.Cell
                .Where(_ => gameObject.activeSelf)
                .SubscribeWithState(this, (cell, self) =>
                {
                    self.View.SetPosition(self._battleCamera.CellToScreenPoint(cell));
                })
                .AddTo(this);
           
            //カメラのUpdate
            battleCamera.Position
                .Where(_ => gameObject.activeSelf)
                .SubscribeWithState(this, (_, self) =>
                {
                    self.View.SetPosition(self._battleCamera.CellToScreenPoint(self.Model.Cell.Value));
                }).AddTo(this);

            _battleCamera.CameraSizeRatio
                .SubscribeWithState(this, (x, self) =>
                {
                    self.View.UpdateScale(x);
                    self.View.SetPosition(self._battleCamera.CellToScreenPoint(self.Model.Cell.Value));
                }).AddTo(this);
        }
    }
}