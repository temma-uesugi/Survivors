// <auto-generated />
#pragma warning disable CS0105
using Master.Constants;
using Master.Tables.Enemy;
using Master.Tables.Hero;
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;
using App.MD.Tables;

namespace App.MD
{
   public sealed class MemoryDatabase : MemoryDatabaseBase
   {
        public EnemyBaseTable EnemyBaseTable { get; private set; }
        public EnemyLevelStatusTable EnemyLevelStatusTable { get; private set; }
        public EnemySkillTable EnemySkillTable { get; private set; }
        public EnemySkillEffectTable EnemySkillEffectTable { get; private set; }
        public EnemySkillSetTable EnemySkillSetTable { get; private set; }
        public HeroFormationTable HeroFormationTable { get; private set; }
        public HeroFormationFrameTable HeroFormationFrameTable { get; private set; }

        public MemoryDatabase(
            EnemyBaseTable EnemyBaseTable,
            EnemyLevelStatusTable EnemyLevelStatusTable,
            EnemySkillTable EnemySkillTable,
            EnemySkillEffectTable EnemySkillEffectTable,
            EnemySkillSetTable EnemySkillSetTable,
            HeroFormationTable HeroFormationTable,
            HeroFormationFrameTable HeroFormationFrameTable
        )
        {
            this.EnemyBaseTable = EnemyBaseTable;
            this.EnemyLevelStatusTable = EnemyLevelStatusTable;
            this.EnemySkillTable = EnemySkillTable;
            this.EnemySkillEffectTable = EnemySkillEffectTable;
            this.EnemySkillSetTable = EnemySkillSetTable;
            this.HeroFormationTable = HeroFormationTable;
            this.HeroFormationFrameTable = HeroFormationFrameTable;
        }

        public MemoryDatabase(byte[] databaseBinary, bool internString = true, MessagePack.IFormatterResolver formatterResolver = null, int maxDegreeOfParallelism = 1)
            : base(databaseBinary, internString, formatterResolver, maxDegreeOfParallelism)
        {
        }

        protected override void Init(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options, int maxDegreeOfParallelism)
        {
            if(maxDegreeOfParallelism == 1)
            {
                InitSequential(header, databaseBinary, options, maxDegreeOfParallelism);
            }
            else
            {
                InitParallel(header, databaseBinary, options, maxDegreeOfParallelism);
            }
        }

        void InitSequential(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options, int maxDegreeOfParallelism)
        {
            this.EnemyBaseTable = ExtractTableData<EnemyBase, EnemyBaseTable>(header, databaseBinary, options, xs => new EnemyBaseTable(xs));
            this.EnemyLevelStatusTable = ExtractTableData<EnemyLevelStatus, EnemyLevelStatusTable>(header, databaseBinary, options, xs => new EnemyLevelStatusTable(xs));
            this.EnemySkillTable = ExtractTableData<EnemySkill, EnemySkillTable>(header, databaseBinary, options, xs => new EnemySkillTable(xs));
            this.EnemySkillEffectTable = ExtractTableData<EnemySkillEffect, EnemySkillEffectTable>(header, databaseBinary, options, xs => new EnemySkillEffectTable(xs));
            this.EnemySkillSetTable = ExtractTableData<EnemySkillSet, EnemySkillSetTable>(header, databaseBinary, options, xs => new EnemySkillSetTable(xs));
            this.HeroFormationTable = ExtractTableData<HeroFormation, HeroFormationTable>(header, databaseBinary, options, xs => new HeroFormationTable(xs));
            this.HeroFormationFrameTable = ExtractTableData<HeroFormationFrame, HeroFormationFrameTable>(header, databaseBinary, options, xs => new HeroFormationFrameTable(xs));
        }

        void InitParallel(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options, int maxDegreeOfParallelism)
        {
            var extracts = new Action[]
            {
                () => this.EnemyBaseTable = ExtractTableData<EnemyBase, EnemyBaseTable>(header, databaseBinary, options, xs => new EnemyBaseTable(xs)),
                () => this.EnemyLevelStatusTable = ExtractTableData<EnemyLevelStatus, EnemyLevelStatusTable>(header, databaseBinary, options, xs => new EnemyLevelStatusTable(xs)),
                () => this.EnemySkillTable = ExtractTableData<EnemySkill, EnemySkillTable>(header, databaseBinary, options, xs => new EnemySkillTable(xs)),
                () => this.EnemySkillEffectTable = ExtractTableData<EnemySkillEffect, EnemySkillEffectTable>(header, databaseBinary, options, xs => new EnemySkillEffectTable(xs)),
                () => this.EnemySkillSetTable = ExtractTableData<EnemySkillSet, EnemySkillSetTable>(header, databaseBinary, options, xs => new EnemySkillSetTable(xs)),
                () => this.HeroFormationTable = ExtractTableData<HeroFormation, HeroFormationTable>(header, databaseBinary, options, xs => new HeroFormationTable(xs)),
                () => this.HeroFormationFrameTable = ExtractTableData<HeroFormationFrame, HeroFormationFrameTable>(header, databaseBinary, options, xs => new HeroFormationFrameTable(xs)),
            };
            
            System.Threading.Tasks.Parallel.Invoke(new System.Threading.Tasks.ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            }, extracts);
        }

