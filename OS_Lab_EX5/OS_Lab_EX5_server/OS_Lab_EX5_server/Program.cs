using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Program
{
    static TcpListener tcpServer;

    public static void Main()
    {
        StartListen();
    }

    public static void StartListen()
    {
        try
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            tcpServer = new TcpListener(localAddr, 5000);
            tcpServer.Start();

            Console.WriteLine("Waiting for a client to connect...");

            // Keep on accepting Client Connection
            while (true)
            {
                TcpClient tcpClient = tcpServer.AcceptTcpClient();
                Console.WriteLine("Client connected...");

                // New thread for handling each client
                ThreadPool.QueueUserWorkItem(HandleClient, tcpClient);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void HandleClient(Object obj)
    {
        var client = (TcpClient)obj;
        NetworkStream ns = client.GetStream();

        try
        {
            using (FileStream fileStream = File.Create("received_file.txt"))
            {
                byte[] buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = ns.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                }
            }

            Console.WriteLine("File received and saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred while receiving or saving the file: " + ex.Message);
        }
        finally
        {
            ns.Close();
            client.Close();
        }
    }

}
