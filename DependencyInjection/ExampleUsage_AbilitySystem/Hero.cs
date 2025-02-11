using System.Collections.Generic;
using UnityEngine;

namespace OghiUnityTools.DependencyInjection.ExampleUsage
{
    public class Hero : MonoBehaviour, IEntity
    {
        [SerializeField] private List<AbilityData> abilities;
        
        private IAbilitySystem abilitySystem;
        
        [Inject]
        private AbilitySystemFactory abilitySystemFactory;

        private void Start()
        {
            abilitySystem = abilitySystemFactory.CreateAbilitySystem(this, abilities);
            abilitySystem.Debug();
        }
    }
}