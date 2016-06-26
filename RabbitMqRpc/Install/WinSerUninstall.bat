@ECHO Uninstalling Service...
@SET PATH=%PATH%;C:\Windows\Microsoft.NET\Framework\v4.0.30319
@installutil -u "%~dp0WinSer.exe"
@ECHO Done.
@pause