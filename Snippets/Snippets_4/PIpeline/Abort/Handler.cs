﻿namespace Snippets4.Pipeline.Abort
{
    using NServiceBus;

    #region AbortHandler

    class Handler : IHandleMessages<MyMessage>
    {
        IBus bus;

        public Handler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MyMessage message)
        {
            // you may also want to log a reason here
            bus.DoNotContinueDispatchingCurrentMessageToHandlers();
        }
    }

    #endregion
}
