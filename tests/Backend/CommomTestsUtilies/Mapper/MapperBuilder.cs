using AutoMapper;
using Kanban.Application.AutoMapping;
using Microsoft.Extensions.Logging.Abstractions;

namespace CommomTestsUtilies.Mapper;

public static class MapperBuilder
{
    public static IMapper Build()
    {
        MapperConfiguration mapper = new(
            configure: config =>
            {
                config.AddProfile(profile: new AutoMapping());
            },
            loggerFactory: NullLoggerFactory.Instance);

        return mapper.CreateMapper();
    }
}