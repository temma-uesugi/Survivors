using System.Collections.Generic;
using System.Threading;
using App.Battle.Core;
using App.Battle.Libs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle.UI.BattleLog
{
    /// <summary>
    /// BattleLogView
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(BattleLogView))]
    public class BattleLogView : MonoBehaviour
    {
        [SerializeField] private int lineAmount = 15;
        [SerializeField] private RectTransform rectTrans;
        [SerializeField] private VerticalLayoutGroup layoutGroup;
        [SerializeField] private BattleLogLine logLinePrefab;
        [SerializeField] private VerticalLayoutGroup anchorLayoutGroup;
        [SerializeField] private ContentSizeFitter anchorContentSizeFitter;

        private GameObjectPool<BattleLogLine> _logLineObjectPool;
        private readonly Queue<BattleLogLine> _logLines = new();

        private CancellationTokenSource _cts;

        /// <summary>
        /// Setup
        /// </summary>
        public async UniTask SetupAsync()
        {
            _logLineObjectPool = new GameObjectPool<BattleLogLine>(logLinePrefab, layoutGroup.transform);
            var anchorRect = anchorLayoutGroup.GetComponent<RectTransform>();
            
            for (int i = 0; i < lineAmount; i++)
            {
                Instantiate(logLinePrefab, anchorLayoutGroup.transform);
            }
            layoutGroup.CalculateLayoutInputVertical();
            layoutGroup.SetLayoutVertical();
            anchorContentSizeFitter.SetLayoutVertical();
            await UniTask.WaitWhile(() => anchorRect.rect.size.y == 0, cancellationToken: this.GetCancellationTokenOnDestroy());
            var anchorSize = anchorRect.rect.size;
            rectTrans.sizeDelta = anchorSize;
        }
        
        /// <summary>
        /// 追加
        /// </summary>
        public async UniTask AddLine(string log)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, this.GetCancellationTokenOnDestroy()).Token;

            if (_logLines.Count >= lineAmount)
            {
                var removed = _logLines.Dequeue();
                _logLineObjectPool.Return(removed);
            }
            
            var line = _logLineObjectPool.Rent();
            line.Setup(log);
            line.transform.SetAsFirstSibling();
            _logLines.Enqueue(line);
            
            layoutGroup.CalculateLayoutInputVertical();
            layoutGroup.SetLayoutVertical();
            await UniTask.Yield(cancellationToken: token);
            line.ShowAnimation();
        }
    }
}