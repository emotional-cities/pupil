using Bonsai;
using NetMQ;
using OpenCV.Net;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace EmotionalCities.Pupil
{
    /// <summary>
    /// Represents an operator that decodes a sequence of binary encoded gaze measurements.
    /// </summary>
    [Combinator]
    [Description("Decodes a sequence of binary encoded gaze measurements.")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class DecodePupilGaze
    {
        /// <summary>
        /// Decodes an observable sequence of gaze measurements retrieved
        /// from the Pupil Network API.
        /// </summary>
        /// <param name="source">The sequence of Network API messages.</param>
        /// <returns>
        /// A sequence of <see cref="Point2f"/> objects representing the
        /// decoded gaze measurement.
        /// </returns>
        public IObservable<Point2f> Process(IObservable<NetMQMessage> source)
        {
            return source.Select(message => ToPoint2f(message.Last.Buffer));
        }

        /// <summary>
        /// Decodes an observable sequence of binary encoded gaze measurements.
        /// </summary>
        /// <param name="source">
        /// The sequence of gaze measurements encoded as raw binary data.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="Point2f"/> objects representing the
        /// decoded gaze measurement.
        /// </returns>
        public IObservable<Point2f> Process(IObservable<byte[]> source)
        {
            return source.Select(ToPoint2f);
        }

        static Point2f ToPoint2f(byte[] value)
        {
            return new Point2f(
                x: BitConverter.ToSingle(value, 0),
                y: BitConverter.ToSingle(value, 4));
        }
    }
}
