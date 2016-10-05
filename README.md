# NServiceBus.SimpleStatistics

Get live throughput statistics of your NServiceBus endpoint performance.

## Version compatibility

| NServiceBus | NServiceBus.SimpleStatistics |
| ----------- | ---------------------------- |
| v5.x        | v1.x                         |

Please note that there might be versions targeting other NServiceBus versions. [Please check the Releases for all versions.](https://github.com/ramonsmits/NServiceBus.SimpleStatistics/releases) or [check the root of the default branch of the repository](https://github.com/ramonsmits/NServiceBus.SimpleStatistics).

## Features

The following features are provided:

- Show high level statistics in the console title
- Show detailed statistics in the NServiceBus log or configured NServiceBus logger

The detailed statistics shows statistics for various fixed periods.

The statistics are:

- Processed successfully (Success)
- Processing failures (Failure)
- Total retrieved messages (Total)

The fixed periods are:

- Average per second (`Avg`)
- Total since start  (`Tot`)
- Current completed interval  (`Cur`)
- Interval maximums (`Max`)

Note that the interval is configuration but it is not possible to define multiple custom reporting intervals.

## Log output example

The following is an example of the statistics written to the log at each reporting interval:

```txt
Avg> Success:        5 (100,00%), Failure:        0 (  0,00%) Total:        5 Period: 0:00:00:01,0000000 Duration: 198,90µs
Tot> Success:    1.479 (100,00%), Failure:        0 (  0,00%) Total:    1.479 Period: 0:00:04:15,1140000 Duration: 171,55µs
Cur> Success:        0 (  0,00%), Failure:        0 (  0,00%) Total:        0 Period: 0:00:00:01,0000000 Duration: NaNµs
Max> Success:       51 (100,00%), Failure:        0 (  0,00%) Total:       51 Period: 0:00:00:00,0000000 Duration: 141,87µs
Uptime: 00:04:15.1140000
```

Note that lines in log file are prefixed with timestamp and logger name.

## Installation

### NuGet

Install the NuGet package [NServiceBus.SimpleStatistics](https://www.nuget.org/packages/NServiceBus.SimpleStatistics)

    Install-Package NServiceBus.SimpleStatistics

### Drop in deployment

The assembly can be deployed without requiring to recompile. Recompile the process and the component will automatically be enabled by NServiceBus.

## Configuration

Settings can be configured via the `app.config` app settings;

```xml
  <appSettings>
    <add key="NServiceBus/SimpleStatistics/OutputConsoleTitle" value="True"/>
    <add key="NServiceBus/SimpleStatistics/OutputLog" value="True"/>
    <add key="NServiceBus/SimpleStatistics/IntervalMilliSeconds" value="15000"/>
  </appSettings>
```

### Report to console title

> Default: False (Disabled)

The `OutputConsoleTitle` settings controls if every report interval statistics will be updated in the console title.

The logic will append ` | Avg:{0:N}/s Last:{1:N}/s, Max:{2:N}/s` to the existing title.

#### Title format

Note that if the title is updated with a different name that the title will be reset with the value used at startup.

### Report to configured logger

> Default: True (Enabled)

The `OutputLog` settings controls if every report interval statistics will be logged.

### Reporting interval

> Default: 3600000 (Hourly)

The `IntervalMilliSeconds` setting controls the reporting interval.
