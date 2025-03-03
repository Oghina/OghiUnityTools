using System.Collections.Generic;

namespace OghiUnityTools.PatternsExamples.AbilitySystem
{
    public interface IAbilitySystem
    {
        List<ICommand> Commands { get; }
        void Add(ICommand command);
        void Remove(ICommand command);
        void Swap(ICommand commandA, ICommand commandB);
        void Debug();
    }
}