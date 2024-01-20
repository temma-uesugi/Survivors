namespace App.Battle.EnemyBots.NodeObjects
{
    /// <summary>
    /// 行動対象タイプ
    /// </summary>
    public enum ActionTargetType
    {
        //ランダム
        Random,
        //一番近い
        Near,
        //一番HPが弱っている
        Weak,
        //倒せる
        Defeat,
        //倒せる
        Kill
    }
}