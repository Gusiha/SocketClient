using System.Net;
using System.Net.Sockets;
using System.Text;

int CalculatePort(string passiveResponse)
{
    int start = passiveResponse.IndexOf('(') + 1;
    int end = passiveResponse.IndexOf(')') - start;
    string dataPortPart = passiveResponse.Substring(start, end);
    string[] parts = dataPortPart.Split(',');
    int partOne = int.Parse(parts[4]);
    int partTwo = int.Parse(parts[5]);
    return (partOne * 256) + partTwo;

}

void SendMessage(Socket socket, string command)
{
    byte[] newBuffer = new byte[1024];
    newBuffer = Encoding.UTF8.GetBytes(command + "\r\n");
    socket.Send(newBuffer);
    Console.WriteLine(command);
}

string RecieveMessage(Socket socket, byte[] buffer)
{
    buffer = new byte[1024];
    int byteAmount = socket.Receive(buffer);
    Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, byteAmount));
    return Encoding.UTF8.GetString(buffer);
}
string ipAddress = "10.102.32.145";
//string ipAddress = "10.126.0.26";
//string ipAddress = "10.102.81.138";
//string ipAddress = "10.102.220.114";


int port = 21;
IPAddress IPAddress = IPAddress.Parse(ipAddress);
Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
Socket fileSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
string path = "C:\\Users\\timam\\Downloads\\Telegram Desktop\\cms159\\cms159\\Mail\\Mailbox\\Stud01\\00500007.msg";
byte[] buffer = new byte[2048];
string message;
string URL = "ftp.dlptest.com";
string user = "dlpuser";
string password = "rNrKYTX9g7z3RgJRmxWuGHbeu";
int byteAmount = 0;

socket.Connect(URL, port);
while (socket.Connected)
{
    buffer = new byte[1024];
    RecieveMessage(socket, buffer);

    SendMessage(socket, $"USER {user}");
    RecieveMessage(socket, buffer);

    SendMessage(socket, $"PASS {password}");
    RecieveMessage(socket, buffer);

    //SendMessage(socket, $"PUT {path}");
    //RecieveMessage(socket, buffer);


    SendMessage(socket, $"PASV");
    message = RecieveMessage(socket, buffer);

    

    //int startIndex = message.IndexOf('(') + 1;
    //int endIndex = message.IndexOf(')') - startIndex;
    //string dataPortPart = message.Substring(startIndex, endIndex);
    //message = message.Substring(startIndex, endIndex).Replace(',','.');
    //string[] parts = dataPortPart.Split(',');
    //int partOne = int.Parse(parts[4]);
    //int partTwo = int.Parse(parts[5]);


    int portForFileSocket = CalculatePort(message);

    Console.WriteLine(portForFileSocket);

    fileSocket.Connect(URL, portForFileSocket);


    SendMessage(socket, $"STOR {path}");
    RecieveMessage(socket, buffer);

    using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
    {
        byte[] newBuffer = new byte[4096];
        int bytesRead;
        while ((bytesRead = fileStream.Read(newBuffer, 0, newBuffer.Length)) > 0)
        {
            fileSocket.Send(newBuffer, bytesRead, SocketFlags.None);
        }
    }


    fileSocket.Shutdown(SocketShutdown.Both);
    fileSocket.Close();
    RecieveMessage(socket, buffer);


    SendMessage(socket, "PASV");
    message = RecieveMessage(socket, buffer);
    portForFileSocket = CalculatePort(message);



    Socket downloadSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    downloadSocket.Connect(URL, portForFileSocket);
    SendMessage(socket, $"RETR {path}");
    RecieveMessage(socket, buffer); // Получение ответа от сервера перед получением данных

    using (var networkStream = new NetworkStream(downloadSocket, true))
    {
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            networkStream.CopyTo(fileStream);
        }
    }



    RecieveMessage(socket, buffer);


    //SendMessage(socket, Console.ReadLine());
    //RecieveMessage(socket, buffer);







    //SendMessage(socket, $"PUT {path}");
    //RecieveMessage(socket, buffer);
    ////byteAmount = socket.Receive(buffer);
    ////Console.WriteLine(ASCIIEncoding.ASCII.GetString(buffer, 0, byteAmount));




    //command = Console.ReadLine() + "\r\n";
    //buffer = Encoding.UTF8.GetBytes(command);
    //socket.Send(buffer);
    //Console.WriteLine($"Message sent to {ipAddress}");
    //Console.WriteLine($"Message recieved from {ipAddress} : {Encoding.UTF8.GetString(buffer)}");
    socket.Close();
}