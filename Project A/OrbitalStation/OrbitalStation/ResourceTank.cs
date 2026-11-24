using System;
using System.Collections.Generic;
using System.Text;

namespace OrbitalStation;

public class ResourceTank : IMonitorable
{
    public string ResourceName => throw new NotImplementedException();

    public double Capacity => throw new NotImplementedException();

    public double CurrentLevel => throw new NotImplementedException();

    public double AlertThresholdPercent => throw new NotImplementedException();

    internal ResourceTank(string resourceName, double capacity, double alertThresholdPercent = 20.0)
        => throw new NotImplementedException();

    public void Consume(double amount) => throw new NotImplementedException();

    public void Refill(double amount) => throw new NotImplementedException();

    public double GetFillPercentage() => throw new NotImplementedException();

    public bool HasAlert() => throw new NotImplementedException();

    public string GetStatusReport() => throw new NotImplementedException();
}

