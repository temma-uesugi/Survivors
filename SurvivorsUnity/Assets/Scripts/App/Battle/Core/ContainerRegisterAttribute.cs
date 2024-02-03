using System;

namespace App.Battle.Core
{
    /// <summary>
    /// DIContainerに登録するクラスにつける属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ContainerRegisterAttribute : Attribute
    {
        public Type Type { get; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ContainerRegisterAttribute(Type type)
        {
            Type = type;
        }
    }

    /// <summary>
    /// DIContainerに登録するMonoBehaviourクラスにつける属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ContainerRegisterMonoBehaviourAttribute : Attribute
    {
        public Type Type { get; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ContainerRegisterMonoBehaviourAttribute(Type type)
        {
            Type = type;
        }
    }
}