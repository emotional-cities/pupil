using Bonsai;
using System;
using System.Linq;
using System.Reactive.Linq;
using Newtonsoft.Json.Linq;
using NetMQ;

namespace EmotionalCities.Pupil
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
                string sensorUuid = jData.sensor_uuid;
                string dataEndpoint = jData.data_endpoint;
                string commandEndpoint = jData.command_endpoint;

                return new SensorData { SensorName = sensorName, SensorUuid = sensorUuid, DataEndpoint = dataEndpoint, CommandEndpoint = commandEndpoint };
            });
        }
    }

    public class SensorData
    {
        public string SensorName;
        public string SensorUuid;
        public string DataEndpoint;
        public string CommandEndpoint;
    }
}
