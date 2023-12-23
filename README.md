# Proteus

[![CodeFactor](https://www.codefactor.io/repository/github/thexds/proteus/badge)](https://www.codefactor.io/repository/github/thexds/proteus)
[![Build Proteus](https://github.com/TheXDS/Proteus/actions/workflows/build.yml/badge.svg)](https://github.com/TheXDS/Proteus/actions/workflows/build.yml)
[![Publish Proteus](https://github.com/TheXDS/Proteus/actions/workflows/publish.yml/badge.svg)](https://github.com/TheXDS/Proteus/actions/workflows/publish.yml)
[![Issues](https://img.shields.io/github/issues/TheXDS/Proteus)](https://github.com/TheXDS/Proteus/issues)
[![MIT](https://img.shields.io/github/license/TheXDS/Proteus)](https://mit-license.org/)

`Proteus` is a framework to rapidly prototype and develop apps connected to databases, implementing the logic required to dynamically generate the required UI and ViewModels to execute basic CRUD operations.

## Releases
Release | Link
--- | ---
Latest public release: | [![Latest stable NuGet package](https://buildstats.info/nuget/TheXDS.Proteus)](https://www.nuget.org/packages/TheXDS.Proteus/)  
Latest development release: | [![Latest development NuGet package](https://buildstats.info/nuget/TheXDS.Proteus?includePreReleases=true)](https://www.nuget.org/packages/TheXDS.Proteus/)

**Package Manager**  
```sh
Install-Package TheXDS.Proteus
```

**.NET CLI**  
```sh
dotnet add package TheXDS.Proteus
```

**Paket CLI**  
```sh
paket add TheXDS.Proteus
```

**Package reference**  
```xml
<PackageReference Include="TheXDS.Proteus" Version="1.0.0" />
```

**C# interactive window (CSI)**  
```
#r "nuget: TheXDS.Proteus, 1.0.0"
```

## Building
Proteus can be built on any platform or CI environment supported by dotnet.

### Prerequisites
- [.Net SDK 6.0](https://dotnet.microsoft.com/).

### Build Proteus
```sh
dotnet build ./src/Proteus.sln
```
The resulting binaries will be in the `./Build/bin` directory.

## Contribute
[![Buy Me A Coffee](https://cdn.buymeacoffee.com/buttons/default-orange.png)](https://www.buymeacoffee.com/xdsxpsivx)

If `Proteus` is useful to you, or if you're interested in donating to sponsor the project, feel free to to a donation via [PayPal](https://paypal.me/thexds), [BuyMeACoffee](https://www.buymeacoffee.com/xdsxpsivx) or just contact me directly.

Sadly, I cannot offer other means of sending donations as of right now due to my country (Honduras) not being supported by almost any platform.
