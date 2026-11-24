using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrbitalStation.Tests;

[TestClass]
public class CrewMemberTests
{
    [TestMethod]
    public void Constructor_ValidArguments_SetsProperties()
    {
        var member = new CrewMember("Ivan", "Engineer");

        Assert.AreEqual("Ivan", member.Name);
        Assert.AreEqual("Engineer", member.Specialty);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void Constructor_EmptyName_ThrowsArgumentException(string name)
    {
        Assert.ThrowsExactly<ArgumentException>(() => new CrewMember(name, "Engineer"));
    }

    [TestMethod]
    public void NewMember_IsNotAssigned()
    {
        var member = new CrewMember("Ivan", "Engineer");

        Assert.IsFalse(member.IsAssigned());
        Assert.IsNull(member.CurrentModule);
    }
}