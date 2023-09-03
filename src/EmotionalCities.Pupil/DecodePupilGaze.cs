using Bonsai;
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
        /// Decodes an observable sequence of binary encoded gaze measurements.
        /// </summary>
        /// <param name="source">
        /// The sequence of gaze measurements encoded as raw binary data.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="PupilGaze"/> objects representing the
        /// decoded gaze measurement.
        /// </returns>
        public IObservable<PupilGaze> Process(IObservable<byte[]> source)
        {
            return source.Select(value => new PupilGaze
            {
                X = BitConverter.ToSingle(value, 0),
                Y = BitConverter.ToSingle(value, 4)
            });
        }
    }

    /// <summary>
    /// Represents a single gaze measurement.
    /// </summary>
    public struct PupilGaze
    {
        /// <summary>
        /// The x-position of the gaze measurement.
        /// </summary>
        public float X;

        /// <summary>
        /// The y-position of the gaze measurement.
        /// </summary>
        public float Y;
    }
}
