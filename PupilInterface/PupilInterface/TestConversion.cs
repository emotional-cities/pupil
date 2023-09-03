using Bonsai;
using System;
using System.Linq;
using System.Reactive.Linq;
using NetMQ.Zyre;
using NetMQ.Zyre.ZyreEvents;
using NetMQ;


namespace PupilInterface
{
    public class TestConversion : Transform<NetMQFrame, float>
    {
        public override IObservable<float> Process(IObservable<NetMQFrame> source)
        {
            return source.Select(frame =>
            {
                return BitConverter.ToSingle(frame.Buffer, 0);
            });
        }
    }
}
