using Application.Mappings;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace SharedTest.Fixtures
{
    public class MapperFixture
    {
        public IMapper Mapper { get; }

        public MapperFixture()
        {
            // ساخت یک LoggerFactory ساده
            var loggerFactory = LoggerFactory.Create(builder => { });

            // ساخت MapperConfiguration با LoggerFactory
            var config = new MapperConfiguration(
                cfg => cfg.AddProfile<CustomerProfile>(),
                loggerFactory
            );

            config.AssertConfigurationIsValid();
            Mapper = config.CreateMapper();
        }
    }
}
