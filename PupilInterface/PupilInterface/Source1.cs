using Bonsai;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using NetMQ.Zyre;

namespace PupilInterface
{
    [Description("")]
    [Combinator(MethodName = nameof(Generate))]
    [WorkflowElementCategory(ElementCategory.Source)]
    public class Source1
    {
        private readonly Zyre zyre = new Zyre("test-zyre");

        private readonly Guid uuid;

        public IObservable<int> Generate()
        {
            zyre.JoinEvent += Zyre_JoinEvent;
            zyre.WhisperEvent += Zyre_WhisperEvent;
            zyre.ShoutEvent += Zyre_ShoutEvent;
            zyre.EnterEvent += Zyre_EnterEvent;

            zyre.Join("pupil-mobile-v4");

            Console.ReadLine();

            zyre.Start();

            return Observable.Return(0);
        }

        private void Zyre_EnterEvent(object sender, NetMQ.Zyre.ZyreEvents.ZyreEventEnter e)
        {
            Console.WriteLine(e.SenderName);
        }

        private void Zyre_ShoutEvent(object sender, NetMQ.Zyre.ZyreEvents.ZyreEventShout e)
        {
            Console.WriteLine("SHOUT");
            Console.WriteLine(e.SenderName);
        }

        private void Zyre_WhisperEvent(object sender, NetMQ.Zyre.ZyreEvents.ZyreEventWhisper e)
        {
            Console.WriteLine("WHISPER");
            Console.WriteLine(e.Content);
        }

        private void Zyre_JoinEvent(object sender, NetMQ.Zyre.ZyreEvents.ZyreEventJoin e)
        {
            Console.WriteLine(e.GroupName);

            foreach (var peer in zyre.Peers())
            {
                Console.WriteLine(peer);
            }
        }
    }
}
