using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;
using Raven.Client;

#region RavenUnitOfWork
class RavenUnitOfWork : Behavior<PhysicalMessageProcessingContext>
{
    IDocumentSession session;

    public RavenUnitOfWork(IDocumentSession session)
    {
        this.session = session;
    }

    public override async Task Invoke(PhysicalMessageProcessingContext context, Func<Task> next)
    {
        await next();
        session.SaveChanges();
    }

    public class Registration:RegisterStep
    {
        public Registration() : base(
            "MyRavenDBUoW", 
            typeof(RavenUnitOfWork), 
            "Manages the RavenDB IDocumentSession for my handlers")
        {
        }
    }

}
#endregion
