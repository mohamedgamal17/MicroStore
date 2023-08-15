﻿namespace MicroStore.Catalog.Domain.Configuration
{
    public class ApplicationSettings
    {
        public SecuritySettings Security { get; set; } = new SecuritySettings();
        public MassTransitSettings MassTransit { get; set; } = new MassTransitSettings();
        public ConnectionStringSettings ConnectionStrings { get; set; } = new ConnectionStringSettings();
        public ElasticSearchConfiguration ElasticSearch { get; set; } = new ElasticSearchConfiguration();
    }


}
