Rextester - windows (frontend) part (>=VS 2012, asp mvc 3). Also includes C#, F#, VB, VC++ and Sql Server running environments.

-GlobalUtils - passwords for services and some globally used functions

-Jobs - some jobs, like putting data to elastic search index

-reExp - main frontend project 

-Sandbox - console applications which wrapps user code. It creates little-privilleged appdomain in which user code is run.
Console application is itself monitored for resource consumption. Applies to C#, VB, F#. This is pretty secure.

-Service - api to linux languages and vc++

-SqlServer - console application used to run sql code

-WindowsEngineTests - some tests

-WindowsExecutionEngine - engine to run native code on windows, currently only for VC++. Main security measure is attachnig job object 
to process, which ensures termination. Now also user code is sandboxed through http://www.sandboxie.com/ . Seems secure.

-WindowsSandbox - exe that is sandboxed in sandboxie and which spawns usercode

-WindowsService - web service as an api to WindowsExecutionEngine.