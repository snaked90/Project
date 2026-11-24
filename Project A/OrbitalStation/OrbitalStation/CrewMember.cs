using System;
using System.Collections.Generic;
using System.Text;

namespace OrbitalStation;

public class CrewMember
{
    public string Name => throw new NotImplementedException();

    public string Specialty => throw new NotImplementedException();

    public Module? CurrentModule
    {
        get => throw new NotImplementedException();
        internal set => throw new NotImplementedException();
    }

    public CrewMember(string name, string specialty) => throw new NotImplementedException();

    public bool IsAssigned() => throw new NotImplementedException();
}
