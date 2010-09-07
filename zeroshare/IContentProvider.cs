using System;
using System.Net;
namespace zeroshare
{
	public interface IContentProvider
	{
    	void Stream (IPEndPoint local_endpoint);
	}
}

