using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrbitalStation.Tests;

[TestClass]
public class StationTests
{
    private static Station CreateStation() => new("Orbit-1");

    [TestMethod]
    public void Constructor_ValidName_SetsNameAndStartsEmpty()
    {
        var station = CreateStation();

        Assert.AreEqual("Orbit-1", station.Name);
        Assert.AreEqual(0, station.Modules.Count);
        Assert.AreEqual(0, station.Crew.Count);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void Constructor_EmptyName_ThrowsArgumentException(string name)
    {
        Assert.ThrowsExactly<ArgumentException>(() => new Station(name));
    }

    [TestMethod]
    public void AddModule_ValidArguments_ModuleIsPartOfStation()
    {
        var station = CreateStation();

        var module = station.AddModule("Habitat-1", ModuleType.Habitat, 80);

        Assert.AreEqual(1, station.Modules.Count);
        Assert.AreSame(module, station.Modules[0]);
        Assert.AreEqual("Habitat-1", module.Name);
        Assert.AreEqual(ModuleType.Habitat, module.Type);
        Assert.AreEqual(80.0, module.Volume);
    }

    [TestMethod]
    [DataRow(0.0)]
    [DataRow(-5.0)]
    public void AddModule_NonPositiveVolume_ThrowsArgumentOutOfRange(double volume)
    {
        var station = CreateStation();

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(
            () => station.AddModule("Habitat-1", ModuleType.Habitat, volume));
    }

    [TestMethod]
    public void AddModule_DuplicateName_ThrowsInvalidOperation()
    {
        var station = CreateStation();
        station.AddModule("Habitat-1", ModuleType.Habitat, 80);

        Assert.ThrowsExactly<InvalidOperationException>(
            () => station.AddModule("Habitat-1", ModuleType.Laboratory, 40));
    }

    [TestMethod]
    public void GetTotalVolume_SumsAllModules()
    {
        var station = CreateStation();
        station.AddModule("Habitat-1", ModuleType.Habitat, 80);
        station.AddModule("Lab-1", ModuleType.Laboratory, 40);

        Assert.AreEqual(120.0, station.GetTotalVolume());
    }

    [TestMethod]
    public void GetTotalVolume_NoModules_ReturnsZero()
    {
        Assert.AreEqual(0.0, CreateStation().GetTotalVolume());
    }

    [TestMethod]
    public void AddCrewMember_ValidMember_IsAddedToCrew()
    {
        var station = CreateStation();
        var member = new CrewMember("Ivan", "Engineer");

        station.AddCrewMember(member);

        Assert.AreEqual(1, station.Crew.Count);
        Assert.AreSame(member, station.Crew[0]);
    }

    [TestMethod]
    public void AddCrewMember_Null_ThrowsArgumentNull()
    {
        var station = CreateStation();

        Assert.ThrowsExactly<ArgumentNullException>(() => station.AddCrewMember(null!));
    }

    [TestMethod]
    public void AddCrewMember_SameMemberTwice_ThrowsInvalidOperation()
    {
        var station = CreateStation();
        var member = new CrewMember("Ivan", "Engineer");
        station.AddCrewMember(member);

        Assert.ThrowsExactly<InvalidOperationException>(() => station.AddCrewMember(member));
    }

    [TestMethod]
    public void AssignToModule_ValidArguments_SetsCurrentModule()
    {
        var station = CreateStation();
        var module = station.AddModule("Habitat-1", ModuleType.Habitat, 80);
        var member = new CrewMember("Ivan", "Engineer");
        station.AddCrewMember(member);

        station.AssignToModule(member, module);

        Assert.AreSame(module, member.CurrentModule);
    }

    [TestMethod]
    public void AssignToModule_AnotherModule_ReplacesCurrentModule()
    {
        var station = CreateStation();
        var habitat = station.AddModule("Habitat-1", ModuleType.Habitat, 80);
        var lab = station.AddModule("Lab-1", ModuleType.Laboratory, 40);
        var member = new CrewMember("Ivan", "Engineer");
        station.AddCrewMember(member);
        station.AssignToModule(member, habitat);

        station.AssignToModule(member, lab);

        Assert.AreSame(lab, member.CurrentModule);
    }

    [TestMethod]
    public void AssignToModule_NullMember_ThrowsArgumentNull()
    {
        var station = CreateStation();
        var module = station.AddModule("Habitat-1", ModuleType.Habitat, 80);

        Assert.ThrowsExactly<ArgumentNullException>(() => station.AssignToModule(null!, module));
    }

    [TestMethod]
    public void AssignToModule_NullModule_ThrowsArgumentNull()
    {
        var station = CreateStation();
        var member = new CrewMember("Ivan", "Engineer");
        station.AddCrewMember(member);

        Assert.ThrowsExactly<ArgumentNullException>(() => station.AssignToModule(member, null!));
    }

    [TestMethod]
    public void AssignToModule_MemberNotOnStation_ThrowsInvalidOperation()
    {
        var station = CreateStation();
        var module = station.AddModule("Habitat-1", ModuleType.Habitat, 80);
        var stranger = new CrewMember("Petro", "Pilot");

        Assert.ThrowsExactly<InvalidOperationException>(() => station.AssignToModule(stranger, module));
    }

    [TestMethod]
    public void AssignToModule_ModuleFromAnotherStation_ThrowsInvalidOperation()
    {
        var station = CreateStation();
        var other = new Station("Orbit-2");
        var foreignModule = other.AddModule("Lab-X", ModuleType.Laboratory, 40);
        var member = new CrewMember("Ivan", "Engineer");
        station.AddCrewMember(member);

        Assert.ThrowsExactly<InvalidOperationException>(() => station.AssignToModule(member, foreignModule));
    }

    [TestMethod]
    public void GetActiveAlerts_NoAlerts_ReturnsEmpty()
    {
        var station = CreateStation();
        var module = station.AddModule("Habitat-1", ModuleType.Habitat, 80);
        module.AddTank("Oxygen", 500);

        Assert.AreEqual(0, station.GetActiveAlerts().Count);
    }

    [TestMethod]
    public void GetActiveAlerts_LowTank_ReturnsTankAndItsModule()
    {
        var station = CreateStation();
        var module = station.AddModule("Habitat-1", ModuleType.Habitat, 80);
        var oxygen = module.AddTank("Oxygen", 500);
        module.AddTank("Water", 300);
        oxygen.Consume(450); // 10% < 20%

        var alerts = station.GetActiveAlerts();

        Assert.AreEqual(2, alerts.Count);
        Assert.IsTrue(alerts.Contains((IMonitorable)oxygen));
        Assert.IsTrue(alerts.Contains((IMonitorable)module));
    }

    [TestMethod]
    public void GetActiveAlerts_OnlyProblemModuleIsReported()
    {
        var station = CreateStation();
        var healthy = station.AddModule("Lab-1", ModuleType.Laboratory, 40);
        healthy.AddTank("Water", 300);
        var problem = station.AddModule("Habitat-1", ModuleType.Habitat, 80);
        problem.AddTank("Oxygen", 500).Consume(480);

        var alerts = station.GetActiveAlerts();

        Assert.IsFalse(alerts.Contains((IMonitorable)healthy));
        Assert.IsTrue(alerts.Contains((IMonitorable)problem));
    }
}