
## Build status

| Build server                | Platform     | Status                                                                                                                    |
|-----------------------------|--------------|---------------------------------------------------------------------------------------------------------------------------|
| AppVeyor                    | Windows      | [![Build status](https://ci.appveyor.com/api/projects/status/jqpkvy5fe523hsja/branch/master?svg=true)](https://ci.appveyor.com/project/ice/payex-client/branch/master)|
| Travis                      | Linux  | [![Build Status](https://travis-ci.org/icenorge/PayEx.Client.svg?branch=master)](https://travis-ci.org/icenorge/PayEx.Client) |


## About
`PayEx.Client` is a `netstandard2` library to talk to PayEx direct REST APIs.

Download it from NuGet:[![NuGet](https://img.shields.io/nuget/dt/payex.client.svg)](https://www.nuget.org/packages/payex.client/)

## Supported APIs:
- Vipps 
  - create payment
  - Vipps authorize
  - capture 
  - cancel
  - reversal
- CreditCard 
  - create payment
  - create recurring payment with initial payment
  - create recurring payment without initial payment
  - capture
  - cancel
  - reversal

# Sample apps
Check the [the samples folder](https://github.com/icenorge/PayEx.Client/tree/master/src/Samples)
