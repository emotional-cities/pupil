using FFmpeg.AutoGen.Bindings.DynamicallyLoaded;

namespace EmotionalCities.Pupil
{
    public class FFmpegBinariesHelper
    {
        internal static void RegisterFFmpegBinaries(string binariesPath)
        {
            DynamicallyLoadedBindings.LibrariesPath = binariesPath;
        }
    }
}
