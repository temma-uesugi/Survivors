using MasterMemory;
using MessagePack;

namespace App.Example
{
    /// <summary>
    /// ステージ
    /// </summary>
    [MemoryTable("Stage"), MessagePackObject(true)]
    public class StageMaster
    {
        public StageMaster(string id, string name, int difficulty, string enemyGroupId, int exp, string resourceId)
        {
            Id = id;
            Name = name;
            Difficulty = difficulty;
            EnemyGroupId = enemyGroupId;
            Exp = exp;
            ResourceId = resourceId;
        }

        // プライマリキーにはPrimaryKeyアトリビュートをつける
        [PrimaryKey] public string Id { get; }

        public string Name { get; }

        public int Difficulty { get; }

        public string EnemyGroupId { get; }

        public int Exp { get; }

        public string ResourceId { get; }
    }
}