﻿#nullable enable
using System;
using System.Threading;
using Marten;
using Marten.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;
using Weasel.Postgresql;

namespace Core
{
    public static class MartenConfigExtensions
    {
        private const string DefaultConfigKey = "EventStore";

        public static IServiceCollection AddMarten(this IServiceCollection services, MartenConfig martenConfig,
            Action<StoreOptions>? configureOptions = null, string configKey = DefaultConfigKey)
        {
            services
                .AddScoped<IIdGenerator, MartenIdGenerator>();

            var documentStore = services
                .AddMarten(options => { SetStoreOptions(options, martenConfig, configureOptions); })
                .InitializeStore();

            SetupSchema(documentStore, martenConfig, 1);

            return services;
        }
        
        private static void SetupSchema(IDocumentStore documentStore, MartenConfig martenConfig, int retryLeft = 1)
        {
            try
            {
                if (martenConfig.ShouldRecreateDatabase)
                    documentStore.Advanced.Clean.CompletelyRemoveAll();

                using (NoSynchronizationContextScope.Enter())
                {
                    documentStore.Schema.ApplyAllConfiguredChangesToDatabaseAsync().Wait();
                }
            }
            catch
            {
                if (retryLeft == 0) throw;

                Thread.Sleep(1000);
                SetupSchema(documentStore, martenConfig, --retryLeft);
            }
        }

        private static void SetStoreOptions(StoreOptions options, MartenConfig config,
            Action<StoreOptions>? configureOptions = null)
        {
            options.Connection(config.ConnectionString);
            options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;

            var schemaName = Environment.GetEnvironmentVariable("SchemaName");
            options.Events.DatabaseSchemaName = schemaName ?? config.WriteModelSchema;
            options.DatabaseSchemaName = schemaName ?? config.ReadModelSchema;


            var serializer = new JsonNetSerializer { EnumStorage = EnumStorage.AsString };
            serializer.Customize(s =>
            {
                s.ContractResolver = new NonDefaultConstructorMartenJsonNetContractResolver(
                    Casing.Default,
                    CollectionStorage.Default,
                    NonPublicMembersStorage.NonPublicSetters
                );
            });

            options.Serializer(serializer);

            options.Projections.AsyncMode = config.DaemonMode;

            configureOptions?.Invoke(options);
        }
    }
}