@echo off

@rd /s /q out_java
@md out_java
@rd /s /q out_csharp
@md out_csharp

protoc --proto_path=source --java_out=out_java --csharp_out=out_csharp source\*.proto

pause