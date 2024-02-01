using App.AppCommon;
using App.Battle2.Core;
using App.Battle2.Facades;
using App.Battle2.Map.Cells;
using Master.Constants;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle2.UI
{
    /// <summary>
    /// マップアイコン管理
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(MapIconManager))]
    public class MapIconManager : MonoBehaviour
    {
        [SerializeField] private GameObject selectIconButton;
        [SerializeField] private GameObject attackIconButton;

        private BattleCamera2 battleCamera2;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            BattleCamera2 battleCamera2,
            BattleEventHub2 eventHub2
        )
        {
            this.battleCamera2 = battleCamera2;
            selectIconButton.SetActive(true);
            
            BattleState.Facade.FocusedHexCell
                .Where(x => x != null)
                .Subscribe(UpdateHexCell).AddTo(this);
            
            eventHub2.Subscribe<BattleEvents2.OnPhaseStartAsync>(async x =>
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
            battleCamera2.SetPosition(cell.Position);
        }
    }
}