using System.Net;
using System.Net.Sockets;

int port = 3245;
IPAddress IPAddress = IPAddress.Parse("192.168.0.102");
Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
TcpClient client = new();
client.Connect(IPAddress, port);

