using System;
using App.AppCommon;
using FastEnumUtility;
using UniRx;

namespace App.Battle.ValueObjects
{
    /// <summary>
    /// ステータス値
    /// </summary>
    public record StatusValue<T> where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
    {
        public T Max { get; init; }
        public T Current { get; private set; }
        private readonly bool _unsigned;
        private readonly Subject<T> _onUpdate = new();
        public IObservable<T> OnUpdate => _onUpdate;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StatusValue(T maxValue)
        {
            Max = maxValue;
            Current = maxValue;
            _unsigned = typeof(T) switch
            {
                var t when t == typeof(ushort) => true,
                var t when t == typeof(uint) => true,
                var t when t == typeof(ulong) => true,
                _ => false,
            };
            _onUpdate.OnNext(Current);
        }

        /// <summary>
        /// Max超えか
        /// </summary>
        private bool IsMaxOver(T value)
        {
            return value.CompareTo(Max) == 1;
        }

        /// <summary>
        /// Set
        /// </summary>
        public void Set(T value, bool overAllowed = false)
        {
            Current = value;
            if (IsMaxOver(value))
            {
                //最大値超え
                Current = overAllowed ? value : Max;
            }
            else
            {
                Current = value;
            }
            _onUpdate.OnNext(Current);
        }

        /// <summary>
        /// リセット
        /// </summary>
        public void Reset()
        {
            Current = Max;
            _onUpdate.OnNext(Current);
        }

        /// <summary>
        /// 加算
        /// </summary>
        public void Add(T addValue, bool overAllowed = false)
        {
            var curDec = Current.ToDecimal(null);
            var addDec = addValue.ToDecimal(null);
            if (addDec >= 0)
            {
                //加算
                Current = (T)Convert.ChangeType(curDec + addDec, typeof(T));
                if (IsMaxOver(Current) && !overAllowed)
                {
                    //Max超えかつMax超え許容していない
                    Current = Max;
                }
            }
            else
            {
                //減算
                if (curDec + addDec < 0 && _unsigned)
                {
                    //マイナスかつunsigned
                    Current = (T)Convert.ChangeType(0, typeof(T));
                }
                else
                {
                    //0以上もしくはunsignedではない
                    Current = (T)Convert.ChangeType(curDec + addDec, typeof(T));
                }
            }
            _onUpdate.OnNext(Current);
        }
    }
}