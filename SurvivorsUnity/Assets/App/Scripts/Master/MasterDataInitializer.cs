using App.MD;
using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;

namespace App.Master
{
    /// <summary>
    /// MasterDataInitializer
    /// </summary>
    public class MasterDataInitializer
    {
        /// <summary>
        /// 初期化
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Initialize()
        {
            StaticCompositeResolver.Instance.Register
            (
                MasterMemoryResolver.Instance,
                GeneratedResolver.Instance,
                StandardResolver.Instance
            );
            var options = MessagePackSerializerOptions.Standard.WithResolver( StaticCompositeResolver.Instance );
            MessagePackSerializer.DefaultOptions = options;    
        }
    }
}