using Bonsai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PupilInterface
{
    public class DecodeByteFrame : Transform<byte[], int>
    {
        public override IObservable<int> Process(IObservable<byte[]> source)
        {
            FFmpegBinariesHelper.RegisterFFmpegBinaries(); // TODO - this sort of stuff should probably go in some general resource node

            return source.Select(val =>
            {
                return 1;
            });
        }
    }
}
