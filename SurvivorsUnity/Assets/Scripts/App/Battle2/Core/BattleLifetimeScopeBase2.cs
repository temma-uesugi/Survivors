using System.Reflection;
using VContainer;
using VContainer.Unity;

namespace App.Battle2.Core
{
    /// <summary>
    /// Battle用LifetimeScopeの拡張
    /// </summary>
    public class BattleLifetimeScopeBase2 : LifetimeScope
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
                ContainerRegisterAttribute2 contRegAttr = t.GetCustomAttribute<ContainerRegisterAttribute2>(false);
                if (contRegAttr != null)
                {
                    builder.Register(contRegAttr.Type, Lifetime.Singleton);
                    continue;
                }

                //MonoBehaviour系
                ContainerRegisterMonoBehaviourAttribute2 contRegMonoBhvAttr = t.GetCustomAttribute<ContainerRegisterMonoBehaviourAttribute2>(false);
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
                pointsBuilder.Add<ContainerRegisterMonoBehaviourResolver2>();
            });
        }
    }
}