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
    public class MasterMemoryResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new MasterMemoryResolver();

        MasterMemoryResolver()
        {

        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                var f = MasterMemoryResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class MasterMemoryResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static MasterMemoryResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(7)
            {
                {typeof(EnemyBase[]), 0 },
                {typeof(EnemyLevelStatus[]), 1 },
                {typeof(EnemySkill[]), 2 },
                {typeof(EnemySkillEffect[]), 3 },
                {typeof(EnemySkillSet[]), 4 },
                {typeof(HeroFormation[]), 5 },
                {typeof(HeroFormationFrame[]), 6 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new MessagePack.Formatters.ArrayFormatter<EnemyBase>();
                case 1: return new MessagePack.Formatters.ArrayFormatter<EnemyLevelStatus>();
                case 2: return new MessagePack.Formatters.ArrayFormatter<EnemySkill>();
                case 3: return new MessagePack.Formatters.ArrayFormatter<EnemySkillEffect>();
                case 4: return new MessagePack.Formatters.ArrayFormatter<EnemySkillSet>();
                case 5: return new MessagePack.Formatters.ArrayFormatter<HeroFormation>();
                case 6: return new MessagePack.Formatters.ArrayFormatter<HeroFormationFrame>();
                default: return null;
            }
        }
    }
}