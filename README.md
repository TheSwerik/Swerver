# Swerver
A Networking Library primarely for games but can also be used for other stuff.

# How to use
### Starting
Inherit from `ServerLibrary.Server.GameLogic` and write your Game-Logic in the `Update` Method

### Client-Server Communication
#### Server
For Server sending, write methods that look like ServerLibrary.Server.ServerSend Welcome and UdpTest.
For Server receiving, add a Method to the Servers PacketHandlers. Method Examples are ServerLibrary.Server.ServerHandler WelcomeReceived and UdpTestReceived.
#### Client
* Inherit from Udp and from ClientTcp and override `ExecuteOnMainThread` so that the Action executes on your main Thread. (examples below)
For Client sending, write methods that look like ServerLibrary.Server.ServerSend Welcome and UdpTest.
For Client receiving, add a Method to the Servers PacketHandlers. Method Examples are ServerLibrary.Server.ServerHandler WelcomeReceived and UdpTestReceived.
### Examples
#### ExecuteOnMainThread
For Console App, use the `Client.ThreadManager` class:
```c#
protected override void ExecuteOnMainThread(Action action)
{
    ThreadManager.ExecuteOnMainThread(action);
}
```
For WPF:
```c#
protected override void ExecuteOnMainThread(Action action)
{
    Application.Current.Dispatcher.Invoke(action);
}
```
# Credits
I followed [this tutorial](https://www.youtube.com/playlist?list=PLXkn83W0QkfnqsK8I0RAz5AbUxfg3bOQ5) for most of the code.
