using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;

namespace PingerTest
{
    [TestClass]
    public class PingerTest
    {
        //https://www.meziantou.net/mstest-v2-exploring-asserts.htm

        /// <summary>
        /// Ping Google DNS (8.8.8.8) 10 times
        /// and check results
        /// Throw fail if exception Triggers
        /// </summary>
        [TestMethod]
        public void ping_google_10_times()
        {
            int run_test_times = 10;
            try
            {
                Pinger.Pinger pinger = new Pinger.Pinger("8.8.8.8"); // Google DNS server
                List<Pinger.PingResultsItemTemplate> results = new List<Pinger.PingResultsItemTemplate>();
                for(int i = 0; i < run_test_times; i++)
                {
                    pinger.SendPing();
                    Pinger.PingResultsItemTemplate item = new Pinger.PingResultsItemTemplate();
                    item.TimeStamp = pinger.Timestamp;
                    item.Pingable = pinger.Pingable;
                    item.roundTripTime = pinger.RoundTripTime;
                    item.Host = pinger.Host;
                    results.Add(item);
                    //Thread.Sleep(1000);
                }
                
            }
            catch(PingException ex)
            {
                Assert.Fail("Problem with Ping: " + ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("General Problem: " + ex.Message);
            }
        }

        [TestMethod]
        public void ping_wrong_or_unreachable_host()
        {
            try
            {
                Pinger.Pinger pinger = new Pinger.Pinger("awdafajfiwajfaiofj"); // Random (hopefully) non existant Host
                pinger.SendPing();

                // Check Results
                Assert.IsFalse(pinger.Pingable);
                Assert.AreEqual(pinger.RoundTripTime, 0);
            }
            catch (PingException ex)
            {
                Assert.Fail("Problem with Ping: " + ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("General Problem: " + ex.Message);
            }
        }
    }
}