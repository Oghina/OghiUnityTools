using System.Collections.Generic;
using OghiUnityTools.ExtensionMethods;

namespace OghiUnityTools.PatternsExamples.AbilitySystem
{
    public class AbilitySystem : IAbilitySystem
    {
        private AbilitySystem() {}
        
        public class Builder
        {
            private IEntity entity;
            private List<AbilityData> abilities;

            public Builder(IEntity entity)
            {
                this.entity = entity;
            }

            public Builder WithAbilities(List<AbilityData> abilities)
            {
                this.abilities = abilities;
                return this;
            }

            public IAbilitySystem Build()
            {
                var abilitySystem = new AbilitySystem();
                foreach (var ability in abilities)
                {
                    abilitySystem.Add(new AbilityCommand(ability));
                }
                return abilitySystem;
            }
        }

        public List<ICommand> Commands { get; } = new();
        public void Add(ICommand command)
        {
            Commands.Add(command);
        }

        public void Remove(ICommand command)
        {
            Commands.Remove(command);
        }

        public void Swap(ICommand commandA, ICommand commandB)
        {
            Commands.Swap(Commands.IndexOf(commandA), Commands.IndexOf(commandB));
        }

        public void Debug()
        {
            if (Commands.IsNullOrEmpty())
            {
                UnityEngine.Debug.Log("No Commands found to execute");
                return;
            }
            
            Commands.ForEach(command => command.Execute());
        }
    }
}