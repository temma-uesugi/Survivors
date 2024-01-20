using System;

namespace App.Battle2.Core
{
    /// <summary>
    /// DIContainerに登録するクラスにつける属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ContainerRegisterAttribute2 : Attribute
    {
        public Type Type { get; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ContainerRegisterAttribute2(Type type)
        {
            Type = type;
        }
    }

    /// <summary>
    /// DIContainerに登録するMonoBehaviourクラスにつける属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ContainerRegisterMonoBehaviourAttribute2 : Attribute
    {
        public Type Type { get; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ContainerRegisterMonoBehaviourAttribute2(Type type)
        {
            Type = type;
        }
    }
}