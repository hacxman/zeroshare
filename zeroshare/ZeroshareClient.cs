using System;

using Mono.Zeroconf;
using System.Net;

namespace zeroshare
{
	public class ZeroshareClient: ZeroshareBase
	{    
		public ZeroshareClient (string pin): base(pin)
		{      		
		}
    
    	public void Connect ()
    	{
      		Running = true;
    		Console.Error.WriteLine ("Starting service browser...");
    		ServiceBrowser browser = new ServiceBrowser ();
      
			browser.ServiceAdded += HandleBrowserServiceAdded;
			browser.Browse ("_zeroshare._tcp", "local");
	    }

    	private void HandleBrowserServiceAdded (object o, ServiceBrowseEventArgs args)
    	{
			Console.Error.WriteLine ("Found Service: {0}", args.Service.Name);
			args.Service.Resolved += HandleArgsServiceResolved;
			args.Service.Resolve ();
    	}

    	void HandleArgsServiceResolved (object o, ServiceResolvedEventArgs args)
    	{
    		IResolvableService s = (IResolvableService)args.Service;
    		Console.Error.WriteLine ("Resolved Zeroshare serivce: {0} - {1}:{2} ({3} TXT record entries) [PIN: {4}]", s.FullName, s.HostEntry.AddressList[0], s.Port, s.TxtRecord.Count, s.TxtRecord["PIN"].ValueString);
    		
      		if (s.TxtRecord["PIN"].ValueString == PIN)
			{
    			Console.Error.WriteLine ("Found my content source, streaming...");
        		new StdinContentDecoder().Decode(new IPEndPoint(s.HostEntry.AddressList[0], s.Port));
        		Running = false;
        		Console.Error.WriteLine ("Streaming done");
			}
    	}
	}
}

