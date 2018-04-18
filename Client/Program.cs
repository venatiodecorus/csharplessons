using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
	class Program
	{
		static void Main(string[] args)
		{
			IPAddress ip = IPAddress.Parse("127.0.0.1");
			int port = 1337;

			TcpClient client = new TcpClient();
			client.Connect(ip, port);
			NetworkStream stream = client.GetStream();

			Thread thread = new Thread(o => receiveData((TcpClient)o));
			thread.Start(client);

			string s;
			while(!string.IsNullOrEmpty((s = Console.ReadLine())))
			{
				byte[] buffer = Encoding.ASCII.GetBytes(s);
				stream.Write(buffer, 0, buffer.Length);
			}
			
			client.Client.Shutdown(SocketShutdown.Send);
			thread.Join();
			stream.Close();
			client.Close();
		}

		private static void receiveData(TcpClient client)
		{
			NetworkStream ns = client.GetStream();
			byte[] receivedBytes = new byte[1014];
			int byteCount;

			while((byteCount = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
			{
				Console.WriteLine(Encoding.ASCII.GetString(receivedBytes, 0, byteCount));
			}
		}
	}
}
