using System.Net;
using System.Net.Sockets;
using System.Text;

string ipAddress = "10.102.3.185";
//string ipAddress = "10.126.0.26";
//string ipAddress = "10.102.81.138";
//string ipAddress = "10.102.220.114";


int port = 11000;
IPAddress IPAddress = IPAddress.Parse(ipAddress);
Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


while (true)
{
    string message = Console.ReadLine();
    byte[] buffer = new byte[2048];
    buffer = Encoding.UTF8.GetBytes(message);
    socket.Connect(IPAddress, port);
    socket.Send(buffer);
    Console.WriteLine($"Message sent to {ipAddress}");
    buffer = new byte[1024];
    socket.Receive(buffer);
    Console.WriteLine($"Message recieved from {ipAddress} : {Encoding.UTF8.GetString(buffer)}");
    socket.Close();
    Console.WriteLine("Socket is closed");
}

