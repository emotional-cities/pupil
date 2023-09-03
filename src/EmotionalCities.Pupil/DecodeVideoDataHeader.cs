using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace EmotionalCities.Pupil
{
    /// <summary>
    /// Represents an operator that decodes a sequence of binary encoded video data headers.
    /// </summary>
    [Description("Decodes a sequence of binary encoded video data headers.")]
    public class DecodeVideoDataHeader : Transform<byte[], VideoDataHeader>
    {
        /// <summary>
        /// Decodes an observable sequence of binary encoded video data headers.
        /// </summary>
        /// <param name="source">
        /// The sequence of video data headers encoded as raw binary data.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="VideoDataHeader"/> objects representing the
        /// decoded headers.
        /// </returns>
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

    /// <summary>
    /// Represents information about a video data frame.
    /// </summary>
    public struct VideoDataHeader
    {
        /// <summary>
        /// The format of each video frame.
        /// </summary>
        public int Format;

        /// <summary>
        /// The width of the video frame, in pixels.
        /// </summary>
        public int Width;

        /// <summary>
        /// The height of the video frame, in pixels.
        /// </summary>
        public int Height;

        /// <summary>
        /// The sequence number for the video frame.
        /// </summary>
        public int Sequence;

        /// <summary>
        /// The timestamp of the video frame.
        /// </summary>
        public long Time;

        /// <summary>
        /// The pointer to the raw video frame data.
        /// </summary>
        public int DataBytes;

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        public int Reserved;
    }
}
