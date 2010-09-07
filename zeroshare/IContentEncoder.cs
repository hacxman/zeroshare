using System;
using System.Net;
namespace zeroshare
{
	public interface IContentEncoder
	{
    	void Encode (IPEndPoint local_endpoint);
	}
}

