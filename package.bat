@ECHO OFF

REM %1 is the sub version number ("x" in 1.x")

SETLOCAL ENABLEEXTENSIONS

RD /S /Q tmp 2> NUL
MD tmp
PUSHD tmp


REM Binary package

MD bin

FOR %%I IN (SixPack.Web.Services SixPack.Net.Mail SixPack.Caching SixPack) DO (
	XCOPY /E /Q ..\src\%%I\bin bin
)

COPY ..\src\LICENSE bin

SET KEYFILE=..\SixPack.pfx
SET ILMERGE=C:\Program Files\Microsoft\ILMerge\ILMerge.exe

IF EXIST "%KEYFILE%" (
	IF EXIST "%ILMERGE%" (
		FOR %%J IN (Debug Release) DO (
			FOR %%I IN (bin/%%J/*.dll) DO (
				ECHO Signing %%J\%%I
				rem REN bin\%%J\%%I _%%I
				rem "%ILMERGE%" bin\%%J\_%%I /keyfile:"%KEYFILE%" /out:bin\%%J\%%I
				rem DEL bin\%%J\_%%I
			)
		)
	) ELSE (
		ECHO To enable signing of the assemblies, install ILMERGE into "%ILMERGE%".
		ECHO Download ILMERGE from http://www.microsoft.com/downloads/details.aspx?familyid=22914587-b4ad-4eae-87cf-b14ae6a939b0&displaylang=en
	)
) ELSE (
	ECHO To enable signing of the assemblies, place a PFX file named "%KEYFILE%" in the root folder.
)

..\support\windows\7zip\7z.exe a ..\SixPack-bin-1.%1.zip bin


REM Source package

ECHO. > exclusions
ECHO \.svn\ >> exclusions
ECHO \bin\ >> exclusions
ECHO \obj\ >> exclusions
ECHO \_Resharper >> exclusions

XCOPY /I /E /Q /EXCLUDE:exclusions ..\src src

..\support\windows\7zip\7z.exe a ..\SixPack-src-1.%1.zip src


REM Help package

ECHO. > exclusions
ECHO \.svn\ >> exclusions
ECHO .chm >> exclusions

XCOPY /I /E /Q /EXCLUDE:exclusions ..\HelpTmp doc

..\support\windows\7zip\7z.exe a ..\SixPack-doc-html-1.%1.zip doc

REM changed number style from 1.x to 1-x because apparently some users were having problems...
COPY ..\HelpTmp\SixPack.chm ..\SixPack-1-%1.chm

POPD
RD /S /Q tmp

GOTO :EOF
