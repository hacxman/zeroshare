using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace zeroshare
{
	public class StdinContentDecoder: ContentDecoder, IContentDecoder
	{
    	private byte[] buffer;
    
		public StdinContentDecoder ()
		{
      		buffer = new byte[1024];
		}
    
		#region IContentDecoder implementation
		public void Decode (System.Net.IPEndPoint remote_endpoint)
		{
			Socket sock = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      		sock.Connect(remote_endpoint);
      		
      		NetworkStream stream = new NetworkStream(sock);
      		
      		do
      		{
        		stream.Read(buffer, 0, buffer.Length);
        		System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding ();
        		Console.Error.Write("{0}", encoding.GetString(buffer));
      		}
      		while (stream.DataAvailable);
		}
		
		#endregion
	}
}

