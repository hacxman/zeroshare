using System;

using Mono.Zeroconf;
using System.Net;
using System.Threading;

namespace zeroshare
{
	public class ZeroshareServer: ZeroshareBase
	{
    	private RegisterService service;

		public ZeroshareServer (string pin): base(pin)
		{
			service = new RegisterService ();
			service.Name = "Zeroshare";
			service.RegType = "_zeroshare._tcp";
			service.ReplyDomain = "local.";
			service.Port = 5566;
			
        	Console.Error.WriteLine ("PIN: {0}", pin);
			
			// TxtRecords are optional
			TxtRecord txt_record = new TxtRecord ();
			txt_record.Add ("PIN", pin);
			service.TxtRecord = txt_record;
			
			service.Response += HandleServiceResponse;
		}
  
  		public void Start ()
  		{
  			Running = true;
			service.Register ();
  		}

		private void HandleServiceResponse (object o, RegisterServiceEventArgs args)
		{
			switch (args.ServiceError) {
			case ServiceErrorCode.NameConflict:
				Console.Error.WriteLine ("*** Name Collision! '{0}' is already registered", args.Service.Name);
				break;
			case ServiceErrorCode.None:
				Console.Error.WriteLine ("*** Registered name = '{0}'", args.Service.Name);
				break;
			case ServiceErrorCode.Unknown:
				Console.Error.WriteLine ("*** Error registering name = '{0}'", args.Service.Name);
				break;
			}
			
			//IContentEncoder cp = new HelloWorldContentEncoder ();
      		IContentEncoder cp = new StdinContentEncoder ();
			cp.Encode (new IPEndPoint (IPAddress.Any, args.Service.Port));
      		Running = false;
			Console.Error.WriteLine ("*** Unregistering service");
			service.Dispose ();
		}
	}
}

