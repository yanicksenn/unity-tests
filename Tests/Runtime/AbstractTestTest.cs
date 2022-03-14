using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class AbstractTestTest : AbstractTest
    {
        [Test]
        public void AssertGameObjectsAreCleanedUp()
        {
            for (var i = 0; i < Random.Range(1, 100); i++)
            {
                var gameObject = new GameObject();
                gameObject.name = $"Instance {i}";
                gameObject.transform.position = Random.insideUnitSphere * Random.Range(-100, 100);
                gameObject.transform.localScale = new Vector3(
                    Random.Range(0.5f, 5f), 
                    Random.Range(0.5f, 5f), 
                    Random.Range(0.5f, 5f));
            }
        }
    }
}