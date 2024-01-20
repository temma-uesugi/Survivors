using VContainer.Unity;
using System.Reflection;
using VContainer;

namespace App.Battle.Core
{
    /// <summary>
    /// Battle用LifetimeScopeの拡張
    /// </summary>
    public class BattleLifetimeScopeBase : LifetimeScope
    {
        /// <summary>
        /// 設定
        /// </summary>
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var t in Assembly.GetExecutingAssembly().GetTypes())
            {
                ContainerRegisterAttribute contRegAttr = t.GetCustomAttribute<ContainerRegisterAttribute>(false);
                if (contRegAttr != null)
                {
                    builder.Register(contRegAttr.Type, Lifetime.Singleton);
                    continue;
                }

                //MonoBehaviour系
                ContainerRegisterMonoBehaviourAttribute contRegMonoBhvAttr = t.GetCustomAttribute<ContainerRegisterMonoBehaviourAttribute>(false);
                if (contRegMonoBhvAttr != null)
                {
                    foreach (var rootObj in rootObjects)
                    {
                        var obj = rootObj.GetComponentInChildren(contRegMonoBhvAttr.Type, true);
                        if (obj != null)
                        {
                            builder.RegisterComponent(obj).As(contRegMonoBhvAttr.Type);
                        }
                    }
                }
            }

            builder.UseEntryPoints(Lifetime.Singleton, pointsBuilder =>
            {
                pointsBuilder.Add<ContainerRegisterMonoBehaviourResolver>();
            });
        }
    }
}