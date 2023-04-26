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
        internal static void RegisterFFmpegBinaries(string binariesPath)
        {
            DynamicallyLoadedBindings.LibrariesPath = binariesPath;
        }
    }
}
