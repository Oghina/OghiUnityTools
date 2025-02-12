using System.Collections.Generic;
using NUnit.Framework;

namespace OghiUnityTools.TestsExample.Editor.Tests
{
    public class EditorTestExample
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
            Assert.That(list, Has.Exactly(2).Matches<int>(x => x % 2 == 0));
        }
        
        [Test]
        public void TestExampleSimplePasses2()
        {
            // 1st level Is/Has/Does/Contains
            // 2nd level All/Not/Some/Exactly
            // Or/And/Not
            // Is.Unique / Is.Ordered
            // Asset.IsTrue

            string username = "User1234";
            Assert.That(username, Does.EndWith("4"));
        }
    }
}
