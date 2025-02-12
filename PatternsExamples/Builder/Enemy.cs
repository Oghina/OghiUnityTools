using UnityEngine;

namespace OghiUnityTools.PatternsExamples.Builder
{
    public class Enemy : MonoBehaviour
    {
        public string Name;
        public WeaponStrategy WeaponStrategy;
        public DetectionStrategy DetectionStrategy;
        
        public class Builder
        {
            string name;
            WeaponStrategy weaponStrategy;
            DetectionStrategy detectionStrategy;
            
            public Builder WithName(string name)
            {
                this.name = name;
                return this;
            }

            public Builder WithWeaponStrategy(WeaponStrategy weaponStrategy)
            {
                this.weaponStrategy = weaponStrategy;
                return this;
            }

            public Builder WithDetectionStrategy(DetectionStrategy detectionStrategy)
            {
                this.detectionStrategy = detectionStrategy;
                return this;
            }

            public Enemy Build()
            {
                var enemy = new GameObject(name).AddComponent<Enemy>();
                enemy.Name = name;
                enemy.WeaponStrategy = weaponStrategy;
                enemy.DetectionStrategy = detectionStrategy;
                return enemy;
            }
            
        }
    }
}
