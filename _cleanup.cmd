@ECHO OFF

ECHO ====================================================================================================
ECHO    Cleans up the ENTIRE workspace
ECHO ====================================================================================================
ECHO.

setlocal
:PROMPT

ECHO WARNING: You are about to clean up the entire Git workspace. This includes the deletion of 
ECHO all untracked directories and files including those being ignored. Tracked modified files 
ECHO will remain untouched and must be reverted manually.
ECHO.

SET /P AREYOUSURE=Are you sure (Y/[N])? 
IF /I "%AREYOUSURE%" NEQ "Y" GOTO END

git clean -d -X -f

:END
endlocal

ECHO.
ECHO ====================================================================================================
ECHO    Press any key to quit
ECHO ====================================================================================================
IF [%1]==[] ( PAUSE )
