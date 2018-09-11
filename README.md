
## Build status

| Build server                | Platform     | Status                                                                                                                    |
|-----------------------------|--------------|---------------------------------------------------------------------------------------------------------------------------|
| AppVeyor                    | Windows      | [![Build status](https://ci.appveyor.com/api/projects/status/jqpkvy5fe523hsja/branch/master?svg=true)](https://ci.appveyor.com/project/ice/payex-client/branch/master)|
| Travis                      | Linux  | [![Build Status](https://travis-ci.org/icenorge/PayEx.Client.svg?branch=master)](https://travis-ci.org/icenorge/PayEx.Client) |
| Azure DevOps | Linux |[![Build Status](https://dev.azure.com/icenorge/PayEx.Client/_apis/build/status/icenorge.PayEx.Client)](https://dev.azure.com/icenorge/PayEx.Client/_build/latest?definitionId=2)|

## About
`PayEx.Client` is a `netstandard2` library to talk to PayEx direct REST APIs.

Download it from NuGet:[![NuGet](https://img.shields.io/nuget/dt/payex.client.svg)](https://www.nuget.org/packages/payex.client/)

## Supported APIs:
- Vipps (create payment, Vipps authorize, capture, cancel, reversal)
- CreditCard link (create payment, capture, cancel, reversal)

# Sample apps
Check the [the samples folder](https://github.com/icenorge/PayEx.Client/tree/master/src/Samples)
