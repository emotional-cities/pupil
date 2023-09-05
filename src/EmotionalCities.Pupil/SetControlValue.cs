using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Bonsai;
using NetMQ;

namespace EmotionalCities.Pupil
{
    /// <summary>
    /// Represents an operator that creates a sequence of messages to change the
    /// value of control properties in pupil device sensors.
    /// </summary>
    [Description("Creates a sequence of messages to change the value of control properties in pupil device sensors.")]
    public class SetControlValue : Combinator<NetMQMessage>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the device sensor containing
        /// the control property to update.
        /// </summary>
        [Description("The unique identifier of the device sensor containing the control property.")]
        public string SensorUuid { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the control property to update.
        /// </summary>
        [Description("The identifier of the control property to update.")]
        public string ControlId { get; set; }

        /// <summary>
        /// Gets or sets the value used to update the control property.
        /// </summary>
        [Description("The value used to update the control property.")]
        public string Value { get; set; }

        /// <summary>
        /// Creates an observable sequence of messages that can be used to change
        /// the value of control properties in pupil device sensors.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting new control messages.
        /// </param>
        /// <returns>
        /// The sequence of created <see cref="NetMQMessage"/> objects.
        /// </returns>
        public override IObservable<NetMQMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ =>
            {
                var message = new NetMQMessage(expectedFrameCount: 2);
                message.Append(SensorUuid);
                message.Append(
                    $"{{" +
                    $@"""action"": ""set_control_value"", " +
                    $@"""control_id"": ""{ControlId}"", " +
                    $@"""value"": ""{Value}""" +
                    $"}}");
                return message;
            });
        }
    }
}
