%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe E:\working\BasicService\Dyd.BaseService.Monitor\Dyd.BaseService.Monitor.Collect.WinService\bin\Debug\Dyd.BaseService.Monitor.Collect.WinService.exe
Net Start Dyd.BaseService.Monitor.Collect.WinService
sc config Dyd.BaseService.Monitor.Collect.WinService start= auto