using App.AppCommon;
using App.Battle.Core;
using App.Battle.Facades;
using App.Battle.Map.Cells;
using App.Battle.UI.HexButtons;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle.UI
{
    /// <summary>
    /// マップアイコン管理
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(MapIconManager))]
    public class MapIconManager : MonoBehaviour
    {
        [SerializeField] private GameObject selectIconButton;
        [SerializeField] private GameObject attackIconButton;

        private BattleCamera _battleCamera;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            BattleCamera battleCamera,
            BattleEventHub eventHub
        )
        {
            _battleCamera = battleCamera;
            selectIconButton.SetActive(true);
            
            BattleState.Facade.FocusedHexCell
                .Where(x => x != null)
                .Subscribe(UpdateHexCell).AddTo(this);
            
            eventHub.Subscribe<BattleEvents.OnPhaseStartAsync>(async x =>
            {
                selectIconButton.SetActive(x.Phase == PhaseType.PlayerPhase); 
            }).AddTo(this);
        }

        /// <summary>
        /// Cellの選択
        /// </summary>
        private void UpdateHexCell(HexCell cell)
        {
            // zは変更しない
            Vector3 p = selectIconButton.transform.position;
            p.x = cell.Position.x;
            p.y = cell.Position.y;
            // プレイヤ選択カーソル
            selectIconButton.transform.position = p;

            // カメラ
            _battleCamera.SetPosition(cell.Position);
        }
    }
}