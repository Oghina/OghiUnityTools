using System.Collections.Generic;

namespace OghiUnityTools.DependencyInjection.ExampleUsage
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