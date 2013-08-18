@echo off
SET MSBUILD=%WINDIR%\microsoft.net\framework\v4.0.30319\MSBuild.exe

%MSBUILD% Release.proj /m
pause