// <auto-generated />
#pragma warning disable CS0105
using Master.Tables.Hero;
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;

namespace App.MD.Tables
{
   public sealed partial class HeroFormationTable : TableBase<HeroFormationEntity>, ITableUniqueValidate
   {
        public Func<HeroFormationEntity, uint> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<HeroFormationEntity, uint> primaryIndexSelector;


        public HeroFormationTable(HeroFormationEntity[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.FormationId;
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public HeroFormationEntity FindByFormationId(uint key)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].FormationId;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { return data[mid]; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            return ThrowKeyNotFound(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryFindByFormationId(uint key, out HeroFormationEntity result)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].FormationId;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { result = data[mid]; return true; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            result = default;
            return false;
        }

        public HeroFormationEntity FindClosestByFormationId(uint key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<uint>.Default, key, selectLower);
        }

        public RangeView<HeroFormationEntity> FindRangeByFormationId(uint min, uint max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<uint>.Default, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
#if !DISABLE_MASTERMEMORY_VALIDATOR

            ValidateUniqueCore(data, primaryIndexSelector, "FormationId", resultSet);       

#endif
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(HeroFormationEntity), typeof(HeroFormationTable), "HeroFormation",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(HeroFormationEntity).GetProperty("FormationId")),
                    new MasterMemory.Meta.MetaProperty(typeof(HeroFormationEntity).GetProperty("Name")),
                    new MasterMemory.Meta.MetaProperty(typeof(HeroFormationEntity).GetProperty("Description")),
                    new MasterMemory.Meta.MetaProperty(typeof(HeroFormationEntity).GetProperty("FrontDamageCutCoef")),
                    new MasterMemory.Meta.MetaProperty(typeof(HeroFormationEntity).GetProperty("SideDamageCutCoef")),
                    new MasterMemory.Meta.MetaProperty(typeof(HeroFormationEntity).GetProperty("BackDamageCutCoef")),
                    new MasterMemory.Meta.MetaProperty(typeof(HeroFormationEntity).GetProperty("BonusHpCoef")),
                    new MasterMemory.Meta.MetaProperty(typeof(HeroFormationEntity).GetProperty("BonusDamageCutCoef")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(HeroFormationEntity).GetProperty("FormationId"),
                    }, true, true, System.Collections.Generic.Comparer<uint>.Default),
                });
        }

#endif
    }
}