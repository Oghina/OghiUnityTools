# Unity dialog system

## Description
Easy dialog system using the EventBus. All dialogs will appear in the order they were invoked no mather what script invoked them.

## Example usage

Drag the VR_Canvas prefab (containing the DialogManager.cs) in the scene. 

Example usage 

```csharp 
public class ExampleMonoBehaviour : MonoBehaviour
    {        
        private void Start()
        {
           ShowDialogs();
        }
        
        private void ShowDialogs()
        {
            EventBus<DialogRequest>.Raise(new DialogRequest(
                "Info", "This is a info dialog with one button.", DialogType.Info,
                () => Debug.Log("Info button clicked")
                ));
            
            EventBus<DialogRequest>.Raise(new DialogRequest(
                "Yes no dialog", "This is a Yes or No dialog with two buttons.", DialogType.YesNo,
                () => Debug.Log("Yes button clicked"),
                () => Debug.Log("No button clicked")
            ));
            
            EventBus<DialogRequest>.Raise(new DialogRequest(
                "Yes no cancel dialog", "This is a Yes, No or Cancel dialog with three buttons.", DialogType.YesNoCancel,
                () => Debug.Log("Yes button clicked"),
                () => Debug.Log("No button clicked"),
                () => Debug.Log("Cancel button clicked")
            ));
        } 
```

# Unit Testing Examples
### Using Unity Test Framework and [NSubstitute]("https://github.com/Thundernerd/Unity3D-NSubstitute")

Editor test
```csharp 
public class TestExample
    {
        [Test]
        public void TestExampleSimplePasses()
        {
            // 1st level Is/Has/Does/Contains
            // 2nd level All/Not/Some/Exactly
            // Or/And/Not
            // Is.Unique / Is.Ordered
            // Asset.IsTrue

            string username = "User1234";
            
            Assert.That(username, Does.StartWith("U"));

            var list = new List<int> { 1, 2, 3, 4, 5 };
            Assert.That(list, Contains.Item(3));
            Assert.That(list, Is.All.Positive);
            Assert.That(list, Has.Exactly(2).LessThan(3));
            Assert.That(list, Is.Ordered);
            Assert.That(list, Is.Unique);
            
            //check if three items in this list that are odd numbers
            Assert.That(list, Has.Exactly(3).Matches<int>(x => x % 2 == 0));    
        }
}
```

## Credits to Git-Amend for the following:

# Unity C# EventBus 

## Description
This EventBus system provides a way to create decoupled architectures in Unity projects. It allows communication between different parts of an application without requiring direct references.

## Code Structure
This EventBus system contains several C# classes residing in the Scripts\EventBus directory:

1. `EventBus.cs` - Main EventBus class that provides static functions for registering, deregistering, and triggering custom events.

2. `EventBinding.cs` - IEventBinding interface and class definition for EventBinding, which is used to bind functions to events.

3. `Events.cs` - IEvent interface and sample code, which shows how to define custom events.

4. `PredefinedAssemblyUtil.cs` - Utility class for locating assemblies and finding types within them. See [Unity Documentation](https://docs.unity3d.com/Manual/ScriptCompileOrderFolders.html).

5. `EventBusUtil.cs` - Static initialization methods and additional utilities used for EventBus.


## Example Usage

The usage generally works like:

```csharp 
public struct PlayerEvent : IEvent {
    public int health;
    public int mana;
}

EventBinding<PlayerEvent> playerEventBinding;

void OnEnable() {    
    playerEventBinding = new EventBinding<PlayerEvent>(HandlePlayerEvent);
    EventBus<PlayerEvent>.Register(playerEventBinding);

    // Can Add or Remove Actions to/from the EventBinding
}

void OnDisable() {
    EventBus<PlayerEvent>.Deregister(playerEventBinding);
}

void Start() {
    EventBus<PlayerEvent>.Raise(new PlayerEvent {
        health = healthComponent.GetHealth(),
        mana = manaComponent.GetMana()
    });    
}

void HandlePlayerEvent(PlayerEvent playerEvent) {
    Debug.Log($"Player event received! Health: {playerEvent.health}, Mana: {playerEvent.mana}");
}
```

# Unity C# Dependency Injection Lite Usage Guide

Unity Dependency Injection Lite is a lightweight, easy-to-use dependency injection system for Unity. It leverages Unity's `MonoBehaviour` to automatically inject dependencies into your classes. This system supports field, method and property injections, and allows for easy setup of providers.

## Features

- **Automatic Dependency Injection**: Automatically injects dependencies into your Unity MonoBehaviours.
- **Custom Attributes**: Use `[Inject]` and `[Provide]` attributes to denote injectable members and providers.
- **Method Injection**: Supports method injection for more complex and multiple initialization.
- **Field Injection**: Simplify your code with direct field injection.
- **Property Injection**: Supports property injection.

## Usage
### Defining Injectable Fields, Methods and Properties

Use the `[Inject]` attribute on fields, methods, or properties to mark them as targets for injection.

```csharp
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
```

### Creating Providers

Implement IDependencyProvider and use the `[Provide]` attribute on methods to define how dependencies are created.

```csharp
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
```

### Example of Using Multiple Dependencies

```csharp
using DependencyInjection;
using UnityEngine;

public class ClassB : MonoBehaviour {
    [Inject] IServiceA serviceA;
    
    IServiceB serviceB;
    IFactoryA factoryA;
        
    [Inject] // Method injection supports multiple dependencies
    public void Init(IFactoryA factoryA, IServiceB serviceB) {
        this.factoryA = factoryA;
        this.serviceB = serviceB;
    }

    void Start() {
        serviceA.Initialize("ServiceA initialized from ClassB");
        serviceB.Initialize("ServiceB initialized from ClassB");
        factoryA.CreateServiceA().Initialize("ServiceA initialized from FactoryA");
    }
}
