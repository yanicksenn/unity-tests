# Tests

Unity3d package containing test utilities.

Feedback is welcome.

## Plug and Play
1. Open "Package Manager"
2. Choose "Add package from git URL..."
3. Use the HTTPS URL of this repository:
   `https://github.com/yanicksenn/unity-tests.git`
4. Click "Add"

## Usage
- [AbstractTest](#user-content-abstracttest)
- [GameObjectBuilder](#user-content-gameobjectbuilder)
- [UnityEventListener](#user-content-unityeventlistener)
  - [Example](#user-content-example)
  - [Assert-Methods](#user-content-assert-methods)

## AbstractTest

Because `GameObjects` are not cleaned up in between unity tests you'll have to take care of that by yourself.

Instead of manually adding a cleanup method and invoking it after every test you can simply inherit your test-class from `AbstractTest`.

The following test class automatically cleans up all randomly generated `GameObjects` ensuring independent test cases.

```c#
public class MyTests : AbstractTest 
{
    [Test]
    public void AssertCreationOfGameObjects()
    {
        for (var i = 0; i < Random.Range(1, 100); i++)
            new GameObject { name = $"Instance {i}" };
    }
}
```

## GameObjectBuilder

Sometimes you want to test that the right thing happens during the `Awake` hook of a `GameObject`.

TODO

## UnityEventListener

### Example
Assume a behaviour with `UnityEvents` should be tested.

We have two kinds of events, an empty event and an event with an int as payload.

```c#
[Serializable]
public class EmptyEvent : UnityEvent { }

[Serializable]
public class EventWithPayload : UnityEvent<string> { }
```

`MyBehaviour` contains an instance of both events.

```c#
public class MyBehaviour : MonoBehaviour
{
    [SerializeField] 
    private EmptyEvent emptyEvent = new EmptyEvent();
    public EmptyEvent EmptyEvent => emptyEvent;

    [SerializeField] 
    private EventWithPayload eventWithPayload = new EventWithPayload();
    public EventWithPayload EventWithPayload => eventWithPayload;
}
```

The associated test creates a `GameObject` and assigns a `MyBehaviour` component.
In each test case an instance of the `UnityEventListener` is assigned to the `UnityEvents` of `MyBehaviour`.

```c#
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
    public void AssertInvocationOfEmptyEvent()
    {
        _behaviour.EmptyEvent.AddListener(_listener.Invoke);
        _behaviour.EmptyEvent.Invoke();
        _behaviour.EmptyEvent.Invoke();
        _behaviour.EmptyEvent.Invoke();
  
        _listener.AssertInvocation();
        _listener.AssertInvocations(3);
    }

    [Test]
    public void AssertInvocationOfEventWithPayload()
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
```

### Assert-Methods

To assert a variety of properties of the `UnityEvent` use the listeners built-in assert-methods.

| Method                                                                  | Description                                                                                    |
|-------------------------------------------------------------------------|------------------------------------------------------------------------------------------------|
| `AssertInvocation()`                                                    | Asserts that at least one invocation took place.                                               |
| `AssertNoInvocation()`                                                  | Asserts that no invocation took place.                                                         |
| `AssertInvocations(int expectedInvocations)`                            | Asserts that exactly the amount of provided invocations took place.                            |
| `AssertInvocationWithPayload(object payload)`                           | Asserts that at least one invocation with the provided payload took place.                     |
| `AssertNoInvocationWithPayload(object payload)`                         | Asserts that no invocations with the provided payload took place.                              |
| `AssertInvocationsWithPayload(object payload, int expectedInvocations)` | Asserts that exactly the amount of provided invocations with the provided payload took place.  |
