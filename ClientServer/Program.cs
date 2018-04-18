using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClientServer
{
	class Program
	{
		static void Main(string[] args)
		{
			TcpListener socket = new TcpListener(IPAddress.Any, 1337);
			socket.Start();

			while(true)
			{
				TcpClient client = socket.AcceptTcpClient();
				Console.WriteLine("User connected");

				Thread t = new Thread(handle_clients);
				t.Start(client);
			}
		}

		public static void handle_clients(object client)
		{
			TcpClient tcp = (TcpClient)client;
			NetworkStream stream = tcp.GetStream();

			while (true)
			{
				byte[] buffer = new byte[1024];
				int byte_count = stream.Read(buffer, 0, buffer.Length);

				if(byte_count == 0)
				{
					break;
				}

				writeClient(tcp, "Received data");

				string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
				Console.WriteLine(data);
			}

			Console.WriteLine("Client disconnected");
			tcp.Client.Shutdown(SocketShutdown.Both);
			tcp.Close();
		}

		public static void writeClient(TcpClient c, string s)
		{
			byte[] buffer = Encoding.ASCII.GetBytes(s);
			NetworkStream ns = c.GetStream();
			ns.Write(buffer, 0, buffer.Length);
		}
	}
}
