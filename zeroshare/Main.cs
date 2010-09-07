using System;

using Mono.Zeroconf;
using System.Net;
using System.Threading;
using System.IO;

namespace zeroshare
{
	class MainClass
	{
		public static void Main (string[] args)
		{      
			if (args.Length > 0)
      		{
				if (args[0] == "-s")
      			{
					string pin = "0000";
					if (args.Length > 1)
						pin = args[1];
					else
						pin = String.Format ("{0:0000}", new Random ().Next (1000, 9999));
          
					ZeroshareServer zs = new ZeroshareServer (pin);
					zs.Start ();
					
      				while (zs.Running)
						Thread.Sleep (1000);
				}
      			else
		  		{
					ZeroshareClient zc = new ZeroshareClient (args[0]);
					zc.Connect ();
					
      				while (zc.Running)
						Thread.Sleep (1000);
				}
      		}
		}
	}
}

