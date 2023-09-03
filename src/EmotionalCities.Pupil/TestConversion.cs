using Bonsai;
using System;
using System.Linq;
using System.Reactive.Linq;
using NetMQ;


namespace EmotionalCities.Pupil
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
