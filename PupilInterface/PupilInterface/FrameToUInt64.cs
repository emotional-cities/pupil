using Bonsai;
using System;
using System.Linq;
using System.Reactive.Linq;
using NetMQ;

namespace PupilInterface
{
    public class FrameToUInt64 : Transform<NetMQFrame, ulong>
    {
        public int Offset { get; set; } = 0;

        public override IObservable<ulong> Process(IObservable<NetMQFrame> source)
        {
            return source.Select(frame =>
            {
                return BitConverter.ToUInt64(frame.Buffer, Offset);
            });
        }
    }
}
