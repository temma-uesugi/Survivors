// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1129 // Do not use default value type constructor
#pragma warning disable SA1309 // Field names should not begin with underscore
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Formatters.Master.Tables.Enemy
{
    public sealed class EnemySkillEffectFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Master.Tables.Enemy.EnemySkillEffectEntity>
    {
        // EffectId
        private static global::System.ReadOnlySpan<byte> GetSpan_EffectId() => new byte[1 + 8] { 168, 69, 102, 102, 101, 99, 116, 73, 100 };
        // Type
        private static global::System.ReadOnlySpan<byte> GetSpan_Type() => new byte[1 + 4] { 164, 84, 121, 112, 101 };
        // Value
        private static global::System.ReadOnlySpan<byte> GetSpan_Value() => new byte[1 + 5] { 165, 86, 97, 108, 117, 101 };
        // RangeType
        private static global::System.ReadOnlySpan<byte> GetSpan_RangeType() => new byte[1 + 9] { 169, 82, 97, 110, 103, 101, 84, 121, 112, 101 };
        // RangeValue
        private static global::System.ReadOnlySpan<byte> GetSpan_RangeValue() => new byte[1 + 10] { 170, 82, 97, 110, 103, 101, 86, 97, 108, 117, 101 };
        // ImageId
        private static global::System.ReadOnlySpan<byte> GetSpan_ImageId() => new byte[1 + 7] { 167, 73, 109, 97, 103, 101, 73, 100 };

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Master.Tables.Enemy.EnemySkillEffectEntity value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            var formatterResolver = options.Resolver;
            writer.WriteMapHeader(6);
            writer.WriteRaw(GetSpan_EffectId());
            writer.Write(value.EffectId);
            writer.WriteRaw(GetSpan_Type());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Master.Constants.SkillEffectType>(formatterResolver).Serialize(ref writer, value.Type, options);
            writer.WriteRaw(GetSpan_Value());
            writer.Write(value.Value);
            writer.WriteRaw(GetSpan_RangeType());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Master.Constants.SkillEffectRangeType>(formatterResolver).Serialize(ref writer, value.RangeType, options);
            writer.WriteRaw(GetSpan_RangeValue());
            writer.Write(value.RangeValue);
            writer.WriteRaw(GetSpan_ImageId());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Serialize(ref writer, value.ImageId, options);
        }

        public global::Master.Tables.Enemy.EnemySkillEffectEntity Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var formatterResolver = options.Resolver;
            var length = reader.ReadMapHeader();
            var __EffectId__ = default(uint);
            var __Type__ = default(global::Master.Constants.SkillEffectType);
            var __Value__ = default(float);
            var __RangeType__ = default(global::Master.Constants.SkillEffectRangeType);
            var __RangeValue__ = default(int);
            var __ImageId__ = default(string);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                switch (stringKey.Length)
                {
                    default:
                    FAIL:
                      reader.Skip();
                      continue;
                    case 8:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 7226435047344465477UL) { goto FAIL; }

                        __EffectId__ = reader.ReadUInt32();
                        continue;
                    case 4:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 1701869908UL) { goto FAIL; }

                        __Type__ = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Master.Constants.SkillEffectType>(formatterResolver).Deserialize(ref reader, options);
                        continue;
                    case 5:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 435761733974UL) { goto FAIL; }

                        __Value__ = reader.ReadSingle();
                        continue;
                    case 9:
                        if (!global::System.MemoryExtensions.SequenceEqual(stringKey, GetSpan_RangeType().Slice(1))) { goto FAIL; }

                        __RangeType__ = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Master.Constants.SkillEffectRangeType>(formatterResolver).Deserialize(ref reader, options);
                        continue;
                    case 10:
                        if (!global::System.MemoryExtensions.SequenceEqual(stringKey, GetSpan_RangeValue().Slice(1))) { goto FAIL; }

                        __RangeValue__ = reader.ReadInt32();
                        continue;
                    case 7:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 28228197546028361UL) { goto FAIL; }

                        __ImageId__ = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Deserialize(ref reader, options);
                        continue;

                }
            }

            var ____result = new global::Master.Tables.Enemy.EnemySkillEffectEntity(__EffectId__, __Type__, __Value__, __RangeType__, __RangeValue__, __ImageId__);
            reader.Depth--;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1129 // Do not use default value type constructor
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name
