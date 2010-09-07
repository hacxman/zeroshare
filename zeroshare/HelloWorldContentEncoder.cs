using System;

using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace zeroshare
{
	public class HelloWorldContentEncoder : ContentEncoder, IContentEncoder
	{
    	private ManualResetEvent allDone;
    
		public HelloWorldContentEncoder ()
		{
			allDone = new ManualResetEvent (false);
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
			allDone.Reset ();
			sock.BeginAccept (new AsyncCallback (this.AcceptCallback), sock);
			allDone.WaitOne ();
      		Console.Error.WriteLine ("Closing socket");
			sock.Close ();
		}
		#endregion
    
    	private void AcceptCallback (IAsyncResult ar)
    	{
    		// Get the socket that handles the client request.
    		Socket sock = (Socket)ar.AsyncState;
    		Socket client = sock.EndAccept (ar);
    		
    		Console.Error.WriteLine ("Got connection from {0}", client.RemoteEndPoint);
    		
      		System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding ();
    		client.Send (encoding.GetBytes ("Hello world!\n"));
    		client.Close ();
      		allDone.Set ();
    	}
  	}
}

