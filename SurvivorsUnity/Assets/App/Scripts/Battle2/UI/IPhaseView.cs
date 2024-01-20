namespace App.Battle2.UI
{
    /// <summary>
    /// PhaseによるView
    /// </summary>
    public interface IPhaseView
    {
        void PhaseStart();
        void PhaseEnd();
    }
    
    /// <summary>
    /// プレイヤーPhaseによるView
    /// </summary>
    public interface IPlayerPhaseView : IPhaseView { }
    
    /// <summary>
    /// 敵PhaseによるView
    /// </summary>
    public interface IEnemyPhaseView : IPhaseView { }
}