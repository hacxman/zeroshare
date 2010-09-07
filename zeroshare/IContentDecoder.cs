using System;
using System.Net;
namespace zeroshare
{
	public interface IContentDecoder
	{
    	void Decode (IPEndPoint remote_endpoint);
	}
}

