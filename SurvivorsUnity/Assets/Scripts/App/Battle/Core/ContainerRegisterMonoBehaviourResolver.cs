using System;
using System.Reflection;
using App.AppCommon.Core;
using VContainer;
using VContainer.Unity;

namespace Master.Battle.Core
{
    /// <summary>
    /// ContainerRegisterMonoBehaviourAttributeが設定されたシーン内のObjectをDIContainerのインスタンスとして登録するためのもの
    /// </summary>
    [ContainerRegister(typeof(ContainerRegisterMonoBehaviourResolver))]
    public class ContainerRegisterMonoBehaviourResolver : IStartable
    {
        private readonly IObjectResolver _resolver;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public ContainerRegisterMonoBehaviourResolver(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Start
        /// </summary>
        public void Start()
        {
            var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var t in Assembly.GetExecutingAssembly().GetTypes())
            {
                ContainerRegisterAttribute contRegAttr = t.GetCustomAttribute<ContainerRegisterAttribute>(false);
                if (contRegAttr != null)
                {
                    try
                    {
                        _resolver.Resolve(contRegAttr.Type);
                    }
                    catch (Exception e)
                    {
                        Log.Warning("ContainerRegisterAttribute ResolveFailed", contRegAttr.Type, e.Message);
                        continue;
                    }
                    continue;
                }

                //MonoBehaviour
                ContainerRegisterMonoBehaviourAttribute contRegMonoBhvAttr =
                    t.GetCustomAttribute<ContainerRegisterMonoBehaviourAttribute>(false);
                if (contRegMonoBhvAttr != null)
                {
                    foreach (var rootObj in rootObjects)
                    {
                        var obj = rootObj.GetComponentInChildren(contRegMonoBhvAttr.Type, true);
                        if (obj != null)
                        {
                            try
                            {
                                _resolver.Resolve(contRegMonoBhvAttr.Type);
                            }
                            catch (Exception e)
                            {
                                Log.Warning("ContainerRegisterMonoBehaviourAttribute ResolveFailed", contRegMonoBhvAttr.Type, e.Message);
                                continue;
                            }
                        }
                    }
                }
            }
        }
    }
}