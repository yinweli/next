@echo off

@rd /s /q build
@rd /s /q out

call gradle outputJar

pause