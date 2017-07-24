using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;

namespace Wardship.Logger
{
    public interface ITelemetryLogger
    {
        void LogError(Exception exception, string message);

        void TrackTrace(string message, SeverityLevel level, IDictionary<string, string> properties);
    }
}
