using System;
using System.Collections.Generic;
using GitVersion.Helpers;
using GitVersion.OutputVariables;

namespace GitVersion.BuildServers
{
    public class MyGet : BuildServerBase
    {
        public const string EnvironmentVariableName = "BuildRunner";
        protected override string EnvironmentVariable { get; } = EnvironmentVariableName;
        public override bool CanApplyToCurrentContext()
        {
            var buildRunner = Environment.GetEnvironmentVariable(EnvironmentVariable);

            return !string.IsNullOrEmpty(buildRunner)
                && buildRunner.Equals("MyGet", StringComparerUtils.IgnoreCaseComparison);
        }

        public override string[] GenerateSetParameterMessage(string name, string value)
        {
            var messages = new List<string>
            {
                $"##myget[setParameter name='GitVersion.{name}' value='{ServiceMessageEscapeHelper.EscapeValue(value)}']"
            };

            if (string.Equals(name, "LegacySemVerPadded", StringComparerUtils.IgnoreCaseComparison))
            {
                messages.Add($"##myget[buildNumber '{ServiceMessageEscapeHelper.EscapeValue(value)}']");
            }

            return messages.ToArray();
        }

        public override string GenerateSetVersionMessage(VersionVariables variables)
        {
            return null;
        }

        public override bool PreventFetch() => false;
    }
}
