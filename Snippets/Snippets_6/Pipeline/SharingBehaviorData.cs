namespace Snippets6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;

#region SharingBehaviorData

    public class ParentBehavior : Behavior<PhysicalMessageProcessingContext>
    {
        public override async Task Invoke(PhysicalMessageProcessingContext context, Func<Task> next)
        {
            // set some shared information on the context
            context.Set(new SharedData());

            await next();
        }
    }

    public class ChildBehavior : Behavior<LogicalMessageProcessingContext>
    {
        public override async Task Invoke(LogicalMessageProcessingContext context, Func<Task> next)
        {
            // access the shared data
            SharedData data = context.Get<SharedData>();

            await next();
        }
    }

#endregion

    public class SharedData
    {
    }
}