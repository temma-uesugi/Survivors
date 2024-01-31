using System;
using System.Threading;
using App.AppCommon.Core;
using Cysharp.Threading.Tasks;
using Febucci.UI;
using Febucci.UI.Core;
using TMPro;
using UniRx;
using UnityEngine;

namespace App.AppCommon.UI
{
    /// <summary>
    /// TextAnimType
    /// </summary>
    public enum TextAnimType
    {
        None,
        Rot,
        Diagexp,
        Vertexp,
        Offset,
        Fade,
        Size,
        Rdir
    }

    /// <summary>
    /// 設定
    /// </summary>
    public record TextAnimSetting(TextAnimType Type, float Strength, float Duration, bool IsBottom, int Horizontal);

    /// <summary>
    /// 設定の拡張
    /// </summary>
    public static class TextAnimSettingExtensions
    {
        /// <summary>
        /// Prefixを取得
        /// </summary>
        public static string GetPrefix(this TextAnimSetting self, bool isHide)
        {
            if (self.Type == TextAnimType.None) return string.Empty;

            var tag = self.Type switch
            {
                TextAnimType.Rot => $"rot a={self.Strength} d={self.Duration}",
                TextAnimType.Diagexp => $"diagexp d={self.Duration} bot={(self.IsBottom ? 1 : 0)}",
                TextAnimType.Vertexp => $"vertexp d={self.Duration} bot={(self.IsBottom ? 1 : 0)}",
                TextAnimType.Offset => $"offset a={self.Strength} d={self.Duration}",
                TextAnimType.Fade => $"fade d={self.Duration}",
                TextAnimType.Size => $"size a={self.Strength} d={self.Duration}",
                TextAnimType.Rdir => $"rdir a={self.Strength} d={self.Duration}",
            };
            var prefix = (isHide ? "{#" : "{") + tag + "}";
            return prefix;
        }

        /// <summary>
        /// Suffixを取得
        /// </summary>
        public static string GetSuffix(this TextAnimSetting self, bool isHide)
        {
            if (self.Type == TextAnimType.None) return string.Empty;
            var suffix = "{/" + (isHide ? "#" : "") + self.Type.ToString().ToLower() + "}";
            return suffix;
        }

        /// <summary>
        /// TextAnimTypeから設定を取得
        /// </summary>
        public static TextAnimSetting GetSetting(this TextAnimType type) => type switch
        {
            TextAnimType.Rot => TextAnim.RotSetting(),
            TextAnimType.Diagexp => TextAnim.DiagexpSetting(false),
            TextAnimType.Vertexp => TextAnim.VertexpSetting(false),
            TextAnimType.Offset => TextAnim.OffsetSetting(),
            TextAnimType.Fade => TextAnim.FadeSetting(),
            TextAnimType.Size => TextAnim.SizeSetting(),
            TextAnimType.Rdir => TextAnim.RdirSetting(),
            _ => TextAnim.NoneSetting()
        };
    }

    /// <summary>
    /// TextAnim
    /// </summary>
    [RequireComponent(typeof(TextAnimator_TMP), typeof(TypewriterByCharacter), typeof(TextMeshProUGUI))]
    public class TextAnim : MonoBehaviour
    {
        [SerializeField] private TextAnimType showAnimType;
        [SerializeField] private TextAnimType hideAnimType;
        [SerializeField] private bool isHideInvert;
        [SerializeField] private float displaySec;
        
        private TextMeshProUGUI _text;
        private TextAnimator_TMP _textAnimator;
        private TypewriterByCharacter _typewriter;

        private string _originText;
        private string _typewriterText;
        private string _displayText;
        private bool _isShowText = false;
        private const float WaitForNormalChars = 0.1f;

        private float _hideAnimDuration = 0f;
        private CancellationToken _ctx;

        private TextAnimSetting _showAnimSetting = NoneSetting();
        private TextAnimSetting _hideAnimSetting = NoneSetting();
        
        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _textAnimator = GetComponent<TextAnimator_TMP>();
            _typewriter = GetComponent<TypewriterByCharacter>();
            _originText = _text.text;
            _text.SetText(string.Empty);
            gameObject.SetActive(false);

            _textAnimator.typewriterStartsAutomatically = false;
            _typewriter.startTypewriterMode = TypewriterCore.StartTypewriterMode.OnShowText;
            _typewriter.useTypeWriter = true;
            _typewriter.waitForNormalChars = WaitForNormalChars;

            _ctx = this.GetCancellationTokenOnDestroy();

            if (showAnimType != TextAnimType.None)
            {
                var showSetting = showAnimType.GetSetting();
                var hideSetting = hideAnimType.GetSetting();
                Setup(showSetting, hideSetting);
            }
        }

