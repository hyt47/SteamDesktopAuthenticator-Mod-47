REM Remove un-needed CEF files
del /q "Steam Desktop Authenticator\bin\Release\devtools_resources.pak"
REM del /q "Steam Desktop Authenticator\bin\Release\cef.pak"
del /q "Steam Desktop Authenticator\bin\Release\cef_100_percent.pak"
del /q "Steam Desktop Authenticator\bin\Release\cef_200_percent.pak"
del /q "Steam Desktop Authenticator\bin\Release\cef_extensions.pak"
del /q "Steam Desktop Authenticator\bin\Release\d3dcompiler_47.dll"
del /q "Steam Desktop Authenticator\bin\Release\widevinecdmadapter.dll"

REM Remove CEF locale files
RD /S /Q "Steam Desktop Authenticator\bin\Release\locales\"

REM Remove CEF debug files
del /q "Steam Desktop Authenticator\bin\Release\CefSharp.BrowserSubprocess.Core.pdb"
del /q "Steam Desktop Authenticator\bin\Release\CefSharp.BrowserSubprocess.pdb"
del /q "Steam Desktop Authenticator\bin\Release\CefSharp.Core.pdb"
del /q "Steam Desktop Authenticator\bin\Release\CefSharp.pdb"
del /q "Steam Desktop Authenticator\bin\Release\CefSharp.WinForms.pdb"

REM Remove XML files
del /q "Steam Desktop Authenticator\bin\Release\Newtonsoft.Json.xml"
del /q "Steam Desktop Authenticator\bin\Release\CefSharp.xml"
del /q "Steam Desktop Authenticator\bin\Release\CefSharp.WinForms.xml"
del /q "Steam Desktop Authenticator\bin\Release\CefSharp.Core.xml"
del /q "Steam Desktop Authenticator\bin\Release\CommandLine.xml"

REM Remove Dev app
del /q "Steam Desktop Authenticator\bin\Release\Steam Desktop Authenticator 47.exe.config"
del /q "Steam Desktop Authenticator\bin\Release\Steam Desktop Authenticator 47.vshost.exe"
del /q "Steam Desktop Authenticator\bin\Release\Steam Desktop Authenticator 47.vshost.exe.config"
del /q "Steam Desktop Authenticator\bin\Release\Steam Desktop Authenticator 47.vshost.exe.manifest"
del /q "Steam Desktop Authenticator\bin\Release\SteamAuth.pdb"

REM Debug files / logs
del /q "Steam Desktop Authenticator\bin\Release\- debug pg.txt"
del /q "Steam Desktop Authenticator\bin\Release\debug.log"
