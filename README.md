# Tests

Unity3d package containing test utilities.

Feedback is welcome.

## Plug and Play
1. Open "Package Manager"
2. Choose "Add package from git URL..."
3. Use the HTTPS URL of this repository:
   `https://github.com/yanicksenn/unity-tests.git#1.0.0`
4. Click "Add"

## Usage
- [UnityEventListener](#user-content-unityeventlistener)
  - [Example](#user-content-example)
  - [Assert-Methods](#user-content-assert-methods)

## UnityEventListener

### Example
Assumed a behaviour with `UnityEvents` should be tested.
`MyBehaviour` contains a plain `UnityEvent` and a `UnityEvent` with string payload.

```c#
public class MyBehaviour : MonoBehaviour
{
   [field: SerializeField] 
   public UnityEvent UnityEvent1 { get; } = new UnityEvent();
   
   [field: SerializeField] 
   public UnityEvent<string> UnityEvent2 { get; } = new UnityEvent<string>();
}
```

The associated test can create and assign the `UnityEventListener` to the `UnityEvent`.

```c#
public class MyBehaviourTest 
{
   [Test]
   public void AssertInvocationOfUnityEvent1()
   {
      var gameObject = new GameObject();
      var behaviour = gameObject.AddComponent<MyBehaviour>();
      var listener = new UnityEventListener("UnityEvent1");
      behaviour.UnityEvent1.AddListener(listener.Invoke);
      
      behaviour.UnityEvent1.Invoke();
      behaviour.UnityEvent1.Invoke();
      behaviour.UnityEvent1.Invoke();
      
      listener.AssertInvocation();
      listener.AssertInvocations(3);
   }
   
   [Test]
   public void AssertInvocationOfUnityEvent2()
   {
      var gameObject = new GameObject();
      var behaviour = gameObject.AddComponent<MyBehaviour>();
      var listener = new UnityEventListener("UnityEvent2");
      behaviour.UnityEvent2.AddListener(listener.Invoke);
      
      behaviour.UnityEvent2.Invoke("Str1");
      behaviour.UnityEvent2.Invoke("Str1");
      behaviour.UnityEvent2.Invoke("Str2");
      
      listener.AssertInvocation();
      listener.AssertInvocations(3);
      listener.AssertInvocationWithPayload("Str1");
      listener.AssertInvocationsWithPayload("Str1", 2);
      listener.AssertInvocationWithPayload("Str2");
      listener.AssertInvocationsWithPayload("Str2", 1);
      listener.AssertNoInvocationWithPayload("Str3");
   }
}
```

Add the listeners `Invoke` method to the `UnityEvent`.

```c#
var listener = new UnityEventListener("Event-Name");
behaviour.UnityEvent1.AddListener(listener.Invoke);
behaviour.UnityEvent2.AddListener(listener.Invoke);
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
