using System.Text;
using App.Battle.Core;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle.UI.BattleLog
{
    /// <summary>
    /// 過去ログView
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(BattleLogPastView2))]
    public class BattleLogPastView2 : MonoBehaviour
    {
        [SerializeField] private Text text;
        // [SerializeField] private ObservableEventTrigger layerEventTrigger;

        private readonly StringBuilder _builder = new();

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            gameObject.SetActive(false);
            // layerEventTrigger.OnPointerClickAsObservable().Subscribe(_ =>
            // {
            //     Hide();
            // }).AddTo(this);
        }

        /// <summary>
        /// AddLog
        /// </summary>
        public void AddLog(string log)
        {
            _builder.AppendLine(log);
        }

        /// <summary>
        /// 表示
        /// </summary>
        public void Show()
        {
            text.text = _builder.ToString();
            gameObject.SetActive(true); 
        }
        
        /// <summary>
        /// 非表示
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false); 
        }
    }
}