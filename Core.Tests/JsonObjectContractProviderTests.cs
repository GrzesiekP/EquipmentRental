using Core.EventStore;
using Marten;
using Orders.Aggregate.ValueObjects;
using Orders.Events;

namespace Core.Tests;

[TestClass]
public class JsonObjectContractProviderTests
{
    [TestMethod]
    public void ResolvingObjectType()
    {
        var contractObject = new OrderApproved(Guid.NewGuid());
        var resolver = new NonDefaultConstructorMartenJsonNetContractResolver(
            Casing.Default,
            CollectionStorage.Default,
            NonPublicMembersStorage.NonPublicSetters
        );

        var resolvedContract = resolver.ResolveContract(contractObject.GetType());
        
        Assert.AreEqual(contractObject.GetType().FullName, resolvedContract.CreatedType.FullName);
    }
    
    [TestMethod]
    public void ResolvingEnum()
    {
        const OrderStatus contractEnum = OrderStatus.Approved;
        var resolver = new NonDefaultConstructorMartenJsonNetContractResolver(
            Casing.Default,
            CollectionStorage.Default,
            NonPublicMembersStorage.NonPublicSetters
        );

        var resolvedContract = resolver.ResolveContract(contractEnum.GetType());
        
        Assert.AreEqual(contractEnum.GetType().FullName, resolvedContract.CreatedType.FullName);
    }
}