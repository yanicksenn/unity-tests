using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace Tests
{
    /// <summary>
    /// Test-Listener for UnityEvents.
    /// </summary>
    public class UnityEventListener
    {
        private readonly string _name;
        private readonly Dictionary<object, int> _invocationsWithPayload;
        private int _invocations = 0;
        private int _invocationsWithNull = 0;
        
        public string Name => _name;
        public int Invocations => _invocations 
          + _invocationsWithNull 
          + _invocationsWithPayload.Values.Sum(x => x);
        
        public UnityEventListener(string name)
        {
            _name = name;
            _invocationsWithPayload = new Dictionary<object, int>();
        }

        /// <summary>
        /// Count an invocation.
        /// </summary>
        public void Invoke()
        {
            _invocations++;
        }

        /// <summary>
        /// Count an invocation with payload.
        /// </summary>
        /// <param name="payload">Payload</param>
        public void Invoke<T>(T payload)
        {
            if (payload == null)
            {
                _invocationsWithNull++;
            }
            else
            {
                _invocationsWithPayload.TryGetValue(payload, out var invocations);
                _invocationsWithPayload[payload] = invocations + 1;
            }
        }

        /// <summary>
        /// Asserts that at least one invocation took place.
        /// </summary>
        public void AssertInvocation()
        {
            var actualInvocations = Invocations;
            Assert.IsTrue(actualInvocations > 0, $"Event {_name} should have been invoked at least once");
        }
        
        /// <summary>
        /// Asserts that no invocation took place.
        /// </summary>
        public void AssertNoInvocation()
        {
            var actualInvocations = Invocations;
            Assert.IsTrue(actualInvocations == 0, $"Event {_name} should not have been invoked");
        }

        /// <summary>
        /// Asserts that exactly the amount of provided invocations took place.
        /// </summary>
        /// <param name="expectedInvocations">Amount of invocations</param>
        public void AssertInvocations(int expectedInvocations)
        {
            var actualInvocations = Invocations;
            Assert.AreEqual(expectedInvocations, actualInvocations, $"Event {_name} should have been invoked {expectedInvocations} times but was {actualInvocations}");
        }

        /// <summary>
        /// Asserts that at least one invocation with the provided payload took place. 
        /// </summary>
        /// <param name="payload">Payload</param>
        public void AssertInvocationWithPayload<T>(T payload)
        {
            var actualInvocations = GetInvocationsForPayload(payload);
            Assert.IsTrue(actualInvocations > 0, $"Event {_name} for {payload} should have been invoked at least once");
        }
        
        /// <summary>
        /// Asserts that no invocations with the provided payload took place.
        /// </summary>
        /// <param name="payload">Payload</param>
        public void AssertNoInvocationWithPayload<T>(T payload)
        {
            var actualInvocations = GetInvocationsForPayload(payload);
            Assert.IsTrue(actualInvocations == 0, $"Event {_name} for {payload} should not have been invoked");
        }
        
        /// <summary>
        /// Asserts that exactly the amount of provided invocations with the provided payload took place.
        /// </summary>
        /// <param name="payload">Payload</param>
        /// <param name="expectedInvocations">Amount of invocations</param>
        public void AssertInvocationsWithPayload<T>(T payload, int expectedInvocations)
        {
            var actualInvocations = GetInvocationsForPayload(payload);
            Assert.AreEqual(expectedInvocations, actualInvocations, $"Event {_name} for {payload} should have been invoked {expectedInvocations} times but was {actualInvocations}");
        }

        private int GetInvocationsForPayload<T>(T payload)
        {
            if (payload == null)
                return _invocationsWithNull;
            
            _invocationsWithPayload.TryGetValue(payload, out var actualInvocations);
            return actualInvocations;
        }
    }
}