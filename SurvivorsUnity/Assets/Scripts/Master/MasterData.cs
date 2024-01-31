using App.MD;
using UnityEngine;

namespace App.Master
{
    /// <summary>
    /// マスターデータFacade
    /// </summary>
    public static class MasterData
    {
        private static MemoryDatabase _facade;
        public static MemoryDatabase Facade => _facade ??= LoadMasterData();

        /// <summary>
        /// MasterDataをLoad
        /// </summary>
        private static MemoryDatabase LoadMasterData()
        {
            return new MemoryDatabase((Resources.Load("master-data") as TextAsset).bytes);
        } 
    }
}