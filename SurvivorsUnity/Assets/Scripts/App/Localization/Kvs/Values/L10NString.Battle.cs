
//generated file

using System.Collections.Generic;
using App.Localization.Kvs.Keys;

namespace App.Localization.Kvs.Values
{
    public partial class L10NString
    {
        [L10N(typeof(L10NKey.Battle))]
        public Dictionary<string, string[]> Battle { get; init; } = new()
        {
            
            {
                nameof(L10NKey.Battle.Assault),
                new []
                {
                    "突撃{0:#,0.00}", "Assault"
                }
            },
            {
                nameof(L10NKey.Battle.Bomb),
                new []
                {
                    "砲撃", "Bomb"
                }
            },
        };
    }
}
