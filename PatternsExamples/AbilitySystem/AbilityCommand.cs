using UnityEngine;

namespace OghiUnityTools.PatternsExamples.AbilitySystem
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