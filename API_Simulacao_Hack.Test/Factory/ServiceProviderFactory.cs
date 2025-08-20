using API_Simulacao_Hack.Interfaces.Repositories;
using API_Simulacao_Hack.Interfaces.Services;
using API_Simulacao_Hack.Models;
using API_Simulacao_Hack.Repositories;
using API_Simulacao_Hack.Services;
using API_Simulacao_Hack.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace API_Contabil.Test.Factory
{
    public static class ServiceProviderFactory
    {
        public static IServiceProvider CreateServiceProviderWithConnection()
        {
            var services = new ServiceCollection();

            services.AddScoped<ISimulacaoService, SimulacaoService>();
            services.AddScoped<IEventHubService, EventHubService>();
            services.AddSingleton<ITelemetriaService, TelemetriaService>();

            services.AddScoped<ISimulacaoRepository, SimulacaoRepository>();

            services.AddScoped<SolicitacaoSimulacaoValidator>();

            IConfiguration _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            services.AddDbContext<DbHack>(
                options =>
                {
                    options.UseSqlServer(_configuration.GetConnectionString("DbHack"),
                    providerOptions => providerOptions.EnableRetryOnFailure());
                    options.UseLazyLoadingProxies();
                });

            services.AddDbContext<SimulacaoContext>(
                options =>
                {
                    options.UseSqlite("Data Source=simulacao.db");
                });

            services.AddLogging();

            services.AddSingleton(_configuration);

            services.AddHttpClient();

            return services.BuildServiceProvider();
        }



        public static IServiceProvider CreateServiceProviderWithMockSimulacao(Mock<ISimulacaoRepository> mockSimulacaoRepository)
        {
            var services = new ServiceCollection();

            services.AddScoped<ISimulacaoService, SimulacaoService>();
            services.AddScoped<IEventHubService, EventHubService>();
            services.AddSingleton<ITelemetriaService, TelemetriaService>();

            services.AddScoped<ISimulacaoRepository, SimulacaoRepository>();

            services.AddScoped<SolicitacaoSimulacaoValidator>();

            IConfiguration _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            services.AddDbContext<DbHack>(
                options =>
                {
                    options.UseSqlServer(_configuration.GetConnectionString("DbHack"),
                    providerOptions => providerOptions.EnableRetryOnFailure());
                    options.UseLazyLoadingProxies();
                });

            services.AddDbContext<SimulacaoContext>(
                options =>
                {
                    options.UseSqlite("Data Source=simulacao.db");
                });


            services.AddLogging();

            services.AddSingleton(_configuration);

            services.AddHttpClient();

            return services.BuildServiceProvider();
        }

        public static IServiceProvider CreateServiceProviderWithMockSimulacaoService(Mock<ISimulacaoService> mockSimulacaoService)
        {
            var services = new ServiceCollection();

            services.AddSingleton(mockSimulacaoService.Object);

            services.AddScoped<IEventHubService, EventHubService>();
            services.AddSingleton<ITelemetriaService, TelemetriaService>();

            services.AddScoped<ISimulacaoRepository, SimulacaoRepository>();

            services.AddScoped<SolicitacaoSimulacaoValidator>();

            IConfiguration _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            services.AddDbContext<DbHack>(
                options =>
                {
                    options.UseSqlServer(_configuration.GetConnectionString("DbHack"),
                    providerOptions => providerOptions.EnableRetryOnFailure());
                    options.UseLazyLoadingProxies();
                });

            services.AddDbContext<SimulacaoContext>(
                options =>
                {
                    options.UseSqlite("Data Source=simulacao.db");
                });

            services.AddLogging();

            services.AddSingleton(_configuration);

            services.AddHttpClient();

            return services.BuildServiceProvider();
        }

        public static IServiceProvider CreateServiceProviderWithMockEventHubService(Mock<IEventHubService> mockEventHubService)
        {
            var services = new ServiceCollection();

            services.AddSingleton(mockEventHubService.Object);

            services.AddScoped<ISimulacaoService, SimulacaoService>();
            services.AddSingleton<ITelemetriaService, TelemetriaService>();

            services.AddScoped<ISimulacaoRepository, SimulacaoRepository>();

            services.AddScoped<SolicitacaoSimulacaoValidator>();

            IConfiguration _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            services.AddDbContext<DbHack>(
                options =>
                {
                    options.UseSqlServer(_configuration.GetConnectionString("DbHack"),
                    providerOptions => providerOptions.EnableRetryOnFailure());
                    options.UseLazyLoadingProxies();
                });

            services.AddDbContext<SimulacaoContext>(
                options =>
                {
                    options.UseSqlite("Data Source=simulacao.db");
                });

            services.AddLogging();

            services.AddSingleton(_configuration);

            services.AddHttpClient();

            return services.BuildServiceProvider();
        }

    }
}
