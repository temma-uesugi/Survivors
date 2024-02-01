// <auto-generated />
#pragma warning disable CS0105
using Master.Constants;
using Master.Tables.Enemy;
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;

namespace App.MD.Tables
{
   public sealed partial class EnemyBaseTable : TableBase<EnemyBase>, ITableUniqueValidate
   {
        public Func<EnemyBase, uint> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<EnemyBase, uint> primaryIndexSelector;


        public EnemyBaseTable(EnemyBase[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.EnemyId;
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public EnemyBase FindByEnemyId(uint key)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].EnemyId;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { return data[mid]; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            return ThrowKeyNotFound(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryFindByEnemyId(uint key, out EnemyBase result)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].EnemyId;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { result = data[mid]; return true; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            result = default;
            return false;
        }

        public EnemyBase FindClosestByEnemyId(uint key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<uint>.Default, key, selectLower);
        }

        public RangeView<EnemyBase> FindRangeByEnemyId(uint min, uint max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<uint>.Default, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
#if !DISABLE_MASTERMEMORY_VALIDATOR

            ValidateUniqueCore(data, primaryIndexSelector, "EnemyId", resultSet);       

#endif
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(EnemyBase), typeof(EnemyBaseTable), "EnemyBase",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(EnemyBase).GetProperty("EnemyId")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemyBase).GetProperty("EnemyName")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemyBase).GetProperty("ActionInterval")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemyBase).GetProperty("MovePower")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemyBase).GetProperty("SkillSetId")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemyBase).GetProperty("ActiveConditionType")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemyBase).GetProperty("ActiveConditionValue")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemyBase).GetProperty("InactiveConditionType")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemyBase).GetProperty("InactiveConditionValue")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemyBase).GetProperty("IsFlight")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemyBase).GetProperty("ImageId")),
                    new MasterMemory.Meta.MetaProperty(typeof(EnemyBase).GetProperty("Hoge")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(EnemyBase).GetProperty("EnemyId"),
                    }, true, true, System.Collections.Generic.Comparer<uint>.Default),
                });
        }

#endif
    }
}