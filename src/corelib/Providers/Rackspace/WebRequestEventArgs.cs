namespace net.openstack.Providers.Rackspace
{
    using System;
    using System.Net;

    public class WebRequestEventArgs : EventArgs
    {
        private readonly HttpWebRequest _request;

        public WebRequestEventArgs(HttpWebRequest request)
        {
            _request = request;
        }

        public HttpWebRequest Request
        {
            get
            {
                return _request;
            }
        }
    }
}
