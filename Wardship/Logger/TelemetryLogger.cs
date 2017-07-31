using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;

namespace Wardship.Logger
{
    public class TelemetryLogger : ITelemetryLogger
    {
        private readonly TelemetryClient telemetryClient = new TelemetryClient();

        public void LogError(Exception exception, string message)
        {
            var telemetry = new ExceptionTelemetry(exception)
            {
                Message = message
            };

            telemetryClient.TrackException(telemetry);
        }

        public void TrackTrace(string message, SeverityLevel level, IDictionary<string, string> properties)
        {
            telemetryClient.TrackTrace(message, level, properties);
        }
    }
}