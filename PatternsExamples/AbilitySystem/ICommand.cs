namespace OghiUnityTools.PatternsExamples.AbilitySystem
{
    public interface ICommand
    {
        string Name { get; }
        void Execute();
    }
}