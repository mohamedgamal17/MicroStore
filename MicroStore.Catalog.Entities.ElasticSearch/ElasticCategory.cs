﻿namespace MicroStore.Catalog.Entities.ElasticSearch
{
    public class ElasticCategory : ElasticAuditedEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
