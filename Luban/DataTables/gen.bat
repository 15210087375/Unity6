set WORKSPACE=..
set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t client ^
    -c cs-simple-json ^
    -c cs-bin ^
    -d json ^
    -d bin ^
    --conf %CONF_ROOT%\luban.conf ^
    -x cs-simple-json.outputCodeDir=..\Code\Json ^
    -x cs-bin.outputCodeDir=..\..\ClientFrame\Assets\Game\Scripts\DataTable ^
    -x json.outputDataDir=..\..\ClientFrame\Assets\DataTable\Json ^
    -x bin.outputDataDir=..\..\ClientFrame\Assets\Res\Table

pause