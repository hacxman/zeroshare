using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
namespace zeroshare
{
	public class StdinContentEncoder : IContentEncoder
	{
		public StdinContentEncoder ()
		{
		}
	

		#region IContentEncoder implementation
		public void Encode (IPEndPoint local_endpoint)
		{
			Socket sock = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			sock.Bind (local_endpoint);
			sock.Listen (5);
			
			//      		for (;;)
			//      		{
			//				allDone.Reset ();
			//				Console.Error.WriteLine ("Waiting for connection on port {0}", local_endpoint);
			//				sock.BeginAccept (new AsyncCallback (this.AcceptCallback), sock);
			//				allDone.WaitOne ();
			//      		}
			
      		Console.Error.WriteLine ("Waiting for connection on port {0}", local_endpoint);
      		Socket client = sock.Accept();

    		NetworkStream stream = new NetworkStream(client);

    		Console.Error.WriteLine ("Got connection from {0}", client.RemoteEndPoint);

      		byte[] buffer = new byte[1024];
      		System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
      
      		Stream stdin = Console.OpenStandardInput(buffer.Length);

      		while (stdin.Read(buffer, 0, buffer.Length) != 0)
			{
        		Console.WriteLine("Data: {0}", encoding.GetString(buffer));
				stream.Write(buffer, 0, buffer.Length);
			};
      
      		Console.Error.WriteLine ("Closing socket");
			sock.Close ();
		}
		#endregion    
	}
}

