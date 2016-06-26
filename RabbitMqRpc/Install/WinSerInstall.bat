@ECHO Installing Service...
@SET PATH=%PATH%;C:\Windows\Microsoft.NET\Framework\v4.0.30319
@SET SERADR=%CD%
@InstallUtil /LogToConsole=true "%~dp0WinSer.exe"
@ECHO Install Done.
@pause