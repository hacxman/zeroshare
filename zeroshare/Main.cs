using System;

using Mono.Zeroconf;
using System.Net;
using System.Threading;

namespace zeroshare
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Zeroshare zs = new Zeroshare ();
			zs.Start ();
			
      		while (zs.Running)
      		{
				//Console.WriteLine ("Zeroshare state: {0}", zs.Running);
				Thread.Sleep (1000);
      		}
		}
	}
}

