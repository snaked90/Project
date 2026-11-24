using System;
using System.Collections.Generic;
using System.Text;

namespace OrbitalStation;

public interface IMonitorable
{
    bool HasAlert();

    string GetStatusReport();
}

