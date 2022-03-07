using NUnit.Framework;
using AssertionException = UnityEngine.Assertions.AssertionException;

namespace Tests
{
    public class UnityEventListenerTest
    {
        private const string TestName = "Test-Name";
        private const string TestPayloadA = "A";
        private const string TestPayloadB = "B";
        
        private UnityEventListener _listener;

        [SetUp]
        public void SetUp()
        {
            _listener = new UnityEventListener(TestName);
        }

        [Test]
        public void AssertNameIsSet()
        {
            Assert.AreEqual(TestName, _listener.Name);
        }

        [Test]
        public void AssertInvokeIncreasesInvocations()
        {
            _listener.Invoke();
            Assert.AreEqual(1, _listener.Invocations);
        }
        
        [Test]
        public void AssertInvokeWithObjectIncreasesInvocations()
        {
            _listener.Invoke(TestPayloadA);
            _listener.Invoke(TestPayloadB);
            Assert.AreEqual(2, _listener.Invocations);
        }
        
        [Test]
        public void AssertInvocationsAreSummedUp()
        {
            _listener.Invoke();
            _listener.Invoke(TestPayloadA);
            _listener.Invoke(TestPayloadB);
            Assert.AreEqual(3, _listener.Invocations);
        }

        [Test]
        public void AssertInvocation()
        {
            InvokeRandomly(_listener);
            Assert.DoesNotThrow(() => _listener.AssertInvocation());
        }

        [Test]
        public void AssertInvocationFails()
        {
            Assert.Throws<AssertionException>(() => _listener.AssertInvocation());
        }

        [Test]
        public void AssertNoInvocation()
        {
            Assert.DoesNotThrow(() => _listener.AssertNoInvocation());
        }

        [Test]
        public void AssertNoInvocationFails()
        {
            InvokeRandomly(_listener);
            Assert.Throws<AssertionException>(() => _listener.AssertNoInvocation());
        }

        [Test]
        public void AssertInvocations()
        {
            var invocations = InvokeRandomly(_listener);
            Assert.DoesNotThrow(() => _listener.AssertInvocations(invocations));
        }

        [Test]
        public void AssertInvocationsFails()
        {
            var invocations = InvokeRandomly(_listener) + 1;
            Assert.Throws<AssertionException>(() => _listener.AssertInvocations(invocations));
        }

        [Test]
        public void AssertInvocationWithPayload()
        {
            InvokeRandomlyWithObject(_listener, TestPayloadA);
            InvokeRandomlyWithObject(_listener, TestPayloadB);
            Assert.DoesNotThrow(() => _listener.AssertInvocationWithPayload(TestPayloadA));
            Assert.DoesNotThrow(() => _listener.AssertInvocationWithPayload(TestPayloadB));
        }

        [Test]
        public void AssertInvocationWithPayloadFails()
        {
            Assert.Throws<AssertionException>(() => _listener.AssertInvocationWithPayload(TestPayloadA));
            Assert.Throws<AssertionException>(() => _listener.AssertInvocationWithPayload(TestPayloadB));
        }

        [Test]
        public void AssertNoInvocationWithPayload()
        {
            InvokeRandomlyWithObject(_listener, TestPayloadA);
            InvokeRandomlyWithObject(_listener, TestPayloadB);
            Assert.Throws<AssertionException>(() => _listener.AssertNoInvocationWithPayload(TestPayloadA));
            Assert.Throws<AssertionException>(() => _listener.AssertNoInvocationWithPayload(TestPayloadB));
        }

        [Test]
        public void AssertNoInvocationWithPayloadFails()
        {
            Assert.DoesNotThrow(() => _listener.AssertNoInvocationWithPayload(TestPayloadA));
            Assert.DoesNotThrow(() => _listener.AssertNoInvocationWithPayload(TestPayloadB));
        }

        [Test]
        public void AssertInvocationsWithPayload()
        {
            var invocationsA = InvokeRandomlyWithObject(_listener, TestPayloadA);
            var invocationsB = InvokeRandomlyWithObject(_listener, TestPayloadB);
            Assert.DoesNotThrow(() => _listener.AssertInvocationsWithPayload(TestPayloadA, invocationsA));
            Assert.DoesNotThrow(() => _listener.AssertInvocationsWithPayload(TestPayloadB, invocationsB));
        }

        [Test]
        public void AssertInvocationsWithPayloadFails()
        {
            var invocationsA = InvokeRandomlyWithObject(_listener, TestPayloadA) + 1;
            var invocationsB = InvokeRandomlyWithObject(_listener, TestPayloadB) + 1;
            Assert.Throws<AssertionException>(() => _listener.AssertInvocationsWithPayload(TestPayloadA, invocationsA));
            Assert.Throws<AssertionException>(() => _listener.AssertInvocationsWithPayload(TestPayloadB, invocationsB));
        }

        private static int InvokeRandomly(UnityEventListener listener)
        {
            var randomInt = UnityEngine.Random.Range(1, 100);
            for (var i = 0; i < randomInt; i++)
                listener.Invoke();

            return randomInt;
        }
        
        private static int InvokeRandomlyWithObject(UnityEventListener listener, string obj)
        {
            var randomInt = UnityEngine.Random.Range(1, 100);
            for (var i = 0; i < randomInt; i++)
                listener.Invoke(obj);

            return randomInt;
        }
    }
}