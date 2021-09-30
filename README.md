# ADBForwarder

Console application designed to handle forwarding TCP Ports (using [ADB](https://developer.android.com/studio/command-line/adb)) between your PC and Quest 2 headset, for use with [ALVR](https://github.com/alvr-org/ALVR)

### [Download here!](https://github.com/AtlasTheProto/ADBForwarder/releases/latest/download/ADBForwarder.exe)

# Building

Go look here for my build settings

This app is published in debug mode, for some reason it'll refuse to run if published in release mode, I cannot figure out why.

Other than that, its a standard dotnet publish using "Single-file" publish mode and "Ready-to-run"

# It doesn't run!

Please install the DotNet 6 runtimes!

[32bit](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-6.0.0-rc.1-windows-x86-binaries)

[64bit (You probably want this)](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-6.0.0-rc.1-windows-x64-binaries)

[Arm64](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-6.0.0-rc.1-windows-arm64-binaries)

# Problems?

Don't hesitate to raise an [issue](https://github.com/AtlasTheProto/ADBForwarder/issues) if you encounter problems!
