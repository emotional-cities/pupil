using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bonsai;
using NetMQ.Zyre;
using NetMQ.Zyre.ZyreEvents;
using Newtonsoft.Json.Linq;

namespace EmotionalCities.Pupil
{
    /// <summary>
    /// Represents an operator that finds a pupil device in the local network interface
    /// and returns the sequence of all its sensors.
    /// </summary>
    [Description("Finds a pupil device in the local network interface and returns the sequence of all its sensors.")]
    public class PupilDevice : Source<SensorInfo>
    {
        /// <summary>
        /// Gets or sets the interface for the network.
        /// </summary>
        [Description("The local network interface to use for discovery.")]
        public string Interface { get; set; }

        /// <summary>
        /// Finds a pupil device in the local network interface and returns an
        /// observable sequence of all its sensors.
        /// </summary>
        /// <returns>
        /// A sequence of <see cref="SensorInfo"/> objects representing each of the
        /// pupil device sensors.
        /// </returns>
        public override IObservable<SensorInfo> Generate()
        {
            return Observable.Create<SensorInfo>(observer =>
            {
                var zyre = new Zyre(nameof(PupilDevice));
                zyre.Join("pupil-mobile-v4");
                zyre.SetInterface(Interface);
                zyre.Start();

                var sensorInfo = Observable.FromEventPattern<ZyreEventWhisper>(
                    handler => zyre.WhisperEvent += handler,
                    handler => zyre.WhisperEvent -= handler)
                    .Select(evt =>
                    {
                        var message = evt.EventArgs.Content;
                        var payload = message.First.ConvertToString();
                        dynamic jData = JObject.Parse(payload);
                        string sensorName = jData.sensor_name;
                        string sensorUuid = jData.sensor_uuid;
                        string dataEndpoint = jData.data_endpoint;
                        string commandEndpoint = jData.command_endpoint;

                        return new SensorInfo
                        {
                            SensorName = sensorName,
                            SensorUuid = sensorUuid,
                            DataEndpoint = dataEndpoint,
                            CommandEndpoint = commandEndpoint
                        };
                    });
                var subscription = sensorInfo.Subscribe(observer);
                return Disposable.Create(() =>
                {
                    subscription.Dispose();
                    Task.Run(() =>
                    {
                        zyre.Stop();
                        zyre.Dispose();
                    });
                });
            });
        }
    }
}
