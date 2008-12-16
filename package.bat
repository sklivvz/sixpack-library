@ECHO OFF

SETLOCAL ENABLEEXTENSIONS

RD /S /Q tmp 2> NUL
MD tmp
PUSHD tmp


REM Binary package

MD bin

FOR %%I IN (SixPack.Web.Services SixPack.Net.Mail SixPack) DO (
	XCOPY /E ..\src\%%I\bin bin
)

COPY ..\src\LICENSE bin

..\support\windows\7zip\7z.exe a ..\SixPack-bin-1.0.zip bin


REM Source package

ECHO. > exclusions
ECHO \.svn\ >> exclusions
ECHO \bin\ >> exclusions
ECHO \obj\ >> exclusions

XCOPY /I /E /EXCLUDE:exclusions ..\src src

..\support\windows\7zip\7z.exe a ..\SixPack-src-1.0.zip src


REM Help package

ECHO. > exclusions
ECHO \.svn\ >> exclusions
ECHO .chm >> exclusions

XCOPY /I /E /EXCLUDE:exclusions ..\HelpTmp doc

..\support\windows\7zip\7z.exe a ..\SixPack-doc-html-1.0.zip doc

COPY ..\HelpTmp\SixPack.chm ..\SixPack-1.0.chm

POPD
RD /S /Q tmp

GOTO :EOF
