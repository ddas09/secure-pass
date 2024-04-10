using AutoMapper;

namespace SecurePass.UnitTests.Tests;

public class TestBase
{
    protected IMapper Mapper { get; set; }

    protected void ConfigureMapper(IEnumerable<Profile> profiles)
    {
        var config = new MapperConfiguration(cfg =>
        {
            foreach (var profile in profiles)
            {
                cfg.AddProfile(profile);
            }
        });

        this.Mapper =  config.CreateMapper();
    }
}

