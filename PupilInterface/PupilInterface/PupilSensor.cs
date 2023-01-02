using Bonsai;
using System;
using System.Linq;
using System.Reactive.Linq;
using NetMQ.Zyre;
using NetMQ.Zyre.ZyreEvents;
using Newtonsoft.Json.Linq;
using NetMQ;

namespace PupilInterface
{
    public class PupilSensor : Transform<NetMQFrame, SensorData>
    {
        public override IObservable<SensorData> Process(IObservable<NetMQFrame> source)
        {
            return source.Select(message =>
            {
                var payload = message.ConvertToString();
                dynamic jData = JObject.Parse(payload);
                string sensorName = jData.sensor_name;
                string dataEndpoint = jData.data_endpoint;

                return new SensorData { SensorName = sensorName, DataEndpoint = dataEndpoint };
            });
        }
    }

    public class SensorData
    {
        public string SensorName;
        public string DataEndpoint;
    }
}
