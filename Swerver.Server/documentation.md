# How to use
* Inherit from `GameLogic` and write your Game-Logic in the `Update` Method.
* In your Main-Method Call `ServerStarter.Start(gameLogic);` with your inherited GameLogic class.
* For Server sending, write methods that look like `ServerSend` `Welcome` and `UdpTest`.
* For Server receiving, add methods to the Servers `PacketHandlers`. Method Examples are `ServerHandler`'s `WelcomeReceived` and `UdpTestReceived`.

# Things you can change:
* Port of the Server:`Constants.Port`
* Maximum number of Concurrent Players:`Constants.MaxPlayers`
* How often the Server updates every second:`Constants.TicksPerSec`
* IP of the Server with `Constants.Ip`
