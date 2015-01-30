@ECHO OFF
If NOT [%1] == [] GOTO :SETLOOP
IF [%1] == [] GOTO :RESETLOOP
:SETLOOP 
SET loop=%1%
GOTO :MAINFLOW
:RESETLOOP
SET loop=3
:MAINFLOW
@ECHO ON
START "" /W %cd%\WhiteLibrary\bin\Debug\WhiteLibrary.exe 0 %loop%
START "" /W %cd%\AutoUILibrary\bin\Debug\AutoUILibrary.exe 0 %loop%


