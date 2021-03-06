﻿using System;
using NHibernate.Cfg;
using NServiceBus;
using NServiceBus.Persistence;
using Environment = NHibernate.Cfg.Environment;

class Program
{
    static void Main()
    {
        var nhConfiguration = new Configuration();

        nhConfiguration.SetProperty(Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
        nhConfiguration.SetProperty(Environment.ConnectionDriver, "NHibernate.Driver.Sql2008ClientDriver");
        nhConfiguration.SetProperty(Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
        nhConfiguration.SetProperty(Environment.ConnectionStringName, "NServiceBus/Persistence");

        nhConfiguration = AddLoquaciousMappings(nhConfiguration);

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.CustomNhMappings.Loquacious");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();

        busConfiguration
            .UsePersistence<NHibernatePersistence>()
            .UseConfiguration(nhConfiguration);

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new StartOrder
            {
                OrderId = "123"
            });

            bus.SendLocal(new CompleteOrder
            {
                OrderId = "123"
            });

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }

    #region LoquaciousConfiguration
    static Configuration AddLoquaciousMappings(Configuration nhConfiguration)
    {
        var mapper = new NHibernate.Mapping.ByCode.ModelMapper();
        mapper.AddMappings(typeof(OrderSagaDataLoquacious).Assembly.GetTypes());
        nhConfiguration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
        return nhConfiguration;
    }
    #endregion
}
