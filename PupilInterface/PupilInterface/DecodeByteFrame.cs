using Bonsai;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using FFmpeg.AutoGen.Abstractions;
using FFmpeg.AutoGen.Bindings.DynamicallyLoaded;

namespace PupilInterface
{
    public class DecodeByteFrame : Transform<byte[], AVFrame>
    {
        public override IObservable<AVFrame> Process(IObservable<byte[]> source)
        {
            var decoder = new VideoDecoder();

            return source.Select(val =>
            {
                return decoder.DecodeFrame(val);
            });
        }
    }

    public unsafe class VideoDecoder
    {
        AVCodecID codecID;
        AVCodec* pAvCodec;
        AVCodecContext* pCodecContext;

        public VideoDecoder()
        {
            FFmpegBinariesHelper.RegisterFFmpegBinaries(); // TODO - this sort of stuff should probably go in some general resource node
            DynamicallyLoadedBindings.Initialize();

            codecID = AVCodecID.AV_CODEC_ID_H264;
            pAvCodec = ffmpeg.avcodec_find_decoder(codecID);
            pCodecContext = ffmpeg.avcodec_alloc_context3(pAvCodec);
            int openResult = ffmpeg.avcodec_open2(pCodecContext, pAvCodec, null);
        }

        public unsafe AVFrame DecodeFrame(byte[] frame)
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

                    Size frameSize = new Size(sourceFrame.width, sourceFrame.height);

                    VideoFrameConverter vfc = new VideoFrameConverter(
                        frameSize,
                        pixelFormat,
                        frameSize,
                        AVPixelFormat.AV_PIX_FMT_BGR24
                    );

                    AVFrame convertedFrame = vfc.Convert(sourceFrame);

                    vfc.Dispose();

                    // Free packets/frames
                    ffmpeg.av_frame_unref(decodedFrame);
                    ffmpeg.av_freep(decodedFrame);

                    return convertedFrame;
                }

                return new AVFrame();
            }
        }
    }
}
