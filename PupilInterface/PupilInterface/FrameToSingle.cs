using Bonsai;
using System;
using System.Linq;
using System.Reactive.Linq;
using NetMQ;

namespace PupilInterface
{
    public class FrameToSingle : Transform<NetMQFrame, float>
    {
        public int Offset { get; set; } = 0;

        public override IObservable<float> Process(IObservable<NetMQFrame> source)
        {
            return source.Select(frame =>
            {
                return BitConverter.ToSingle(frame.Buffer, Offset);
            });
        }
    }
}
