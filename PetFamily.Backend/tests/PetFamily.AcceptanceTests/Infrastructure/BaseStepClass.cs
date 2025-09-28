using Reqnroll;

namespace PetFamily.AcceptanceTests.Infrastructure;

public abstract class BaseStepClass
{
    protected readonly TestContext _context;

    protected BaseStepClass(TestContext context)
    {
        _context = context;
    }

    protected AcceptanceTestBase TestBase => _context.TestBase ?? throw new InvalidOperationException("TestBase is not initialized. Make sure the test infrastructure is properly set up.");
}
