namespace OghiUnityTools.DependencyInjection.ExampleUsage
{
    public interface ICommand
    {
        string Name { get; }
        void Execute();
    }
}