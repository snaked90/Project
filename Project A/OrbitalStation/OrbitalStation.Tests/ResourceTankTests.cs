using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace OrbitalStation.Tests;

[TestClass]
public class ResourceTankTests
{
    private static ResourceTank CreateTank(double capacity = 500, double thresholdPercent = 20.0)
    {
        var station = new Station("Orbit-1");
        var module = station.AddModule("Habitat-1", ModuleType.Habitat, 80);
        return module.AddTank("Oxygen", capacity, thresholdPercent);
    }

    [TestMethod]
    public void NewTank_IsFull()
    {
        var tank = CreateTank(500);

        Assert.AreEqual("Oxygen", tank.ResourceName);
        Assert.AreEqual(500.0, tank.Capacity);
        Assert.AreEqual(500.0, tank.CurrentLevel);
    }

    [TestMethod]
    public void Consume_ValidAmount_DecreasesLevel()
    {
        var tank = CreateTank(500);

        tank.Consume(100);

        Assert.AreEqual(400.0, tank.CurrentLevel);
    }

    [TestMethod]
    public void Consume_MoreThanAvailable_ThrowsInvalidOperation()
    {
        var tank = CreateTank(500);

        Assert.ThrowsExactly<InvalidOperationException>(() => tank.Consume(501));
    }

    [TestMethod]
    [DataRow(0.0)]
    [DataRow(-10.0)]
    public void Consume_NonPositiveAmount_ThrowsArgumentOutOfRange(double amount)
    {
        var tank = CreateTank();

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => tank.Consume(amount));
    }

    [TestMethod]
    public void Refill_ValidAmount_IncreasesLevel()
    {
        var tank = CreateTank(500);
        tank.Consume(200);

        tank.Refill(100);

        Assert.AreEqual(400.0, tank.CurrentLevel);
    }

    [TestMethod]
    public void Refill_ExceedsCapacity_ThrowsInvalidOperation()
    {
        var tank = CreateTank(500);
        tank.Consume(50);

        Assert.ThrowsExactly<InvalidOperationException>(() => tank.Refill(51));
    }

    [TestMethod]
    [DataRow(0.0)]
    [DataRow(-1.0)]
    public void Refill_NonPositiveAmount_ThrowsArgumentOutOfRange(double amount)
    {
        var tank = CreateTank();

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => tank.Refill(amount));
    }

    [TestMethod]
    public void GetFillPercentage_HalfFull_Returns50()
    {
        var tank = CreateTank(500);
        tank.Consume(250);

        Assert.AreEqual(50.0, tank.GetFillPercentage(), 0.001);
    }

    [TestMethod]
    public void HasAlert_FullTank_ReturnsFalse()
    {
        var tank = CreateTank(500);

        Assert.IsFalse(tank.HasAlert());
    }

    [TestMethod]
    public void HasAlert_BelowThreshold_ReturnsTrue()
    {
        var tank = CreateTank(500, 20.0);
        tank.Consume(410); // залишилось 90 (18%)

        Assert.IsTrue(tank.HasAlert());
    }

    [TestMethod]
    public void HasAlert_ExactlyAtThreshold_ReturnsFalse()
    {
        var tank = CreateTank(500, 20.0);
        tank.Consume(400); // залишилось 100 (рівно 20%)

        Assert.IsFalse(tank.HasAlert());
    }

    [TestMethod]
    public void GetStatusReport_ContainsResourceName()
    {
        var tank = CreateTank();

        StringAssert.Contains(tank.GetStatusReport(), "Oxygen");
    }
}

