using System.Collections.Generic;
using OghiUnityTools.DependencyInjection;
using UnityEngine;

namespace OghiUnityTools.PatternsExamples.AbilitySystem
{
    public class AbilitySystemFactory : MonoBehaviour, IDependencyProvider
    {
        [Provide]
        public AbilitySystemFactory ProvideAbilitySystemFactory()
        {
            return this;
        }
        
        public IAbilitySystem CreateAbilitySystem(IEntity entity, List<AbilityData> abilities)
        {
            return new AbilitySystem.Builder(entity).WithAbilities(abilities).Build();
        }
    }
}