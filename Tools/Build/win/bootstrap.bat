::
:: Copyright (c) 2022-2023 Carbon Community 
:: All rights reserved
::
@echo off

echo   ______ _______ ______ ______ _______ _______ 
echo  ^|      ^|   _   ^|   __ \   __ \       ^|    ^|  ^|
echo  ^|   ---^|       ^|      ^<   __ ^<   -   ^|       ^|
echo  ^|______^|___^|___^|___^|__^|______/_______^|__^|____^|
echo                           discord.gg/carbonmod
echo.

pushd %~dp0..\..\..
set BOOTSTRAP_ROOT=%CD%
popd

rem Handle git Hooks
xcopy /s /i /y "%BOOTSTRAP_ROOT%\.githooks\*" "%BOOTSTRAP_ROOT%\.git\hooks" > NUL
echo * Updated - git hooks.

rem Inits and downloads the submodules
git -C "%BOOTSTRAP_ROOT%" submodule init
git -C "%BOOTSTRAP_ROOT%" submodule update
git -C "%BOOTSTRAP_ROOT%" submodule foreach git checkout
git -C "%BOOTSTRAP_ROOT%" submodule foreach git pull

cd %BOOTSTRAP_ROOT%
for /f %%i in ('git branch --show-current') do set CURRENT_BRANCH=%%i

echo * Handling component submodules..
FOR %%P IN (Carbon.Core/Carbon.Components/Carbon.Bootstrap Carbon.Core/Carbon.Components/Carbon.Common Carbon.Core/Carbon.Components/Carbon.Common.Client Carbon.Core/Carbon.Components/Carbon.Compat Carbon.Core/Carbon.Components/Carbon.Modules Carbon.Core/Carbon.Components/Carbon.Preloader Carbon.Core/Carbon.Components/Carbon.SDK Carbon.Core/Carbon.Hooks/Carbon.Hooks.Base Carbon.Core/Carbon.Hooks/Carbon.Hooks.Oxide Carbon.Core/Carbon.Hooks/Carbon.Hooks.Community) DO (
	echo ** Updating '%%P'
	cd %BOOTSTRAP_ROOT%/%%P
	git checkout %CURRENT_BRANCH% > NUL
	git fetch > NUL
	git pull > NUL
	echo    done.
)
echo * Finished - handling component submodules.

echo * Building submodules..
FOR %%O IN (DepotDownloader) DO (
	echo ** Build '%%O'
	dotnet restore "%BOOTSTRAP_ROOT%\Tools\%%O" --verbosity quiet --nologo --force > NUL
	dotnet clean   "%BOOTSTRAP_ROOT%\Tools\%%O" --verbosity quiet --configuration Release --nologo > NUL
	dotnet build   "%BOOTSTRAP_ROOT%\Tools\%%O" --verbosity quiet --configuration Release --no-restore --no-incremental > NUL
	echo    done.
)
echo * Finsihed - building submodules.

rem Download Rust binary libs
call "%~dp0\update.bat" %1

rem Don't track changes to this file
git -C "%BOOTSTRAP_ROOT%" update-index --assume-unchanged "%BOOTSTRAP_ROOT%\Tools\Helpers\doorstop_config.ini"
