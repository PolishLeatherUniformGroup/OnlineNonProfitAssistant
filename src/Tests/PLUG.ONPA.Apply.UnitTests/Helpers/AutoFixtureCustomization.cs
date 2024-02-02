using AutoFixture;

namespace PLUG.ONPA.Apply.UnitTests.Helpers;

public class DateOnlyFixtureCustomization: ICustomization
{

    void ICustomization.Customize(IFixture fixture)
    {
        fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
    }
}
