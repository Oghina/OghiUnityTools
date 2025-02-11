using UnityEngine;

namespace OghiUnityTools.DependencyInjection.ExampleUsage
{
    [CreateAssetMenu(fileName = "AbilityData", menuName = "OghiUnityTools/Create New Ability Data")]
    public class AbilityData : ScriptableObject
    {
        public string Name;
    }
}