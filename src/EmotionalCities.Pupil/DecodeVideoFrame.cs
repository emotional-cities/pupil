using Bonsai;
using System;
using System.Linq;
using System.Reactive.Linq;
using FFmpeg.AutoGen.Abstractions;
using FFmpeg.AutoGen.Bindings.DynamicallyLoaded;
using OpenCV.Net;
using System.ComponentModel;

namespace EmotionalCities.Pupil
{
    /// <summary>
    /// Represents an operator that decodes a sequence of binary encoded video frames.
    /// </summary>
    [Description("Decodes a sequence of binary encoded video frames.")]
    public class DecodeVideoFrame : Transform<byte[], IplImage>
    {
        /// <summary>
        /// Gets or sets the path to the FFmpeg binaries used to decode the video frames.
        /// </summary>
        [Description("The path to the FFmpeg binaries used to decode the video frames.")]
        public string BinariesPath { get; set; }

        /// <summary>
        /// Decodes an observable sequence of binary encoded video frames.
        /// </summary>
        /// <param name="source">
        /// The sequence of video frames encoded as raw binary data.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="IplImage"/> objects representing the
        /// decoded frames.
        /// </returns>
        public unsafe override IObservable<IplImage> Process(IObservable<byte[]> source)
        {
            var decoder = new VideoDecoder(BinariesPath);
            return source.Select(decoder.DecodeFrame);
        }
    }

    unsafe class VideoDecoder
    {
        AVCodecID codecID;
        AVCodec* pAvCodec;
        AVCodecContext* pCodecContext;

        public VideoDecoder(string binariesPath)
        {
            DynamicallyLoadedBindings.LibrariesPath = binariesPath;
            DynamicallyLoadedBindings.Initialize();

            codecID = AVCodecID.AV_CODEC_ID_H264;
            pAvCodec = ffmpeg.avcodec_find_decoder(codecID);
            pCodecContext = ffmpeg.avcodec_alloc_context3(pAvCodec);
            int openResult = ffmpeg.avcodec_open2(pCodecContext, pAvCodec, null);
        }

        public unsafe IplImage DecodeFrame(byte[] frame)
        {
            fixed (byte* _frame = frame)
            {
                AVPacket packet;
                ffmpeg.av_init_packet(&packet);

                int packetResult = ffmpeg.av_packet_from_data(&packet, _frame, frame.Length);
                int sendPacketResult = ffmpeg.avcodec_send_packet(pCodecContext, &packet);

                AVFrame* decodedFrame = ffmpeg.av_frame_alloc();
                int decodeFrameResult = ffmpeg.avcodec_receive_frame(pCodecContext, decodedFrame);

                if (decodeFrameResult == 0) 
                {
                    AVFrame sourceFrame = *decodedFrame;
                    AVPixelFormat pixelFormat = pCodecContext->pix_fmt;

                    System.Drawing.Size frameSize = new System.Drawing.Size(sourceFrame.width, sourceFrame.height);

                    VideoFrameConverter vfc = new VideoFrameConverter(
                        frameSize,
                        pixelFormat,
                        frameSize,
                        AVPixelFormat.AV_PIX_FMT_BGR24
                    );

                    AVFrame convertedFrame = vfc.Convert(sourceFrame);

                    IplImage image = new IplImage(new OpenCV.Net.Size(convertedFrame.width, convertedFrame.height), IplDepth.U8, 3, (IntPtr)convertedFrame.data[0]);
                    IplImage imageCopy = image.Clone();

                    vfc.Dispose();

                    // Free packets/frames
                    ffmpeg.av_frame_unref(decodedFrame);
                    ffmpeg.av_freep(decodedFrame);

                    return imageCopy;
                }

                return null;
            }
        }
    }
}
