# Swerver  ![Build status](https://ci.appveyor.com/api/projects/status/1t0mfwjgoq8cwvy8?svg=true)
A Networking Library primarily for games but can also be used for other stuff.

# How to use
### Server
* Inherit from `GameLogic` and write your Game-Logic in the `Update` Method.
* In your Main-Method Call `ServerStarter.Start(gameLogic);` with your inherited GameLogic class.
* For Server sending, write methods that look like `ServerSend` `Welcome` and `UdpTest`.
* For Server receiving, add methods to the Servers `PacketHandlers`. Method Examples are `ServerHandler`'s `WelcomeReceived` and `UdpTestReceived`.
### Client
* Inherit from Udp and from Tcp and override `ExecuteOnMainThread` so that the Action executes on your main Thread. (examples below)
* Call `Client.Init(tcp, udp);` with your inherited Udp and Tcp Classes.
    * You can enter an Ip there aswell (`Client.Init(tcp, udp, ip);`) 
* Call `Client.Instance.ConnectToServer();` to connect to the Server.
* For Client sending, write methods that look like `ClientSend` `Welcome` and `UdpTest`.
* For Client receiving, add methods to the Clients `PacketHandlers`. Method Examples are `ClientHandler`'s `WelcomeReceived` and `UdpTestReceived`.
* For proper Disconnecting, call `Client.Instance.Disconnect();`
### Examples
#### ExecuteOnMainThread
For a Console App, use the `Client.ThreadManager` class:
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

# Things you can change:
### Client
* IP of the Server:`Constants.Ip`
* Port of the Server:`Constants.Port`
### Server
* Port of the Server:`Constants.Port`
* Maximum number of Concurrent Players:`Constants.MaxPlayers`
* How often the Server updates every second:`Constants.TicksPerSec`
* IP of the Server with `Constants.Ip`

# Credits
I followed [this tutorial](https://www.youtube.com/playlist?list=PLXkn83W0QkfnqsK8I0RAz5AbUxfg3bOQ5) for most of the code.
