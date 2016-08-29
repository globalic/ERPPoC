using ERP.Microservice.Model;
using Medseek.Util.MicroServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Microservice.TestService
{

    public class TestService
    {
        public const string Xmlns = "";

        internal const string RoutingKeyPrefix = "ERP.services.TestService";
        internal const string ConsumeQueue = "ERP.Platform.Services.TestServiceQueue";
        internal const string Exchange = "ERP-api";

        public TestService()
        {

        }
        [MicroServiceBinding(Exchange, RoutingKeyPrefix + ".outbound", ConsumeQueue, AutoDelete = false, IsOneWay = true)]
        public void Process(TestMessage message)
        {
            Console.WriteLine("Service Tested");
        }
    }
}
