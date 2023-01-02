using Bonsai;
using System;
using System.Linq;
using System.Reactive.Linq;
using NetMQ.Zyre;
using NetMQ.Zyre.ZyreEvents;
using NetMQ;

namespace PupilInterface
{
    public class ZyreNode : Source<ZyreEvent>
    {
        /// <summary>
        /// Gets or sets the Zyre node name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Zyre group to join.
        /// </summary>
        public string Group { get; set; }

        public override IObservable<ZyreEvent> Generate()
        {
            return Observable.Using(() =>
            {
                Zyre zyre = new Zyre(Name);
                zyre.Join(Group);
                zyre.Start();

                return zyre;
            },
            zyre =>
            {
                var enterObservable = Observable.FromEventPattern<ZyreEventEnter>(zyre, nameof(zyre.EnterEvent))
                    .Select(e =>
                    {
                        return new ZyreEvent { EventType = nameof(zyre.EnterEvent), FromNode = e.EventArgs.SenderName, Content = EmptyMessage() };
                    });

                var evasiveObservable = Observable.FromEventPattern<ZyreEventEvasive>(zyre, nameof(zyre.EvasiveEvent))
                    .Select(e =>
                    {
                        return new ZyreEvent { EventType = nameof(zyre.EvasiveEvent), FromNode = e.EventArgs.SenderName, Content = EmptyMessage() };
                    });

                var exitObservable = Observable.FromEventPattern<ZyreEventExit>(zyre, nameof(zyre.ExitEvent))
                    .Select(e =>
                    {
                        return new ZyreEvent { EventType = nameof(zyre.ExitEvent), FromNode = e.EventArgs.SenderName, Content = EmptyMessage() };
                    });

                var joinObservable = Observable.FromEventPattern<ZyreEventJoin>(zyre, nameof(zyre.JoinEvent))
                    .Select(e =>
                    {
                        return new ZyreEvent { EventType = nameof(zyre.JoinEvent), FromNode = e.EventArgs.SenderName, Content = EmptyMessage() };
                    });

                var leaveObservable = Observable.FromEventPattern<ZyreEventLeave>(zyre, nameof(zyre.LeaveEvent))
                    .Select(e =>
                    {
                        return new ZyreEvent { EventType = nameof(zyre.LeaveEvent), FromNode = e.EventArgs.SenderName, Content = EmptyMessage() };
                    });

                var whisperObservable = Observable.FromEventPattern<ZyreEventWhisper>(zyre, nameof(zyre.WhisperEvent))
                    .Select(e =>
                    {
                        return new ZyreEvent { EventType = nameof(zyre.WhisperEvent), FromNode = e.EventArgs.SenderName, Content = e.EventArgs.Content };
                    });

                var shoutObservable = Observable.FromEventPattern<ZyreEventShout>(zyre, nameof(zyre.ShoutEvent))
                    .Select(e =>
                    {
                        return new ZyreEvent { EventType = nameof(zyre.ShoutEvent), FromNode = e.EventArgs.SenderName, Content = e.EventArgs.Content };
                    });

                return enterObservable
                    .Merge(evasiveObservable)
                    .Merge(exitObservable)
                    .Merge(joinObservable)
                    .Merge(leaveObservable)
                    .Merge(whisperObservable)
                    .Merge(shoutObservable);
            });
        }

        private NetMQMessage EmptyMessage()
        {
            NetMQMessage emptyMessage = new NetMQMessage();
            emptyMessage.AppendEmptyFrame();
            return emptyMessage;
        }
    }

    public class ZyreEvent
    {
        public string EventType;
        public string FromNode;
        public NetMQMessage Content;
    }
}
