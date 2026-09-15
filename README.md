# Etoile.Lite

<a href="https://www.nuget.org/packages/Etoile.Lite/"><img src="https://img.shields.io/nuget/v/Etoile.Lite?logo=nuget&label=Etoile.Lite&color=%23004880"/></a>
<a href="https://www.nuget.org/packages/Etoile.Lite.Parser/"><img src="https://img.shields.io/nuget/v/Etoile.Lite.Parser?logo=nuget&label=Etoile.Lite.Parser&color=%23004880"/></a>

A lightweight utility that converts Arcaea chart files into [ArcCreate](https://github.com/Arcthesia/ArcCreate)-compatible packages

Install to your system with the following command:

```shell
dotnet tool install --global Etoile.Lite
```

## Features

- Converts '.aff' charts in the official Arcaea chart format into ArcCreate-compatible '.arcpkg' packages, with '.sc.json' guaranteed
- Contains codes from ArcCreate to maximize compatibility

#### What is this NOT

This is not a general-purpose '.aff' format converter, unlike [EtoileResurrection](https://github.com/freeze-dolphin/EtoileResurrection), it cannot:

- Convert existing '.arcpkg' packages back into Arcaea-compatible '.aff' charts
- Handle commands, features, or formats that have never been in Arcaea official charts
- Automatically create equivalent representations for features that cannot be expressed in the official Arcaea chart format