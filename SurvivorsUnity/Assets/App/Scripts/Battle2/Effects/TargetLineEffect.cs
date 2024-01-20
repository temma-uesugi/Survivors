using UnityEngine;

namespace App.Battle2.Effects
{
    /// <summary>
    /// ターゲットラインエフェクト
    /// </summary>
    public class TargetLineEffect : MonoBehaviour
    {
        [SerializeField]
        private float _height = 1.0f;

        [SerializeField]
        private float _duration = 0.1f;

        [SerializeField]
        private float _interval = 0.2f;

        [SerializeField]
        private TrailRenderer _trail = null;

        [SerializeField]
        private AnimationCurve _curve = new AnimationCurve();

        // インターバル
        private float _intervalTime = 0;
        // 時間計測用
        private float _time = 0;

        private Vector3 _targetPosition;

        /// <summary>
        /// 位置設定
        /// </summary>
        public void SetPosition(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
        }
        
        private void LateUpdate()
		{
            // インターバル
            if(_intervalTime > 0)
            {
                _intervalTime -= Time.deltaTime;
                if(_intervalTime <= 0)
                {
                    _trail.transform.position = transform.position;
                    _trail.Clear();
                }
                return;
            }

            // 自分の座標
            Vector3 p = transform.position;
            // 対象物までのベクトル
            Vector3 v = _targetPosition - p;

            // 時間を計算
            float t = _time / _duration;

            // 高さ
            float h = _curve.Evaluate(t) * _height;
            // 座標計算
            p = p + v * t + new Vector3(0, h, 0);
            
            // トレイルの座標設定
            _trail.transform.position = p;
            // フレームを進める
            _time += Time.deltaTime;

            // 目的地までついた
            if(_time > _duration)
            {
                // インターバルを設定
                _intervalTime = _trail.time;
                // 時間リセット
                _time = 0;
            }
		}

        /// <summary>
        /// Clear
        /// </summary>
        public void Clear()
        {
            // トレイルの初期化
			if(_trail != null)
            {
                _trail.transform.position = transform.position;
                _trail.Clear();
            }
            // 時間の初期化
            _time = 0;
            _intervalTime = 0;
        }
        
		private void OnEnable()
		{
            Clear();
		}
	}
}