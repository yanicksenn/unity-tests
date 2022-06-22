using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class GameObjectBuilderTest : AbstractTest
    {
        [Test]
        public void AssertBuilderCreatesDifferentGameObjects()
        {
            var builder = new GameObjectBuilder();
            var gameObject1 = builder.Build();
            var gameObject2 = builder.Build();
            
            Assert.AreNotEqual(gameObject1, gameObject2);
        }
        
        [Test]
        public void AssertBuilderCreatesGameObjectBasedOnBuilder()
        {
            var desiredInt = 17;
            var builder = new GameObjectBuilder(g =>
            {
                var testBehaviour = g.AddComponent<TestBehaviour>();
                testBehaviour.DesiredInt = desiredInt;
            });
            
            
            var gameObject = builder.Build();
            var testBehaviour = gameObject.GetComponent<TestBehaviour>();
            Assert.AreEqual(desiredInt, testBehaviour.DesiredInt);
            Assert.AreEqual(desiredInt, testBehaviour.IntAfterAwake);
        }
        
        [Test]
        public void AssertNotUsingBuilderHasDefaultValueOnAwake()
        {
            var desiredInt = 17;
            var gameObject = new GameObject();
            var testBehaviour = gameObject.AddComponent<TestBehaviour>();
            testBehaviour.DesiredInt = desiredInt;
            
            Assert.AreEqual(desiredInt, testBehaviour.DesiredInt);
            Assert.AreEqual(0, testBehaviour.IntAfterAwake);
        }
        
        public class TestBehaviour : MonoBehaviour
        {
            [SerializeField]
            private int desiredInt;
            public int DesiredInt
            {
                get => desiredInt;
                set => desiredInt = value;
            }

            private int intAfterAwake;
            public int IntAfterAwake
            {
                get => intAfterAwake;
                set => intAfterAwake = value;
            }

            private void Awake()
            {
                IntAfterAwake = DesiredInt;
            }
        }
    }
}