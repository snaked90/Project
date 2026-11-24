using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace OrbitalStation.Tests;

[TestClass]
public class ModuleTests
{
    private static Module CreateModule()
    {
        var station = new Station("Orbit-1");
        return station.AddModule("Habitat-1", ModuleType.Habitat, 80);
    }

    [TestMethod]
    public void AddTank_ValidArguments_TankIsPartOfModule()
    {
        var module = CreateModule();

        var tank = module.AddTank("Oxygen", 500);

        Assert.AreEqual(1, module.Tanks.Count);
        Assert.AreSame(tank, module.Tanks[0]);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void AddTank_EmptyResourceName_ThrowsArgumentException(string name)
    {
        var module = CreateModule();

        Assert.ThrowsExactly<ArgumentException>(() => module.AddTank(name, 500));
    }

    [TestMethod]
    [DataRow(0.0)]
    [DataRow(-100.0)]
    public void AddTank_NonPositiveCapacity_ThrowsArgumentOutOfRange(double capacity)
    {
        var module = CreateModule();

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => module.AddTank("Oxygen", capacity));
    }

    [TestMethod]
    public void AddTank_DuplicateResource_ThrowsInvalidOperation()
    {
        var module = CreateModule();
        module.AddTank("Oxygen", 500);

        Assert.ThrowsExactly<InvalidOperationException>(() => module.AddTank("Oxygen", 100));
    }

    [TestMethod]
    public void FindTank_ExistingResource_ReturnsTank()
    {
        var module = CreateModule();
        var tank = module.AddTank("Water", 300);

        Assert.AreSame(tank, module.FindTank("Water"));
    }

    [TestMethod]
    public void FindTank_MissingResource_ReturnsNull()
    {
        var module = CreateModule();

        Assert.IsNull(module.FindTank("Nitrogen"));
    }

    [TestMethod]
    public void HasAlert_NoTanks_ReturnsFalse()
    {
        var module = CreateModule();

        Assert.IsFalse(module.HasAlert());
    }

    [TestMethod]
    public void HasAlert_AllTanksAboveThreshold_ReturnsFalse()
    {
        var module = CreateModule();
        module.AddTank("Oxygen", 500);
        module.AddTank("Water", 300);

        Assert.IsFalse(module.HasAlert());
    }

    [TestMethod]
    public void HasAlert_AnyTankBelowThreshold_ReturnsTrue()
    {
        var module = CreateModule();
        module.AddTank("Water", 300);
        var oxygen = module.AddTank("Oxygen", 500);
        oxygen.Consume(450);

        Assert.IsTrue(module.HasAlert());
    }

    [TestMethod]
    public void GetStatusReport_ContainsModuleNameAndType()
    {
        var module = CreateModule();

        var report = module.GetStatusReport();

        StringAssert.Contains(report, "Habitat-1");
        StringAssert.Contains(report, "Habitat");
    }

    [TestMethod]
    public void Module_UsedThroughInterface_NoAlertInitially()
    {
        IMonitorable monitorable = CreateModule();

        Assert.IsFalse(monitorable.HasAlert());
    }
}

