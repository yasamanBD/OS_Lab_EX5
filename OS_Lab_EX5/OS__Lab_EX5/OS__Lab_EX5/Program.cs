using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

public class TcpClientSample
{
    public static void Main()
    {
        int port;
        TcpClient server;

        Console.WriteLine("Please Enter the port number of Server:");
        port = Int32.Parse(Console.ReadLine());
        try
        {
            server = new TcpClient("127.0.0.1", port);
        }
        catch (SocketException)
        {
            Console.WriteLine("Unable to connect to server");
            return;
        }
        Console.WriteLine("Connected to the Server...");

        NetworkStream ns = server.GetStream();

        // Read file content
        byte[] fileData = File.ReadAllBytes("D:\\Uni\\OS Lab\\OS_Lab_EX5\\test.txt"); // Replace "file_path_here" with the actual path to your file

        // Send file data
        ns.Write(fileData, 0, fileData.Length);
        ns.Flush();

        Console.WriteLine("File sent to the server...");

        Console.WriteLine("Disconnecting from server...");
        ns.Close();
        server.Close();
    }
}

