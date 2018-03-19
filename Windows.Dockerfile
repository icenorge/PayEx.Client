# escape=`
FROM microsoft/windowsservercore:latest

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Retrieve .NET Core SDK
ENV DOTNET_SDK_VERSION 2.1.101
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-win-x64.zip
ENV DOTNET_SDK_DOWNLOAD_SHA 794901f629921c2ef8db9de9ef984725a3b7f7b165289294593f4add34a5abb456d1165b90cf63df287d22ba21d06a136086e4db37a63c74196332608f18b0e8
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE 1
RUN Invoke-WebRequest $Env:DOTNET_SDK_DOWNLOAD_URL -OutFile dotnet.zip;    
RUN Expand-Archive dotnet.zip -DestinationPath C:\dotnet
RUN Remove-Item -Force dotnet.zip

ENV PATH="${PATH};C:\\dotnet\\;"

# Pre-pre dotnet
RUN dotnet --info

WORKDIR \build  
COPY .\ .
RUN .\build.ps1