        public ImmutableBuilder ToImmutableBuilder()
        {
            return new ImmutableBuilder(this);
        }

        public DatabaseBuilder ToDatabaseBuilder()
        {
            var builder = new DatabaseBuilder();
            builder.Append(this.EnemyBaseTable.GetRawDataUnsafe());
            builder.Append(this.EnemyLevelStatusTable.GetRawDataUnsafe());
            builder.Append(this.EnemySkillTable.GetRawDataUnsafe());
            builder.Append(this.EnemySkillEffectTable.GetRawDataUnsafe());
            builder.Append(this.EnemySkillSetTable.GetRawDataUnsafe());
            builder.Append(this.HeroFormationTable.GetRawDataUnsafe());
            builder.Append(this.HeroFormationFrameTable.GetRawDataUnsafe());
            return builder;
        }

        public DatabaseBuilder ToDatabaseBuilder(MessagePack.IFormatterResolver resolver)
        {
            var builder = new DatabaseBuilder(resolver);
            builder.Append(this.EnemyBaseTable.GetRawDataUnsafe());
            builder.Append(this.EnemyLevelStatusTable.GetRawDataUnsafe());
            builder.Append(this.EnemySkillTable.GetRawDataUnsafe());
            builder.Append(this.EnemySkillEffectTable.GetRawDataUnsafe());
            builder.Append(this.EnemySkillSetTable.GetRawDataUnsafe());
            builder.Append(this.HeroFormationTable.GetRawDataUnsafe());
            builder.Append(this.HeroFormationFrameTable.GetRawDataUnsafe());
            return builder;
        }

#if !DISABLE_MASTERMEMORY_VALIDATOR

        public ValidateResult Validate()
        {
            var result = new ValidateResult();
            var database = new ValidationDatabase(new object[]
            {
                EnemyBaseTable,
                EnemyLevelStatusTable,
                EnemySkillTable,
                EnemySkillEffectTable,
                EnemySkillSetTable,
                HeroFormationTable,
                HeroFormationFrameTable,
            });

            ((ITableUniqueValidate)EnemyBaseTable).ValidateUnique(result);
            ValidateTable(EnemyBaseTable.All, database, "EnemyId", EnemyBaseTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)EnemyLevelStatusTable).ValidateUnique(result);
            ValidateTable(EnemyLevelStatusTable.All, database, "(EnemyId, Level)", EnemyLevelStatusTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)EnemySkillTable).ValidateUnique(result);
            ValidateTable(EnemySkillTable.All, database, "SkillId", EnemySkillTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)EnemySkillEffectTable).ValidateUnique(result);
            ValidateTable(EnemySkillEffectTable.All, database, "EffectId", EnemySkillEffectTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)EnemySkillSetTable).ValidateUnique(result);
            ValidateTable(EnemySkillSetTable.All, database, "(SkillSetId, SkillId)", EnemySkillSetTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)HeroFormationTable).ValidateUnique(result);
            ValidateTable(HeroFormationTable.All, database, "FormationId", HeroFormationTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)HeroFormationFrameTable).ValidateUnique(result);
            ValidateTable(HeroFormationFrameTable.All, database, "FormationFrameId", HeroFormationFrameTable.PrimaryKeySelector, result);

            return result;
        }

#endif

        static MasterMemory.Meta.MetaDatabase metaTable;

        public static object GetTable(MemoryDatabase db, string tableName)
        {
            switch (tableName)
            {
                case "EnemyBase":
                    return db.EnemyBaseTable;
                case "EnemyStatus":
                    return db.EnemyLevelStatusTable;
                case "EnemySkill":
                    return db.EnemySkillTable;
                case "EnemySkillEffect":
                    return db.EnemySkillEffectTable;
                case "EnemySkillSet":
                    return db.EnemySkillSetTable;
                case "HeroFormation":
                    return db.HeroFormationTable;
                case "HeroFormationFrame":
                    return db.HeroFormationFrameTable;
                
                default:
                    return null;
            }
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static MasterMemory.Meta.MetaDatabase GetMetaDatabase()
        {
            if (metaTable != null) return metaTable;

            var dict = new Dictionary<string, MasterMemory.Meta.MetaTable>();
            dict.Add("EnemyBase", App.MD.Tables.EnemyBaseTable.CreateMetaTable());
            dict.Add("EnemyStatus", App.MD.Tables.EnemyLevelStatusTable.CreateMetaTable());
            dict.Add("EnemySkill", App.MD.Tables.EnemySkillTable.CreateMetaTable());
            dict.Add("EnemySkillEffect", App.MD.Tables.EnemySkillEffectTable.CreateMetaTable());
            dict.Add("EnemySkillSet", App.MD.Tables.EnemySkillSetTable.CreateMetaTable());
            dict.Add("HeroFormation", App.MD.Tables.HeroFormationTable.CreateMetaTable());
            dict.Add("HeroFormationFrame", App.MD.Tables.HeroFormationFrameTable.CreateMetaTable());

            metaTable = new MasterMemory.Meta.MetaDatabase(dict);
            return metaTable;
        }

#endif
    }
}