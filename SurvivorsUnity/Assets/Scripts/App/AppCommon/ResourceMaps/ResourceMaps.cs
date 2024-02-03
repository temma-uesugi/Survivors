using App.AppCommon.ResourceMaps.DBs;
using UnityEngine;

namespace App.AppCommon.ResourceMaps
{
    /// <summary>
    /// リソースMap
    /// </summary>
    public class ResourceMaps : ScriptableObject
    {
        private static ResourceMaps _instance;
        public static ResourceMaps Instance => _instance ??= Resources.Load<ResourceMaps>("ResourceMaps");

        [field: SerializeField] public WindIconMap WindIcon { get; private set; }
        [field: SerializeField] public WeatherIconMap WeatherIcon { get; private set; }
        [field: SerializeField] public CellPrefabMap CellPrefab { get; private set; }
    }
}