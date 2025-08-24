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
            var options = new DbContextOptionsBuilder<SimulacaoContext>()
                .UseInMemoryDatabase("SimulacaoTestContext")
                .EnableServiceProviderCaching(false)
                .Options;

            var context = new SimulacaoContext(options);

            context.Database.EnsureCreated();

            return context;
        }

        public static DbHack GetInMemoryContextDbHackContext()
        {
            var options = new DbContextOptionsBuilder<DbHack>()
                .UseInMemoryDatabase("DbHackTestContext")
                .Options;

            var context = new DbHack(options);

            context.Database.EnsureCreated();

            return context;
        }
    }
}
