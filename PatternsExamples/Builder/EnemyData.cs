using UnityEngine;

namespace OghiUnityTools.PatternsExamples.Builder
{
    [CreateAssetMenu(menuName = "OghiUnityTools/Patterns Examples/Builder/EnemyData", fileName = "Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public string Name;

        public WeaponStrategy WeaponStrategy;
        
        public DetectionStrategy DetectionStrategy;
        
        // Other EnemyData properties .. can be anything
    }
}
