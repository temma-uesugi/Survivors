// <auto-generated />
#pragma warning disable CS0105
using App.Master.Tables;
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;

namespace App.MD.Tables
{
   public sealed partial class EnemySkillEffectTable : TableBase<EnemySkillEffect>, ITableUniqueValidate
   {
        public Func<EnemySkillEffect, uint> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<EnemySkillEffect, uint> primaryIndexSelector;


        public EnemySkillEffectTable(EnemySkillEffect[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.EffectId;
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public EnemySkillEffect FindByEffectId(uint key)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].EffectId;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { return data[mid]; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            return ThrowKeyNotFound(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryFindByEffectId(uint key, out EnemySkillEffect result)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].EffectId;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { result = data[mid]; return true; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            result = default;
            return false;
        }

        public EnemySkillEffect FindClosestByEffectId(uint key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<uint>.Default, key, selectLower);
        }

        public RangeView<EnemySkillEffect> FindRangeByEffectId(uint min, uint max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<uint>.Default, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
#if !DISABLE_MASTERMEMORY_VALIDATOR

            ValidateUniqueCore(data, primaryIndexSelector, "EffectId", resultSet);       

#endif
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(EnemySkillEffect), typeof(EnemySkillEffectTable), "EnemySkillEffect",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(EnemySkillEffect).GetProperty("EffectId")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemySkillEffect).GetProperty("Type")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemySkillEffect).GetProperty("Value")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemySkillEffect).GetProperty("RangeType")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemySkillEffect).GetProperty("RangeValue")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemySkillEffect).GetProperty("ImageId")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(EnemySkillEffect).GetProperty("EffectId"),
                    }, true, true, System.Collections.Generic.Comparer<uint>.Default),
                });
        }

#endif
    }
}