# How to use
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
* IP of the Server:`Constants.Ip`
* Port of the Server:`Constants.Port`