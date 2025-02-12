using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace OghiUnityTools.TestsExample.Runtime.Tests
{
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
}
