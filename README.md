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
For Client sending, write methods that look like ServerLibrary.Server.ServerSend Welcome and UdpTest.
For Client receiving, add a Method to the Servers PacketHandlers. Method Examples are ServerLibrary.Server.ServerHandler WelcomeReceived and UdpTestReceived.

# Credits
I followed [this tutorial](https://www.youtube.com/playlist?list=PLXkn83W0QkfnqsK8I0RAz5AbUxfg3bOQ5) for most of the code.
