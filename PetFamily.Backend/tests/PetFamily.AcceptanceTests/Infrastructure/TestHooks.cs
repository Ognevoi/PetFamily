using Reqnroll;

namespace PetFamily.AcceptanceTests.Infrastructure;

[Binding]
public class TestHooks
{
    private readonly TestContext _context;

    public TestHooks(TestContext context)
    {
        _context = context;
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        var testBase = new TestInfrastructure();
        await testBase.InitializeAsync();
        _context.TestBase = testBase;
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        if (_context.TestBase != null)
        {
            await _context.TestBase.DisposeAsync();
        }
    }
}

// Concrete implementation of AcceptanceTestBase for testing
public class TestInfrastructure : AcceptanceTestBase
{
    // This class inherits all the functionality from AcceptanceTestBase
    // but can be instantiated directly for testing purposes
}
