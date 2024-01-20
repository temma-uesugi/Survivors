using MasterMemory;
using MessagePack;

namespace App.Master.Tables
{
    /// <summary>
    /// 敵基本
    /// </summary>
    [MemoryTable("EnemyBase"), MessagePackObject(true)]
    public class EnemyBase
    {
        /// <summary> 敵ID </summary>
        [PrimaryKey]
        public uint EnemyId { get; set; }
        
        /// <summary> 敵名前 </summary>
        public string EnemyName { get; set; }
        
        /// <summary> 行動間隔 </summary>
        public int ActionInterval { get; set; }
        
        /// <summary> 移動力 </summary>
        public int MovePower { get; set; }
    
        /// <summary> スキルSetId </summary>
        public uint SkillSetId { get; set; }
        
        /// <summary> アクティブ化の条件タイプ </summary>
        public App.AppCommon.EnemyActiveConditionType ActiveConditionType { get; set; }
       
        /// <summary> アクティブ化の条件値 </summary>
        public int ActiveConditionValue { get; set; }

        /// <summary> 非アクティブ化の条件タイプ </summary>
        public App.AppCommon.EnemyInactiveConditionType InactiveConditionType { get; set; }
       
        /// <summary> 非アクティブ化の条件値 </summary>
        public int InactiveConditionValue { get; set; }
        
        /// <summary> 飛行しているか </summary>
        public bool IsFlight { get; set; }
        
        /// <summary> イメージID </summary>
        public string ImageId { get; set; }
    }
}