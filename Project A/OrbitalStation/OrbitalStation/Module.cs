using System;
using System.Collections.Generic;
using System.Text;

namespace OrbitalStation;

public class Module : IMonitorable
{
    private readonly List<ResourceTank> _tanks = new();

    public string Name => throw new NotImplementedException();

    public ModuleType Type => throw new NotImplementedException();

    public double Volume => throw new NotImplementedException();

    public IReadOnlyList<ResourceTank> Tanks => throw new NotImplementedException();

    internal Module(string name, ModuleType type, double volume) => throw new NotImplementedException();

    public ResourceTank AddTank(string resourceName, double capacity, double alertThresholdPercent = 20.0) => throw new NotImplementedException();

    public ResourceTank? FindTank(string resourceName) => throw new NotImplementedException();

    public bool HasAlert() => throw new NotImplementedException();

    public string GetStatusReport() => throw new NotImplementedException();
}
