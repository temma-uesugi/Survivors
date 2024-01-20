using App.AppCommon.Core;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace App.Battle._Test.MessageHub
{
    /// <summary>
    /// MessageHubのテスト
    /// </summary>
    public class MessageHubTestService : IStartable
    {
        private Publisher _publisher;
        private Subscriber _subscriber;
        private SubscriberObject _subscriberObject;
        
        [Inject]
        public void Construct(Publisher publisher, Subscriber subscriber, SubscriberObject subscriberObject)
        {
            Log.Debug("Construct");
            _publisher = publisher;
            _subscriber = subscriber;
            _subscriberObject = subscriberObject;
        }

        /// <summary>
        /// Start
        /// </summary>
        public void Start()
        {
            _subscriber.Setup();
            _subscriberObject.Setup();
            _publisher.SetupAsync().Forget();
        }
    }
}