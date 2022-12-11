using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;

public class Options
{
    static readonly string OutputConsoleTitleKey = "OutputConsoleTitle";
    static readonly string OutputLogKey = "OutputLog";
    static readonly string IntervalMilliSecondsKey = "IntervalMilliSeconds";

    public bool? OutputTitle { get; set; }
    public bool? OutputLog { get; set; }
    public int? UpdateInMilliSeconds { get; set; } = 3600000; // Every hour

    internal Options() { }

    bool Enabled => (OutputTitle.GetValueOrDefault() || OutputLog.GetValueOrDefault()) && UpdateInMilliSeconds.GetValueOrDefault() > 0;

    internal Options ImportOverride(IConfiguration configuration)
    {
        const string TrueString = "True";

        // ConfigurationManager

        const string AppSettingPrefix = "NServiceBus/SimpleStatistics/";

        var appSettings = ConfigurationManager.AppSettings;

        if (string.Equals(appSettings[AppSettingPrefix + OutputConsoleTitleKey], TrueString, StringComparison.InvariantCultureIgnoreCase))
        {
            OutputTitle = true;
        }
        if (string.Equals(appSettings[AppSettingPrefix + OutputLogKey], TrueString, StringComparison.InvariantCultureIgnoreCase))
        {
            OutputLog = true;
        }
        if (int.TryParse(appSettings[AppSettingPrefix + IntervalMilliSecondsKey], out var interval))
        {
            UpdateInMilliSeconds = interval;
        }

        // IConfiguration
        const string ConfigPrefix = "NServiceBus.SimpleStatistics:";


        var outputConsoleTitleCfgValue = configuration[ConfigPrefix + OutputConsoleTitleKey];
        var outputLogCfgValue = configuration[ConfigPrefix + OutputLogKey];
        var intervalMilliSecondsCfgValue = configuration[ConfigPrefix + IntervalMilliSecondsKey];

        if (outputConsoleTitleCfgValue == TrueString)
        {
            OutputTitle = true;
        }
        if (outputLogCfgValue == TrueString)
        {
            OutputLog = true;
        }
        if (int.TryParse(intervalMilliSecondsCfgValue, out interval))
        {
            UpdateInMilliSeconds = interval;
        }

        return this;
    }
}