namespace net.openstack.Providers.Rackspace
{
    using System;
    using System.Net;

    public class WebResponseEventArgs : EventArgs
    {
        private readonly HttpWebResponse _response;

        public WebResponseEventArgs(HttpWebResponse response)
        {
            _response = response;
        }

        public HttpWebResponse Response
        {
            get
            {
                return _response;
            }
        }
    }
}
