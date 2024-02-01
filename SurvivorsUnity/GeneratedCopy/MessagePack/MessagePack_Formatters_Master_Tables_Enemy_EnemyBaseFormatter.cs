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
    public sealed class EnemyBaseFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Master.Tables.Enemy.EnemyBase>
    {
        // EnemyId
        private static global::System.ReadOnlySpan<byte> GetSpan_EnemyId() => new byte[1 + 7] { 167, 69, 110, 101, 109, 121, 73, 100 };
        // EnemyName
        private static global::System.ReadOnlySpan<byte> GetSpan_EnemyName() => new byte[1 + 9] { 169, 69, 110, 101, 109, 121, 78, 97, 109, 101 };
        // ActionInterval
        private static global::System.ReadOnlySpan<byte> GetSpan_ActionInterval() => new byte[1 + 14] { 174, 65, 99, 116, 105, 111, 110, 73, 110, 116, 101, 114, 118, 97, 108 };
        // MovePower
        private static global::System.ReadOnlySpan<byte> GetSpan_MovePower() => new byte[1 + 9] { 169, 77, 111, 118, 101, 80, 111, 119, 101, 114 };
        // SkillSetId
        private static global::System.ReadOnlySpan<byte> GetSpan_SkillSetId() => new byte[1 + 10] { 170, 83, 107, 105, 108, 108, 83, 101, 116, 73, 100 };
        // ActiveConditionType
        private static global::System.ReadOnlySpan<byte> GetSpan_ActiveConditionType() => new byte[1 + 19] { 179, 65, 99, 116, 105, 118, 101, 67, 111, 110, 100, 105, 116, 105, 111, 110, 84, 121, 112, 101 };
        // ActiveConditionValue
        private static global::System.ReadOnlySpan<byte> GetSpan_ActiveConditionValue() => new byte[1 + 20] { 180, 65, 99, 116, 105, 118, 101, 67, 111, 110, 100, 105, 116, 105, 111, 110, 86, 97, 108, 117, 101 };
        // InactiveConditionType
        private static global::System.ReadOnlySpan<byte> GetSpan_InactiveConditionType() => new byte[1 + 21] { 181, 73, 110, 97, 99, 116, 105, 118, 101, 67, 111, 110, 100, 105, 116, 105, 111, 110, 84, 121, 112, 101 };
        // InactiveConditionValue
        private static global::System.ReadOnlySpan<byte> GetSpan_InactiveConditionValue() => new byte[1 + 22] { 182, 73, 110, 97, 99, 116, 105, 118, 101, 67, 111, 110, 100, 105, 116, 105, 111, 110, 86, 97, 108, 117, 101 };
        // IsFlight
        private static global::System.ReadOnlySpan<byte> GetSpan_IsFlight() => new byte[1 + 8] { 168, 73, 115, 70, 108, 105, 103, 104, 116 };
        // ImageId
        private static global::System.ReadOnlySpan<byte> GetSpan_ImageId() => new byte[1 + 7] { 167, 73, 109, 97, 103, 101, 73, 100 };
        // Hoge
        private static global::System.ReadOnlySpan<byte> GetSpan_Hoge() => new byte[1 + 4] { 164, 72, 111, 103, 101 };

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Master.Tables.Enemy.EnemyBase value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            var formatterResolver = options.Resolver;
            writer.WriteMapHeader(12);
            writer.WriteRaw(GetSpan_EnemyId());
            writer.Write(value.EnemyId);
            writer.WriteRaw(GetSpan_EnemyName());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Serialize(ref writer, value.EnemyName, options);
            writer.WriteRaw(GetSpan_ActionInterval());
            writer.Write(value.ActionInterval);
            writer.WriteRaw(GetSpan_MovePower());
            writer.Write(value.MovePower);
            writer.WriteRaw(GetSpan_SkillSetId());
            writer.Write(value.SkillSetId);
            writer.WriteRaw(GetSpan_ActiveConditionType());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Master.Constants.EnemyActiveConditionType>(formatterResolver).Serialize(ref writer, value.ActiveConditionType, options);
            writer.WriteRaw(GetSpan_ActiveConditionValue());
            writer.Write(value.ActiveConditionValue);
            writer.WriteRaw(GetSpan_InactiveConditionType());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Master.Constants.EnemyInactiveConditionType>(formatterResolver).Serialize(ref writer, value.InactiveConditionType, options);
            writer.WriteRaw(GetSpan_InactiveConditionValue());
            writer.Write(value.InactiveConditionValue);
            writer.WriteRaw(GetSpan_IsFlight());
            writer.Write(value.IsFlight);
            writer.WriteRaw(GetSpan_ImageId());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Serialize(ref writer, value.ImageId, options);
            writer.WriteRaw(GetSpan_Hoge());
            writer.Write(value.Hoge);
        }

        public global::Master.Tables.Enemy.EnemyBase Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var formatterResolver = options.Resolver;
            var length = reader.ReadMapHeader();
            var __EnemyId__ = default(uint);
            var __EnemyName__ = default(string);
            var __ActionInterval__ = default(int);
            var __MovePower__ = default(int);
            var __SkillSetId__ = default(uint);
            var __ActiveConditionType__ = default(global::Master.Constants.EnemyActiveConditionType);
            var __ActiveConditionValue__ = default(int);
            var __InactiveConditionType__ = default(global::Master.Constants.EnemyInactiveConditionType);
            var __InactiveConditionValue__ = default(int);
            var __IsFlight__ = default(bool);
            var __ImageId__ = default(string);
            var __Hoge__IsInitialized = false;
            var __Hoge__ = default(int);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                switch (stringKey.Length)
                {
                    default:
                    FAIL:
                      reader.Skip();
                      continue;
                    case 7:
                        switch (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey))
                        {
                            default: goto FAIL;
                            case 28228283546299973UL:
                                __EnemyId__ = reader.ReadUInt32();
                                continue;
                            case 28228197546028361UL:
                                __ImageId__ = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Deserialize(ref reader, options);
                                continue;
                        }
                    case 9:
                        switch (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey))
                        {
                            default: goto FAIL;
                            case 7881667106308451909UL:
                                if (stringKey[0] != 101) { goto FAIL; }

                                __EnemyName__ = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Deserialize(ref reader, options);
                                continue;

                            case 7311434911149616973UL:
                                if (stringKey[0] != 114) { goto FAIL; }

                                __MovePower__ = reader.ReadInt32();
                                continue;

                        }
                    case 14:
                        if (!global::System.MemoryExtensions.SequenceEqual(stringKey, GetSpan_ActionInterval().Slice(1))) { goto FAIL; }

                        __ActionInterval__ = reader.ReadInt32();
                        continue;
                    case 10:
                        if (!global::System.MemoryExtensions.SequenceEqual(stringKey, GetSpan_SkillSetId().Slice(1))) { goto FAIL; }

                        __SkillSetId__ = reader.ReadUInt32();
                        continue;
                    case 19:
                        if (!global::System.MemoryExtensions.SequenceEqual(stringKey, GetSpan_ActiveConditionType().Slice(1))) { goto FAIL; }

                        __ActiveConditionType__ = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Master.Constants.EnemyActiveConditionType>(formatterResolver).Deserialize(ref reader, options);
                        continue;
                    case 20:
                        if (!global::System.MemoryExtensions.SequenceEqual(stringKey, GetSpan_ActiveConditionValue().Slice(1))) { goto FAIL; }

                        __ActiveConditionValue__ = reader.ReadInt32();
                        continue;
                    case 21:
                        if (!global::System.MemoryExtensions.SequenceEqual(stringKey, GetSpan_InactiveConditionType().Slice(1))) { goto FAIL; }

                        __InactiveConditionType__ = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Master.Constants.EnemyInactiveConditionType>(formatterResolver).Deserialize(ref reader, options);
                        continue;
                    case 22:
                        if (!global::System.MemoryExtensions.SequenceEqual(stringKey, GetSpan_InactiveConditionValue().Slice(1))) { goto FAIL; }

                        __InactiveConditionValue__ = reader.ReadInt32();
                        continue;
                    case 8:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 8388068008463332169UL) { goto FAIL; }

                        __IsFlight__ = reader.ReadBoolean();
                        continue;
                    case 4:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 1701277512UL) { goto FAIL; }

                        __Hoge__IsInitialized = true;
                        __Hoge__ = reader.ReadInt32();
                        continue;

                }
            }

            var ____result = new global::Master.Tables.Enemy.EnemyBase(__EnemyId__, __EnemyName__, __ActionInterval__, __MovePower__, __SkillSetId__, __ActiveConditionType__, __ActiveConditionValue__, __InactiveConditionType__, __InactiveConditionValue__, __IsFlight__, __ImageId__);
            if (__Hoge__IsInitialized)
            {
                ____result.Hoge = __Hoge__;
            }

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
