using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace App.AppCommon.Libs
{
    /// <summary>
    /// UGUI用のTweenのパターン
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(DOTween))]
    public class UguiTween : MonoBehaviour
    {
        [SerializeField] private bool initHide;

        private bool _initialized = false;
        private RectTransform _rectTrans;

        private SpriteRenderer _spriteRenderer = null;
        private Renderer _renderer = null;
        private CanvasGroup _canvasGroup = null;
        public bool IsPlaying { get; private set; } = false;

        private string _curSettingKey;
        //SequenceのDictionary
        private readonly Dictionary<string, Sequence> _sequenceDic = new Dictionary<string, Sequence>();
        private readonly Dictionary<string, Vector2> _initPosition = new Dictionary<string, Vector2>();
        private readonly Dictionary<string, Vector2> _endPosition = new Dictionary<string, Vector2>();

        private readonly Dictionary<string, Action<GameObject, RectTransform>> _initTransActionDic = new Dictionary<string, Action<GameObject, RectTransform>>();
        private readonly Dictionary<string, Action<GameObject, RectTransform>> _endTransActionDic = new Dictionary<string, Action<GameObject, RectTransform>>();
        private readonly Dictionary<string, Action> _completeActionDic = new Dictionary<string, Action>();
        private readonly Dictionary<string, Action<GameObject, RectTransform>> _updateActionDic = new Dictionary<string, Action<GameObject, RectTransform>>();
        private readonly Dictionary<string, float> _initAlphaDic= new Dictionary<string, float>();
        private readonly Dictionary<string, float> _endAlphaDic= new Dictionary<string, float>();

        public enum SetType
        {
            Append = 1,
            Join = 2,
        }

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            Init();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Init()
        {
            if (_initialized)
            {
                return;
            }
            _initialized = true;
            if (initHide)
            {
                gameObject.SetActive(false);
            }

            if (TryGetComponent(out CanvasGroup canvasGroup))
            {
                _canvasGroup = canvasGroup;
            }
            else if (TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                _spriteRenderer = spriteRenderer;
            }
            else if (TryGetComponent(out Renderer ren))
            {
                _renderer = ren;
            }
        }

        /// <summary>
        /// 作成
        /// </summary>
        public UguiTween CreateSequence(string key)
        {
            Init();
            _rectTrans = GetComponent<RectTransform>();
            _curSettingKey = key;
            var sequence = DOTween.Sequence();
            sequence.id = key;
            _sequenceDic.Add(key, sequence);
            _initAlphaDic.Add(key, -1);
            _endAlphaDic.Add(key, -1);
            return this;
        }

        /// <summary>
        /// keyを指定してシークエンスのセット
        /// </summary>
        public UguiTween SetSequenceByKey(string key)
        {
            if (_sequenceDic.ContainsKey(key))
            {
                _curSettingKey = key;
                return this;
            }
            return CreateSequence(key);
        }

        /// <summary>
        /// 初期位置をセット
        /// </summary>
        public UguiTween SetInitPosition(Vector2 pos)
        {
            _initPosition[_curSettingKey] = pos;
            return this;
        }

        /// <summary>
        /// 終了位置をセット
        /// </summary>
        public UguiTween SetEndPosition(Vector2 pos)
        {
            _endPosition[_curSettingKey] = pos;
            return this;
        }

        /// <summary>
        /// 初期の状態をセット
        /// </summary>
        public UguiTween SetInitTrans(Action<GameObject, RectTransform> callback)
        {
            _initTransActionDic[_curSettingKey] = callback;
            return this;
        }

        /// <summary>
        /// 初期のアルファ値をセット
        /// </summary>
        public UguiTween SetInitAlpha(float alpha)
        {
            _initAlphaDic[_curSettingKey] = alpha;
            return this;
        }

        /// <summary>
        /// 終了時の状態をセット
        /// </summary>
        public UguiTween SetEndTrans(Action<GameObject, RectTransform> callback)
        {
            _endTransActionDic[_curSettingKey] = callback;
            return this;
        }

        /// <summary>
        /// 終了時のアルファ値をセット
        /// </summary>
        public UguiTween SetEndAlpha(float alpha)
        {
            _endAlphaDic[_curSettingKey] = alpha;
            return this;
        }

        /// <summary>
        /// 移動をセット
        /// </summary>
        public UguiTween SetMove(Vector2 pos, float duration, Ease ease = Ease.Linear, SetType type = SetType.Append)
        {
            var tween = _rectTrans.DOAnchorPos(pos, duration)
                .SetEase(ease);

            var sequence = _sequenceDic[_curSettingKey];
            if (type == SetType.Append)
            {
                sequence.Append(tween);
            }
            else if (type == SetType.Join)
            {
                sequence.Join(tween);
            }
            return this;
        }

        /// <summary>
        /// 回転をセット
        /// </summary>
        public UguiTween SetRotate(Vector3 rotate, float duration, Ease ease = Ease.Linear, SetType type = SetType.Append)
        {
            var tween = _rectTrans.DOLocalRotate(rotate, duration)
                .SetEase(ease);

            var sequence = _sequenceDic[_curSettingKey];
            if (type == SetType.Append)
            {
                sequence.Append(tween);
            }
            else if (type == SetType.Join)
            {
                sequence.Join(tween);
            }
            return this;
        }

        /// <summary>
        /// Scaleをセット
        /// </summary>
        public UguiTween SetScale (Vector3 scale, float duration, Ease ease = Ease.Linear, SetType type = SetType.Append)
        {
            var tween = _rectTrans.DOScale(scale, duration)
                .SetEase(ease);

            var sequence = _sequenceDic[_curSettingKey];
            if (type == SetType.Append)
            {
                sequence.Append(tween);
            }
            else if (type == SetType.Join)
            {
                sequence.Join(tween);
            }
            return this;
        }

        /// <summary>
        /// 透過を設定
        /// </summary>
        public UguiTween SetAlpha(float alpha, float duration, Ease ease = Ease.Linear, SetType type = SetType.Append)
        {
            var sequence = _sequenceDic[_curSettingKey];
            if (_canvasGroup != null)
            {
                var alphaTween = _canvasGroup.DOFade(alpha, duration)
                    .SetEase(ease);
                if (type == SetType.Append)
                {
                    sequence.Append(alphaTween);
                }
                else if (type == SetType.Join)
                {
                    sequence.Join(alphaTween);
                }
                return this;
            }

            TweenerCore<Color, Color, ColorOptions> colorTween;
            if (_spriteRenderer != null)
            {
                colorTween = DOTween.ToAlpha(
                        () => _spriteRenderer.color,
                        color => _spriteRenderer.color = color,
                        alpha,
                        duration
                    )
                    .SetEase(ease);
            }
            else if (_renderer != null)
            {
                colorTween = DOTween.ToAlpha(
                        () => _renderer.material.color,
                        color => _renderer.material.color = color,
                        alpha,
                        duration
                    )
                    .SetEase(ease);
            }
            else
            {
                return this;
            }

            if (type == SetType.Append)
            {
                sequence.Append(colorTween);
            }
            else if (type == SetType.Join)
            {
                sequence.Join(colorTween);
            }
            return this;
        }

        /// <summary>
        /// Intervalをセット
        /// </summary>
        public UguiTween SetInterval(float interval)
        {
            _sequenceDic[_curSettingKey].AppendInterval(interval);
            return this;
        }

        /// <summary>
        /// Delayをセット
        /// </summary>
        public UguiTween SetDelay(float delay)
        {
            _sequenceDic[_curSettingKey].PrependInterval(delay);
            return this;
        }

        /// <summary>
        /// 完了時アクションの追加
        /// </summary>
        public UguiTween SetCompleteActon(Action callback)
        {
            _completeActionDic[_curSettingKey] = callback;
            return this;
        }

        /// <summary>
        /// 実行中アクションの追加
        /// </summary>
        public UguiTween SetUpdateAction(Action<GameObject, RectTransform> callback)
        {
            _updateActionDic[_curSettingKey] = callback;
            return this;
        }

        /// <summary>
        /// Sequenceを返す
        /// </summary>
        public Sequence ToSequence()
        {
            return ToSequence(_curSettingKey);
        }

        /// <summary>
        /// Sequenceを返す
        /// </summary>
        public Sequence ToSequence(string key)
        {
            if (!_sequenceDic.TryGetValue(key, out var sequence))
            {
                return DOTween.Sequence();
            }

            var updateAction = _updateActionDic.ContainsKey(key)
                ? _updateActionDic[key]
                : null;

            sequence
                .SetAutoKill(false)
                .SetLink(gameObject)
                .OnStart(() =>
                {
                    IsPlaying = true;
                    if (_initTransActionDic.TryGetValue(key, out var initTransAction))
                    {
                        initTransAction(gameObject, _rectTrans);
                    }
                    if (_initPosition.TryGetValue(key, out var initPos))
                    {
                        _rectTrans.anchoredPosition = initPos;
                    }
                    if (_initAlphaDic.TryGetValue(key, out var initAlpha))
                    {
                        if (_canvasGroup != null)
                        {
                            _canvasGroup.alpha = initAlpha;
                        }
                        else if (_spriteRenderer != null)
                        {
                            var c = _spriteRenderer.color;
                            _spriteRenderer.color = new Color(c.r, c.g, c.b, initAlpha);
                        }
                        else if (_renderer != null)
                        {
                            var c = _renderer.material.color;
                            _renderer.material.color = new Color(c.r, c.g, c.b, initAlpha);
                        }
                    }
                })

                .OnComplete(() =>
                {
                    sequence.Rewind();
                    if (_endTransActionDic.TryGetValue(key, out var endTransAction))
                    {
                        endTransAction(gameObject, _rectTrans);
                    }
                    if (_endPosition.TryGetValue(key, out var endPos))
                    {
                        _rectTrans.anchoredPosition = endPos;
                    }

                    if (_endAlphaDic.TryGetValue(key, out var endAlpha))
                    {
                        if (_canvasGroup != null)
                        {
                            _canvasGroup.alpha = endAlpha;
                        }
                        else if (_spriteRenderer != null)
                        {
                            var c = _spriteRenderer.color;
                            _spriteRenderer.color = new Color(c.r, c.g, c.b, endAlpha);
                        }
                        else if (_renderer != null)
                        {
                            var c = _renderer.material.color;
                            _renderer.material.color = new Color(c.r, c.g, c.b, endAlpha);
                        }
                    }

                    if (_completeActionDic.TryGetValue(key, out var compAction))
                    {
                        compAction();
                    }
                    IsPlaying = false;
                })
                .OnUpdate(() =>
                {
                    if (updateAction != null)
                    {
                        updateAction(gameObject, _rectTrans);
                    }
                })
                .Pause();
            return sequence;
        }
    }
}