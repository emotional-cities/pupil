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
    /// Represents an operator that decodes a sequence of IMU data frames.
    /// </summary>
    [Combinator]
    [Description("Decodes a sequence of IMU data frames.")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class DecodeImuData
    {
        /// <summary>
        /// Decodes an observable sequence of IMU data frames retrieved
        /// from the Pupil Network API.
        /// </summary>
        /// <param name="source">The sequence of Network API messages.</param>
        /// <returns>
        /// A sequence of <see cref="ImuFrame"/> objects representing the
        /// decoded IMU data.
        /// </returns>
        public IObservable<ImuFrame> Process(IObservable<NetMQMessage> source)
        {
            return source.Select(message => ToImuFrame(message.Last.Buffer));
        }

        /// <summary>
        /// Decodes an observable sequence of binary encoded IMU data frames.
        /// </summary>
        /// <param name="source">
        /// The sequence of IMU data frames encoded as raw binary data.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="ImuFrame"/> objects representing the
        /// decoded IMU data.
        /// </returns>
        public IObservable<ImuFrame> Process(IObservable<byte[]> source)
        {
            return source.Select(ToImuFrame);
        }

        unsafe static ImuFrame ToImuFrame(byte[] value)
        {
            // according to spec: https://github.com/pupil-labs/pyndsi/blob/v1.4.4/ndsi-commspec.md#imu
            // timestamp: high, low, accel: x, y, z, gyro: x, y, z
            const int Stride = 32;
            const int Columns = Stride / 4;
            var count = value.Length / Stride;
            using var dataHeader = Mat.CreateMatHeader(value, count, Columns, Depth.F32, 1);
            var data = new Mat(dataHeader.Cols, dataHeader.Rows, dataHeader.Depth, dataHeader.Channels);
            CV.Transpose(dataHeader, data);

            var timestamp = new ulong[data.Cols];
            fixed (byte* dataPtr = value)
            {
                ulong* timePtr = (ulong*)dataPtr;
                for (int i = 0; i < timestamp.Length; i++)
                {
                    timestamp[i] = timePtr[i * Stride];
                }
            }

            return new ImuFrame
            {
                Timestamp = timestamp,
                Data = data.GetSubRect(new Rect(0, 2, data.Cols, data.Rows - 2))
            };
        }
    }

    /// <summary>
    /// Represents a single frame of IMU data containing both a buffer of motion
    /// samples and the array of timestamps for each sample in the buffer.
    /// </summary>
    public struct ImuFrame
    {
        /// <summary>
        /// The array of 64-bit timestamps, in nanoseconds from 1 January 1970
        /// for each IMU sample.
        /// </summary>
        public ulong[] Timestamp;

        /// <summary>
        /// The motion sensor data represented as a multi-channel matrix
        /// where rows are channels and columns are samples.
        /// </summary>
        public Mat Data;
    }
}
