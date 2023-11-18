using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Unicode;
using System.Threading.Channels;

string message = "Hello from NoteBook";
Byte[] buffer = Encoding.UTF8.GetBytes(message);


int port = 3245;
IPAddress IPAddress = IPAddress.Parse("192.168.0.105");
Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Connect(IPAddress, port);    
if(socket.Connected)
{
    Console.WriteLine("Connected");
}

socket.Send(buffer, 0, buffer.Length, SocketFlags.None);

