
//generated file

using System.Collections.Generic;
using App.Localization.Kvs.Keys;

namespace App.Localization.Kvs.Values
{
    public partial class L10NString
    {
        [L10N(typeof(L10NKey.Status))]
        public Dictionary<string, string[]> Status { get; init; } = new()
        {
            
            {
                nameof(L10NKey.Status.Hp),
                new []
                {
                    "Hp", "Hp"
                }
            },
            {
                nameof(L10NKey.Status.AttackPower),
                new []
                {
                    "攻撃力", "Power"
                }
            },
            {
                nameof(L10NKey.Status.ShipSpeed),
                new []
                {
                    "速力", "Speed"
                }
            },
        };
    }
}
