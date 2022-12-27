﻿using Microsoft.EntityFrameworkCore;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Domain.Common;
using MicroStore.Shipping.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace MicroStore.Shipping.Infrastructure.EntityFramework
{
    public class SettingsRepository : ISettingsRepository, ITransientDependency
    {

        private readonly ShippingDbContext _dbContext;

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()

            }
        };

        public SettingsRepository(ShippingDbContext dbContext, IJsonSerializer jsonSerilizar)
        {
            _dbContext = dbContext;
        }

        public async Task<T?> TryToGetSettings<T>(string providerKey, CancellationToken cancellationToken = default) where T : ISettings
        {
            var settings = await TryToGetSettings(providerKey,typeof(T), cancellationToken);

            return (T) settings ;
        }

        public async Task<object?> TryToGetSettings(string providerKey, Type type ,CancellationToken cancellationToken = default)
        {
            var settings = await _dbContext.Set<SettingsEntity>().SingleOrDefaultAsync(x => x.ProviderKey == providerKey, cancellationToken);

            if (settings == null)
            {
                settings = await InsertSettings(providerKey, cancellationToken);
            }

            return  JsonConvert.DeserializeObject(settings.Data,type,_settings);
        }

        public Task TryToUpdateSettings<T>(T settings, CancellationToken cancellationToken = default) where T : ISettings
        {
            return TryToUpdateSettings(settings, cancellationToken);
        }

        public async Task TryToUpdateSettrings(object settings, CancellationToken cancellationToken = default)
        {
            var convertedSettings = (ISettings)settings;

            var settingsEntity = await _dbContext.Set<SettingsEntity>().SingleOrDefaultAsync(x => x.ProviderKey == convertedSettings.ProviderKey, cancellationToken);

            if (settingsEntity == null)
            {
                settingsEntity = await InsertSettings(convertedSettings.ProviderKey, cancellationToken);
            }

            settingsEntity.Data = JsonConvert.SerializeObject(settings, _settings);


            _dbContext.Update(settingsEntity);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }


        private async Task<SettingsEntity> InsertSettings(string providerKey, CancellationToken cancellationToken)
        {
            var settings = new SettingsEntity(providerKey, string.Empty);

            await _dbContext.Set<SettingsEntity>().AddAsync(settings, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);


            return settings;
        }


    }
}