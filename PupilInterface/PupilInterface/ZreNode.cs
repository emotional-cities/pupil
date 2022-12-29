using Bonsai;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using NetMQ.Zyre;
using System.Reactive.Disposables;
using NetMQ.Zyre.ZyreEvents;
using NetMQ;

namespace PupilInterface
{
    public class ZreNode : Source<ZyreEventWhisper>
    {
        /// <summary>
        /// Gets or sets the Zyre node name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Zyre group to join.
        /// </summary>
        public string JoinGroup { get; set; }

        public override IObservable<ZyreEventWhisper> Generate()
        {
            Zyre zyre = new Zyre(Name);

            zyre.Join(JoinGroup);

            zyre.Start();

            return Observable.FromEventPattern<ZyreEventWhisper>(zyre, "WhisperEvent")
                .Select(e => e.EventArgs)
                .Finally(() => zyre.Dispose());
        }
    }
}
