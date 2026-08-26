# Etoile.Lite

A lightweight utility that converts Arcaea chart files into [ArcCreate](https://github.com/Arcthesia/ArcCreate)-compatible packages

## Features

- Converts '.aff' charts in the official Arcaea chart format into ArcCreate-compatible '.arcpkg' packages, with '.sc.json' guaranteed
- Contains codes from ArcCreate to maximize compatibility

#### What is this NOT

This is not a general-purpose '.aff' format converter, unlike [EtoileResurrection](https://github.com/freeze-dolphin/EtoileResurrection), it cannot:

- Convert existing '.arcpkg' packages back into Arcaea-compatible '.aff' charts
- Handle commands, features, or formats that have never been in Arcaea official charts
- Automatically create equivalent representations for features that cannot be expressed in the official Arcaea chart format