FROM microsoft/windowsservercore

WORKDIR /build  
COPY ./ .
RUN powershell ./build.ps1 
