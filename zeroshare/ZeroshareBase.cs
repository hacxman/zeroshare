using System;
namespace zeroshare
{
	public class ZeroshareBase
	{
    	private string pin = "0000";
    	private bool running = false;
    	
		public string PIN {
			get {
				return this.pin;
			}
		}
    
		public bool Running {
			get {
				return this.running;
			}
      		protected set {
        		this.running = value;
      		}
		}
    
    	public ZeroshareBase (string PIN)
    	{
    		this.pin = PIN;
    	}
	}
}

