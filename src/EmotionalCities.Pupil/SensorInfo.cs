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
        /// The type of the pupil device sensor.
        /// </summary>
        public SensorType SensorType;

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

    /// <summary>
    /// Specifies the type of a device sensor.
    /// </summary>
    public enum SensorType
    {
        /// <summary>
        /// Specifies the sensor is a video capture device.
        /// </summary>
        Video,

        /// <summary>
        /// Specifies the sensor is a microphone or other audio capture device.
        /// </summary>
        Audio,

        /// <summary>
        /// Specifies the sensor is a motion sensor or other IMU-like device.
        /// </summary>
        Imu,

        /// <summary>
        /// The device type hosting all device-specific controls.
        /// </summary>
        Hardware,

        /// <summary>
        /// Specifies the touch screen or key press sensor.
        /// </summary>
        Key,

        /// <summary>
        /// Specifies the navigation system sensor for the device, if available. 
        /// </summary>
        Location,

        /// <summary>
        /// Specifies the gaze measurement sensor.
        /// </summary>
        Gaze,

        /// <summary>
        /// Specifies the mobile device interaction event sensor.
        /// </summary>
        Event,

        /// <summary>
        /// Specifies the LED sensor.
        /// </summary>
        Led
    }
}
