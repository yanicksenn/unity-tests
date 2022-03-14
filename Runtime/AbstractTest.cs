using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    /// <summary>
    /// Abstract test-case ensuring that all GameObjects are removed after each test.
    /// </summary>
    public abstract class AbstractTest
    {
        /// <summary>
        /// Removes all GameObjects in the scene.
        /// </summary>
        [TearDown]
        public void RemoveAllGameObjects()
        {
            FindAllGameObjectsInScene()
                .ToList()
                .ForEach(Object.DestroyImmediate);
            
            Assert.IsEmpty(FindAllGameObjectsInScene(), "No GameObjects should be found");
        }

        private static GameObject[] FindAllGameObjectsInScene() => Object.FindObjectsOfType<GameObject>();
    }
}