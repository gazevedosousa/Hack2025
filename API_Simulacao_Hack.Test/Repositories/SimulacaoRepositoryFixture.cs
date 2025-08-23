using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Simulacao_Hack.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Simulacao_Hack.Test.Repositories
{
    public static class SimulacaoRepositoryFixture
    {
        public static SimulacaoContext GetInMemoryContextSimulacaoContext()
        {
            // Garante que cada teste tenha seu banco isolado (se quiser pode passar nome fixo)
            var options = new DbContextOptionsBuilder<SimulacaoContext>()
                .UseInMemoryDatabase("SimulacaoTestContext")
                .EnableServiceProviderCaching(false)
                .Options;

            var context = new SimulacaoContext(options);

            // Se quiser, já garante que o banco é criado
            context.Database.EnsureCreated();

            return context;
        }

        public static DbHack GetInMemoryContextDbHackContext()
        {
            // Garante que cada teste tenha seu banco isolado (se quiser pode passar nome fixo)
            var options = new DbContextOptionsBuilder<DbHack>()
                .UseInMemoryDatabase("DbHackTestContext")
                .Options;

            var context = new DbHack(options);

            // Se quiser, já garante que o banco é criado
            context.Database.EnsureCreated();

            return context;
        }
    }
}
