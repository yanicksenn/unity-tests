using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

namespace Tests
{
    [Serializable]
    public class EmptyEvent : UnityEvent { }

    [Serializable]
    public class EventWithPayload : UnityEvent<string> { }
    
    public class MyBehaviour : MonoBehaviour
    {
        [SerializeField] 
        private EmptyEvent emptyEvent = new EmptyEvent();
        public EmptyEvent EmptyEvent => emptyEvent;
   
        [SerializeField] 
        private EventWithPayload eventWithPayload = new EventWithPayload();
        public EventWithPayload EventWithPayload => eventWithPayload;
    }
    
    public class Test
    {
        private GameObject _gameObject;
        private MyBehaviour _behaviour;
        private UnityEventListener _listener;
        
        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _behaviour = _gameObject.AddComponent<MyBehaviour>();
            _listener = new UnityEventListener("Event");
        }
        
        [Test]
        public void AssertInvocationOfUnityEvent1()
        {
            _behaviour.EmptyEvent.AddListener(_listener.Invoke);
            _behaviour.EmptyEvent.Invoke();
            _behaviour.EmptyEvent.Invoke();
            _behaviour.EmptyEvent.Invoke();
      
            _listener.AssertInvocation();
            _listener.AssertInvocations(3);
        }
   
        [Test]
        public void AssertInvocationOfUnityEvent2()
        {
            _behaviour.EventWithPayload.AddListener(_listener.Invoke);
            _behaviour.EventWithPayload.Invoke("Str1");
            _behaviour.EventWithPayload.Invoke("Str1");
            _behaviour.EventWithPayload.Invoke("Str2");
      
            _listener.AssertInvocation();
            _listener.AssertInvocations(3);
            _listener.AssertInvocationWithPayload("Str1");
            _listener.AssertInvocationsWithPayload("Str1", 2);
            _listener.AssertInvocationWithPayload("Str2");
            _listener.AssertInvocationsWithPayload("Str2", 1);
            _listener.AssertNoInvocationWithPayload("Str3");
        }
    }
}