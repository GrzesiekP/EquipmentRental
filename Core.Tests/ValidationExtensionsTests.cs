using Core.Extensions;

namespace Core.Tests;

[TestClass]
public class ValidationExtensionsTests
{
    [DataTestMethod]
    [DataRow("xd@xd.pl")]
    [DataRow("xd@xd.com")]
    [DataRow("xd@xd.com.pl")]
    [DataRow("es.xd@xd.com.pl")]
    public void ValidateValidEmail(string validEmail)
    {
        var result = validEmail.AssertIsValidEmail(nameof(validEmail));
        Assert.IsNotNull(result);
        Assert.AreEqual(validEmail, result);
    }
    
    [DataTestMethod]
    [DataRow("xdxd.pl")]
    [DataRow("xd@xd@com")]
    [DataRow("asdf")]
    [DataRow("  ")]
    public void ValidateInvalidEmail(string validEmail)
    {
        void Validation() => validEmail.AssertIsValidEmail(nameof(validEmail));
        Assert.ThrowsException<ArgumentException>((Action)Validation);
    }
}