using Bonsai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PupilInterface
{
    public class ToVideoDataHeader : Transform<byte[], VideoDataHeader>
    {
        public override IObservable<VideoDataHeader> Process(IObservable<byte[]> source)
        {
            return source.Select(value =>
            {
                return new VideoDataHeader {
                    Format = BitConverter.ToInt32(value, 0),
                    Width = BitConverter.ToInt32(value, 4),
                    Height = BitConverter.ToInt32(value, 8),
                    Sequence = BitConverter.ToInt32(value, 12),
                    Time = BitConverter.ToInt64(value, 16),
                    DataBytes = BitConverter.ToInt32(value, 24),
                    Reserved = BitConverter.ToInt32(value, 28),
                };
            });
        }
    }

    public struct VideoDataHeader
    {
        public int Format;
        public int Width;
        public int Height;
        public int Sequence;
        public long Time;
        public int DataBytes;
        public int Reserved;
    }
}
