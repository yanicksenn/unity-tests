using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tests
{
    /// <summary>
    /// Builder of GameObjects for unit-tests. Allowing to
    /// test the behaviour of the Awake lifecycle hook.
    /// </summary>
    public class GameObjectBuilder
    {
        private Action<GameObject> builder;

        public GameObjectBuilder(Action<GameObject> builder = null)
        {
            this.builder = builder;
        }

        /// <summary>
        /// Builds a GameObject based on the builder.
        /// </summary>
        /// <returns>GameObject</returns>
        public GameObject Build()
        {
            var gameObject = new GameObject();
            builder?.Invoke(gameObject);
            return Object.Instantiate(gameObject);
        }
    }
}