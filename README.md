# Swerver
A Networking Library primarily for games but can also be used for other stuff.

# How to use
### Server
* Inherit from `Swerver.Server.GameLogic` and write your Game-Logic in the `Update` Method.
* In your Main-Method Call `Swerver.Server.ServerStarter.Start(gameLogic);` with your inherited GameLogic class.
* For Server sending, write methods that look like `Swerver.Server.ServerSend` `Welcome` and `UdpTest`.
* For Server receiving, add a Method to the Servers `PacketHandlers`. Method Examples are `Swerver.Server.ServerHandler`'s `WelcomeReceived` and `UdpTestReceived`.
### Client
* Inherit from Udp and from Tcp and override `ExecuteOnMainThread` so that the Action executes on your main Thread. (examples below)
* Call `Client.Init(tcp, udp);` with your inherited Udp and Tcp Classes.
    * You can enter an Ip there aswell (`Client.Init(tcp, udp, ip);`) 
* Call `Client.Instance.ConnectToServer();` to connect to the Server.
* For Client sending, write methods that look like `Swerver.Client.ClientSend` `Welcome` and `UdpTest`.
* For Client receiving, add a Method to the Servers `PacketHandlers`. Method Examples are `Swerver.Client.ClientHandler`'s `WelcomeReceived` and `UdpTestReceived`.
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

# Things you can change:
### Client
* IP of the Server:`Swerver.Client.Constants.Ip`
* Port of the Server:`Swerver.Client.Constants.Port`
### Server
* Port of the Server:`Swerver.Server.Constants.Port`
* Maximum number of Concurrent Players:`Swerver.Server.Constants.MaxPlayers`
* How often the Server updates every second:`Swerver.Server.Constants.TicksPerSec`
* IP of the Server with `Swerver.Server.Constants.Ip`

# Credits
I followed [this tutorial](https://www.youtube.com/playlist?list=PLXkn83W0QkfnqsK8I0RAz5AbUxfg3bOQ5) for most of the code.
