FROM microsoft/windowsservercore:latest

WORKDIR /build  
COPY ./ .
RUN powershell ./build.ps1 
