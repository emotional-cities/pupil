namespace EmotionalCities.Pupil
{
    /// <summary>
    /// Represents information about a pupil device sensor.
    /// </summary>
    public class SensorInfo
    {
        /// <summary>
        /// The friendly name of the sensor.
        /// </summary>
        public string SensorName;

        /// <summary>
        /// The universally unique identifier of the pupil device sensor.
        /// </summary>
        public string SensorUuid;

        /// <summary>
        /// The ZMQ end-point used to stream sensor data.
        /// </summary>
        public string DataEndpoint;

        /// <summary>
        /// The ZMQ end-point used to receive commands for configuring the
        /// pupil device sensor.
        /// </summary>
        public string CommandEndpoint;
    }
}
