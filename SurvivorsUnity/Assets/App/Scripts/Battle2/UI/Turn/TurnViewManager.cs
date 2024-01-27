using App.AppCommon;
using App.Battle2.Core;
using App.Battle2.Turn;
using App.Battle2.Units;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace App.Battle2.UI.Turn
{
    /// <summary>
    /// ターン表示
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(TurnViewManager))]
    public class TurnViewManager : MonoBehaviour
    {
        [SerializeField] private TurnLineWeather lineWeather;
        [SerializeField] private TurnLineWind lineWind;
        [FormerlySerializedAs("shipTurnView")] [SerializeField] private ShipTurnManagerView shipTurnManagerView;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            WeatherManager weatherManager,
            WindManager windManager,
            UnitManger2 unitManger2
        )
        {
            lineWeather.Setup(GameConst.PredictedTurnAmount, weatherManager);
            lineWind.Setup(GameConst.PredictedTurnAmount, windManager);
            shipTurnManagerView.SetupAsync(unitManger2).Forget();
        }
    }
}