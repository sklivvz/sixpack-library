@ECHO OFF

SETLOCAL ENABLEEXTENSIONS

RD /S /Q tmp 2> NUL
MD tmp
PUSHD tmp


REM Binary package

MD bin

FOR %%I IN (SixPack.Web.Services SixPack.Net.Mail SixPack) DO (
	XCOPY /E /Q ..\src\%%I\bin bin
)

COPY ..\src\LICENSE bin

SET KEYFILE=..\SixPack.pfx
SET ILMERGE=..\support\windows\ILMerge\ILMerge.exe

IF EXIST "%KEYFILE%" (
	IF EXIST "%ILMERGE%" (
		FOR %%J IN (Debug Release) DO (
			FOR %%I IN (bin/%%J/*.dll) DO (
				ECHO Signing %%J\%%I
				REN bin\%%J\%%I _%%I
				"%ILMERGE%" bin\%%J\_%%I /keyfile:"%KEYFILE%" /out:bin\%%J\%%I
				DEL bin\%%J\_%%I
			)
		)
	) ELSE (
		ECHO To enable signing of the assemblies, install ILMERGE into "%ILMERGE%".
		ECHO Download ILMERGE from http://www.microsoft.com/downloads/details.aspx?familyid=22914587-b4ad-4eae-87cf-b14ae6a939b0&displaylang=en
	)
) ELSE (
	ECHO To enable signing of the assemblies, place a PFX file named "%KEYFILE%" in the root folder.
)

..\support\windows\7zip\7z.exe a ..\SixPack-bin-1.0.zip bin


REM Source package

ECHO. > exclusions
ECHO \.svn\ >> exclusions
ECHO \bin\ >> exclusions
ECHO \obj\ >> exclusions
ECHO \_Resharper >> exclusions

XCOPY /I /E /Q /EXCLUDE:exclusions ..\src src

..\support\windows\7zip\7z.exe a ..\SixPack-src-1.0.zip src


REM Help package

ECHO. > exclusions
ECHO \.svn\ >> exclusions
ECHO .chm >> exclusions

XCOPY /I /E /Q /EXCLUDE:exclusions ..\HelpTmp doc

..\support\windows\7zip\7z.exe a ..\SixPack-doc-html-1.0.zip doc

COPY ..\HelpTmp\SixPack.chm ..\SixPack-1.0.chm

POPD
RD /S /Q tmp

GOTO :EOF
