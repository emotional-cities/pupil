using FFmpeg.AutoGen.Bindings.DynamicallyLoaded;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PupilInterface
{
    public class FFmpegBinariesHelper
    {
        internal static void RegisterFFmpegBinaries()
        {
            var binariesPath = "C:\\Users\\neurogears\\source\\repos\\FFmpeg.AutoGen\\FFmpeg\\bin\\x64"; // TODO - must find dynamically based on environment
            DynamicallyLoadedBindings.LibrariesPath = binariesPath;

            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //{
            //    var current = Environment.CurrentDirectory;
            //    var probe = Path.Combine("FFmpeg", "bin", Environment.Is64BitProcess ? "x64" : "x86");

            //    while (current != null)
            //    {
            //        var ffmpegBinaryPath = Path.Combine(current, probe);

            //        if (Directory.Exists(ffmpegBinaryPath))
            //        {
            //            Console.WriteLine($"FFmpeg binaries found in: {ffmpegBinaryPath}");
            //            DynamicallyLoadedBindings.LibrariesPath = ffmpegBinaryPath;
            //            return;
            //        }

            //        current = Directory.GetParent(current)?.FullName;
            //    }
            //}
            //else
            //    throw new NotSupportedException(); // fell free add support for platform of your choose
        }
    }
}
