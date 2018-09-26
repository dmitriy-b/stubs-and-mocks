# How to use stubbing and mocking in unit testing

Stubbing uses when we create some logic but don't want to spend time on every method needed right now. Stubs methods just returns a simple but a valid result. So everything compiles with it and IDE's auto-complete knows about the methods we are planning to use.

Another way to use stubs is in mocking when testing. We create stub methods and set up it with dependency injection. After that we ensure that the code does the right thing with them. So we don't need to spin up a database or other environment-sensitive part of our code just to run tests.

Also, we can use mocking to emulate 3rd party services our code communicate with. In such way we can make sure that our app works with different configuration of that services. 

For example, we need to get username from database (**GetCurrentUser** method) and then use the result in other parts of our code (**ShowUsername** method). Imagine that GetUserName method is not ready to use yet. So, to tests our ShowUsername method, that depends on GetCurrentUser, we could create a stub for it.  

Using stubs we can cover GetUserName with unit tests. Additional way is replace _myDatabse_ using mock of database service and dont connect to a real database too.

For example the following code shows how to use stubs and mocks in unit testing:

```csharp
public class User
{
  public string Name {get; private set; }
  
  public User(string name)
  {
    Name = name;
  }
}

public class UserService 
{

  private readonly DatabaseService _myDatabase;
  
  public UserService(DatabaseService database) 
  // DatabaseService will be replaced on our mock using dependency injection 
  { 
    this._myDatabase = database
  }

//And methods to interact with it:

  public User GetCurrentUser()
  {
    return _myDatabse.GetCurrentUser(); // Use mock of _myDatabse or real database
    // Comment unused return
    return new User("Dmitriy"); // Use a simple stub
  }

  public string GetUserName()
  {
    return GetCurrentUser().Name; // returns Dmitriy for stub or our data if we use mocking
  }
 
}
```
The full example of using mocking in testing you can show in this repository.

## Simple app to get weather for chosen location

You can show how to use mocking with a simple example.
I have created an app that get the weather status using 3rd party API service (openweathermap.org) and unit tests for it. 
This sample written using asp .net core as Web Framework and xUnit for Unit test Framework.

### Usage
To run the tests we need to download .net core runtime https://www.microsoft.com/net/download 

After that, we could build the solutin and run the test:
```
dotnet build
dotnet test SampleUnitTests
```
To run app:
```
dotnet run --project SampleApplication
```
Open http://localhost:5000/api/weather/kiev in any browser to see the current weather in kiev (you can replace kiev with your city).

### Unit tests
So, I create 2 unit tests:

*CheckHttpErrorTestAsync* - in this tests we will send GET response on unexisting method to check our app general 404 error handling.

*CheckWeatherNotFoundTestAsync* - in this tests we will update our 3rd party service URL with dependency injection and prove that the application handles http 404 errors as expected. So, this is a mocking.

See WeatherController.Get(string id) method to check weather service implementation.
