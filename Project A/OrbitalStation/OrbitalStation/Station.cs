using System;
using System.Collections.Generic;
using System.Text;

namespace OrbitalStation;

public class Station
{
    private readonly List<Module> _modules = new();
    private readonly List<CrewMember> _crew = new();

    public string Name => throw new NotImplementedException();

    public IReadOnlyList<Module> Modules => throw new NotImplementedException();

    public IReadOnlyList<CrewMember> Crew => throw new NotImplementedException();

    public Station(string name) => throw new NotImplementedException();

    public Module AddModule(string name, ModuleType type, double volume) => throw new NotImplementedException();

    public void AddCrewMember(CrewMember member) => throw new NotImplementedException();

    public void AssignToModule(CrewMember member, Module module) => throw new NotImplementedException();

    public IReadOnlyList<IMonitorable> GetActiveAlerts() => throw new NotImplementedException();

    public double GetTotalVolume() => throw new NotImplementedException();
}
