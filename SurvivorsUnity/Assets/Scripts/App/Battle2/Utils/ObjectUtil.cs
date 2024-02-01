using App.AppCommon;
using Master.Constants;

namespace App.Battle2.Utils
{
    /// <summary>
    /// ObjectUtil
    /// </summary>
    public static class ObjectUtil
    {
        //Note: 複数スレッドからのアクセスの想定なし
        private static uint _seqId = 0;
        
        /// <summary>
        /// GetObjectId
        /// </summary>
        public static uint GetObjectId()
        {
            return ++_seqId + GameConst.ObjectIdOffset;
        }
    }
}