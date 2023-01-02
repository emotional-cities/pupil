using Bonsai;
using System;
using System.Linq;
using System.Reactive.Linq;
using NetMQ.Zyre;
using NetMQ.Zyre.ZyreEvents;
using Newtonsoft.Json.Linq;

namespace PupilInterface
{
    public class ZreNode : Source<SensorData>
    {
        /// <summary>
        /// Gets or sets the Zyre node name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Zyre group to join.
        /// </summary>
        public string JoinGroup { get; set; }

        public override IObservable<SensorData> Generate()
        {
            Zyre zyre = new Zyre(Name);

            zyre.Join(JoinGroup);

            zyre.Start();

            return Observable.FromEventPattern<ZyreEventWhisper>(zyre, "WhisperEvent")
                .Select(e => {
                    var payload = e.EventArgs.Content.Last.ConvertToString();
                    dynamic jData = JObject.Parse(payload);
                    string sensorName = jData.sensor_name;
                    string dataEndpoint = jData.data_endpoint;

                    return new SensorData { SensorName = sensorName, DataEndpoint = dataEndpoint };
                })
                .Finally(() => zyre.Dispose());
        }
    }

    public class SensorData
    {
        public string SensorName;
        public string DataEndpoint;
    }
}
