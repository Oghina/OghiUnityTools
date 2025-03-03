# Excel Importer

## Description
Imports data from excel file and converts it into chosen scriptable objects

### Usage
```csharp
public class ExcelImporterMonobehaviourExample : MonoBehaviour
    {
        [SerializeField] private ScriptableObject templateScriptableObject;

        private ExcelImporter<ExcelScriptableObjectExample> excelImporter; 
        
        private void Start()
        {
            excelImporter = new ExcelImporter<ExcelScriptableObjectExample>();
            
            // Set paths, the default ones are also these from bellow
            excelImporter.SetExcelFilePath("Assets/ExcelImporter/ItemDetails.xlsx");
            excelImporter.SetOutputPath("Assets/ExcelImporter/ScriptableObjects");
                
            var list = excelImporter.ImportItemsFromExcel(templateScriptableObject);
            foreach (var scriptable in list)
            {
                Debug.Log($"{scriptable.ItemName} with description: {scriptable.ItemDescription}");
            }
        }
    }
    
// Example ScriptableObject
[CreateAssetMenu(fileName = "ExcelScriptableObjectExample", menuName = "New ExcelScriptableObjectExample", order = 1)]
    public class ExcelScriptableObjectExample : ScriptableObject
    {
        public string ItemName;
        public string ItemDescription;
    }
```

# Unit Testing Examples
### Using Unity Test Framework and [NSubstitute](https://github.com/Thundernerd/Unity3D-NSubstitute)

### Editor test
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
### Runtime test
```csharp
public class RuntimeTestExample
    {
        [Test]
        public void VerifyApplicationPlaying()
        {
            Assert.That(Application.isPlaying, Is.True);
        }

        [Test]
        [LoadScene("Assets/Scenes/SampleScene.unity")]
        public void VerifyScene()
        {
            var go = GameObject.Find("Injector");
            Assert.That(go, Is.Not.Null, "Injector not found in {0}", SceneManager.GetActiveScene().name);
        }
    }
    
    
    // This is mandatory for VerifyScene() test because it won't create a dummy scene, it will test the current one
    public class LoadSceneAttribute : NUnitAttribute, IOuterUnityTestAction {
        readonly string scene;
    
        public LoadSceneAttribute(string scene) => this.scene = scene;

        public IEnumerator BeforeTest(ITest test) {
            Debug.Assert(scene.EndsWith(".unity"), "Scene must end with .unity");
            yield return EditorSceneManager.LoadSceneInPlayMode(scene, new LoadSceneParameters(LoadSceneMode.Single));
        }

        public IEnumerator AfterTest(ITest test) {
            yield return null;
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
