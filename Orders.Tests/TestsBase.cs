using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orders.Tests;

[TestClass]
public class TestsBase
{
    protected virtual void Given() { }

    protected virtual void When() { }

    [TestInitialize]
    public void Initialize()
    {
        Given();
        When();
    }
}