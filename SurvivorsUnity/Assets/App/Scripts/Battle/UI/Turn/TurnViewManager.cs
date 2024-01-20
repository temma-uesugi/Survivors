using App.AppCommon;
using App.Battle.Core;
using App.Battle.Turn;
using App.Battle.Units;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace App.Battle.UI.Turn
{
    /// <summary>
    /// ターン表示
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(TurnViewManager))]
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
            UnitManger unitManger
        )
        {
            lineWeather.Setup(GameConst.PredictedTurnAmount, weatherManager);
            lineWind.Setup(GameConst.PredictedTurnAmount, windManager);
            shipTurnManagerView.SetupAsync(unitManger).Forget();
        }
    }
}