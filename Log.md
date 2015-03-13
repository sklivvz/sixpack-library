http://sites.google.com/site/sixpacklibrary/_/rsrc/1237036619924/Home/sixpack-icon.gif?height=48&width=48
# Logging #

This page gives a basic example of logging with the Sixpack log class. Log can be redirected to file or email.
The log class supports log file rolling, meaning that a new file can be created based on time.
Furthermore log can be sorted to different sinks (mail or file) based on severity (log level).

## Log to file ##

```
using System;
using SixPack.Diagnostics;

namespace Sixpack.Samples
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Log.Instance.Add("Hello World!");
			Log.Instance.Add("Specify a level", LogLevel.Info);
			Log.Instance.AddFormat("This {0} {1} test!","is",'a');
			Log.Instance.AddFormat("This is another {0}!", "test", LogLevel.Critical);
			Log.Instance.HandleException(new InvalidOperationException("This is the third test!"));
			Log.Instance.HandleException(new Exception("This is test #4!"), LogLevel.Warning);
		} 
	}
}
```

You also need to specify some configuration:

```
<?xml version="1.0"?>
<configuration>
	<appSettings>
		<add key="FileLogLevel" value="31" />
		<add key="FileLogFile" value="log.cs.log" />
	</appSettings>
</configuration>
```

This is the output, in file `log.cs.log`:

```
20081129T110054.51 - UNKNOWN - log.exe - 0.0.0.0 - Info -  - Log Started
20081129T110054.53 - UNKNOWN - log.exe - 0.0.0.0 - Debug -  - Version: 1.0
20081129T110054.53 - UNKNOWN - log.exe - 0.0.0.0 - Debug -  - Copyright: (c) SixPack
20081129T110054.53 - UNKNOWN - log.exe - 0.0.0.0 - Debug -  - Release Date: 2008-11-08
20081129T110054.53 - UNKNOWN - log.exe - 0.0.0.0 - Debug -  - Hello World!
20081129T110054.53 - UNKNOWN - log.exe - 0.0.0.0 - Info -  - Specify a level
20081129T110054.53 - UNKNOWN - log.exe - 0.0.0.0 - Debug -  - This is a test!
20081129T110054.53 - UNKNOWN - log.exe - 0.0.0.0 - Critical -  - This is another test!
20081129T110054.53 - UNKNOWN - log.exe - 0.0.0.0 - Error -  - Exception occurred:
System.InvalidOperationException: This is the third test!
20081129T110054.53 - UNKNOWN - log.exe - 0.0.0.0 - Warning -  - Exception occurred:
System.Exception: This is test #4!
20081129T110054.53 - UNKNOWN - log.exe - 0.0.0.0 - Info -  - Log Stopped
```

## Formatting the log entries ##

The **FileLogFormat** value of appSettings controls the format of the log entries. This value may contain any text, along with placeholders that are replaced by values, according to the following table. The string is formatted using **String.Format**, therefore the standard .NET formatting options apply.

| **Placeholder** | **Description** |
|:----------------|:----------------|
| {0} | Current date. |
| {1} | Log level. |
| {2} | Message. |
| {3} | Name of the current Web server. |
| {4} | Friendly name of the current AppDomain. |
| {5} | IP address of the client. |
| {6} | The name of the current method. This value is only available with debug builds of the library. |

### Example ###

Web.config:

```
<?xml version="1.0"?>
<configuration>
	<appSettings>
		<add key="FileLogLevel" value="31" />
		<add key="FileLogFile" value="log.cs.log" />
		<add key="FileLogFormat" value="[{1}] {0:yyyyMMddTHHmmss.ff}: {2}" />
	</appSettings>
</configuration>
```

Log file:

```
[Info] 20081129T110054.51: Log Started
[Debug] 20081129T110054.53: Version: 1.0
[Debug] 20081129T110054.53: Version: Copyright: (c) SixPack
[Debug] 20081129T110054.53: Version: Release Date: 2008-11-08
[Debug] 20081129T110054.53: Hello World!
[Info] 20081129T110054.53: Specify a level
[Debug] 20081129T110054.53: This is a test!
[Critical] 20081129T110054.53: This is another test!
[Error] 20081129T110054.53: Exception occurred:
System.InvalidOperationException: This is the third test!
[Warning] 20081129T110054.53: Exception occurred:
System.Exception: This is test #4!
[Info] 20081129T110054.53: Log Stopped
```