        /// <summary>
        /// テキストのセット
        /// </summary>
        public void SetText(string text)
        {
            _originText = text;
            Setup(_showAnimSetting, _hideAnimSetting);
        }
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(TextAnimSetting showAnim) => Setup(showAnim, showAnim);

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(TextAnimSetting showAnim, TextAnimSetting hideAnim)
        {
            _typewriterText = _text.text;
            _typewriterText = _originText;
            _typewriterText = ConvertTagText(_typewriterText, showAnim, isHide: false);
            if (hideAnim.Type != TextAnimType.None)
            {
                _typewriterText = ConvertTagText(_typewriterText, hideAnim, isHide: true);
                _hideAnimDuration = hideAnim.Duration;
            }

            _showAnimSetting = showAnim;
            _hideAnimSetting = hideAnim;
        }

        /// <summary>
        /// タグテキストに変換
        /// </summary>
        private string ConvertTagText(string text, TextAnimSetting setting, bool isHide)
        {
            var prefix = setting.GetPrefix(isHide);
            var suffix = setting.GetSuffix(isHide);
            return prefix + text + suffix;
        }

        /// <summary>
        /// 表示
        /// </summary>
        public async UniTask ShowAsync()
        {
            gameObject.SetActive(true);
            if (_showAnimSetting.Type == TextAnimType.None)
            {
                return;
            }
            
            if (!_isShowText)
            {
                _isShowText = true;
                _typewriter.ShowText(_typewriterText);
            }
            else
            {
                _typewriter.StartShowingText(true);
            }
            await _typewriter.onTextShowed.AsObservable().ToUniTask(useFirstValue: true, cancellationToken: _ctx);
            
            if (!string.IsNullOrEmpty(_displayText))
            {
                _text.SetText(_displayText);
            }
        }

        /// <summary>
        /// 表示->非表示
        /// </summary>
        public async UniTask ShowAndHideAsync() => await ShowAndHideAsync(displaySec, isHideInvert);
        
        /// <summary>
        /// 表示->非表示
        /// </summary>
        public async UniTask ShowAndHideAsync(float waitSec, bool isInvert)
        {
            if (waitSec < 0)
            {
                await ShowAndImmediatelyHideAsync(-waitSec);
                return;
            }
            await ShowAsync();
            await UniTask.Delay(TimeSpan.FromSeconds(waitSec), cancellationToken: _ctx);
            await HideAsync(isInvert);
        }

        /// <summary>
        /// 表示と即時に非表示
        /// </summary>
        private async UniTask ShowAndImmediatelyHideAsync(float waitSec)
        {
            ShowAsync().Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(waitSec), cancellationToken: _ctx);
            await HideAsync(false);
        }

        /// <summary>
        /// 非表示
        /// </summary>
        public async UniTask HideAsync() => await HideAsync(isHideInvert);
        
        /// <summary>
        /// 非表示
        /// </summary>
        public async UniTask HideAsync(bool isInvert)
        {
            if (_hideAnimSetting.Type == TextAnimType.None)
            {
                gameObject.SetActive(false);
                return;
            }
            
            _typewriter.disappearanceOrientation = isInvert
                ? TypewriterCore.DisappearanceOrientation.Inverted
                : TypewriterCore.DisappearanceOrientation.SameAsTypewriter;
            _typewriter.StartDisappearingText();
            await _typewriter.onTextDisappeared.AsObservable().ToUniTask(useFirstValue: true, cancellationToken: _ctx);
            await UniTask.Delay(TimeSpan.FromSeconds(_hideAnimDuration), cancellationToken: _ctx);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// None設定
        /// </summary>
        public static TextAnimSetting NoneSetting() => new TextAnimSetting(TextAnimType.None, 0, 0, false, 0);

        /// <summary>
        /// Rot設定
        /// </summary>
        public static TextAnimSetting RotSetting() => new TextAnimSetting(TextAnimType.Rot, 2f, 0.1f, false, 0);

        /// <summary>
        /// Diagexp設定
        /// </summary>
        public static TextAnimSetting DiagexpSetting(bool isBottom) => new TextAnimSetting(TextAnimType.Diagexp, 0, 0.1f, isBottom, 0);

        /// <summary>
        /// Vertexp設定
        /// </summary>
        public static TextAnimSetting VertexpSetting(bool isBottom) => new TextAnimSetting(TextAnimType.Vertexp, 0, 0.1f, isBottom, 0);

        /// <summary>
        /// Offset設定
        /// </summary>
        public static TextAnimSetting OffsetSetting(float strength = 3) => new TextAnimSetting(TextAnimType.Offset, strength, 0.1f, false, 0);

        /// <summary>
        /// Fade設定
        /// </summary>
        public static TextAnimSetting FadeSetting() => new TextAnimSetting(TextAnimType.Fade, 0, 0.2f, false, 0);

        /// <summary>
        /// Size設定
        /// </summary>
        public static TextAnimSetting SizeSetting(float strength = 2) => new TextAnimSetting(TextAnimType.Size, -strength, 0.15f, false, 0);

        /// <summary>
        /// Rdir設定
        /// </summary>
        public static TextAnimSetting RdirSetting(float strength = 3) => new TextAnimSetting(TextAnimType.Rdir, strength, 0.15f, false, 0);
    }
}