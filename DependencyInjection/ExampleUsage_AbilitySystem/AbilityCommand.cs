using UnityEngine;

namespace OghiUnityTools.DependencyInjection.ExampleUsage
{
    public class AbilityCommand : ICommand
    {
        public string Name { get; }

        public AbilityCommand(AbilityData ability)
        {
            Name = ability.Name;
        }
        
        public void Execute()
        {
            Debug.Log($"Executing command: {Name}");
        }
    }
}