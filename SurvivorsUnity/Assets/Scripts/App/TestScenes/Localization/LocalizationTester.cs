using System;
using System.Linq;
using App.AppCommon.Core;
using App.Localization;
using Cysharp.Threading.Tasks;
using Master;
using UnityEngine;

namespace App.TestScenes.Localization
{
    /// <summary>
    /// ローカライズテストシーン
    /// </summary>
    public class LocalizationTester : MonoBehaviour
    {
        [SerializeField] private L10NText text;
        
        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            var a = MasterData.Facade.EnemySkillTable.All;
            Log.Debug("hoge", a.First.Effect1);
          
            UniTask.Void(async () =>
            {
                text.UpdateText("test");
                await UniTask.Delay(TimeSpan.FromSeconds(5));
                text.UpdateText((int)99999);
                await UniTask.Delay(TimeSpan.FromSeconds(5));
                text.UpdateText(78890.02f);
            });
        }
    }
}