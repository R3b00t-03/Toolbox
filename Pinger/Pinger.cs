using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace Pinger
{
    public class Pinger
    {
        private bool _pingable;
        private float _rtt;
        private string _host_or_address;
        private DateTime _TimeStamp;
        public bool Pingable { get => _pingable; }
        public float RoundTripTime { get => _rtt; }
        public string Host { get => _host_or_address; }
        public DateTime Timestamp { get => _TimeStamp; }


        public Pinger(string host_or_address)
        {
            _host_or_address = host_or_address;
        }
        public void SendPing()
        {
            try
            {
                Ping ping = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                PingReply reply;
                string data = "abcdefghijklmnopqrstuvwxyz";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                reply = ping.Send(_host_or_address, timeout, buffer, options);
                
                _rtt = reply.RoundtripTime;
                if(reply.Status == IPStatus.Success)
                {
                    _pingable = true;
                }
                else
                {
                    _pingable = false;
                }
            }
            catch (PingException ex)
            {

            }
            finally
            {
                _TimeStamp = DateTime.Now;
            }
        }

    }
}
