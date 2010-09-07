using System;

using Mono.Zeroconf;
using System.Net;
using System.Threading;

namespace zeroshare
{
	public class Zeroshare
	{
    	private RegisterService service;
    	private bool running = false;
    	private string pin;
    
		public bool Running {
			get {
				return this.running;
			}
		}
    
		public string Pin {
			get {
				return this.pin;
			}
		}

		public Zeroshare ()
		{
			service = new RegisterService ();
			service.Name = "Zeroshare";
			service.RegType = "_zeroshare._tcp";
			service.ReplyDomain = "local.";
			service.Port = 5566;
			
      		pin = "0000";
        	Console.WriteLine ("PIN: {0}", pin);
			
			// TxtRecords are optional
			TxtRecord txt_record = new TxtRecord ();
			txt_record.Add ("PIN", pin);
			service.TxtRecord = txt_record;
			
			service.Response += HandleServiceResponse;
		}
  
  		public void Start ()
  		{
  			running = true;
			service.Register ();
  		}

		private void HandleServiceResponse (object o, RegisterServiceEventArgs args)
		{
			switch (args.ServiceError) {
			case ServiceErrorCode.NameConflict:
				Console.WriteLine ("*** Name Collision! '{0}' is already registered", args.Service.Name);
				break;
			case ServiceErrorCode.None:
				Console.WriteLine ("*** Registered name = '{0}'", args.Service.Name);
				break;
			case ServiceErrorCode.Unknown:
				Console.WriteLine ("*** Error registering name = '{0}'", args.Service.Name);
				break;
			}
			
			IContentProvider cp = new HelloWorldContentProvider ();
			cp.Stream (new IPEndPoint (IPAddress.Any, args.Service.Port));
      		running = false;
			Console.WriteLine ("*** Unregistering service");
			service.Dispose ();
		}
	}
}

