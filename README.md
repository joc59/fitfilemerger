# FIT File Merger

## Purpose
The FIT File Merger is a tool designed to merge multiple FIT files recorded into a single file. This is particularly useful for athletes and fitness enthusiasts who use multiple devices to track their activities and want to consolidate their data.

FIT File Merger is intended to merge data recorded on multiple devices at the same time during the same activity into a single file. It is not intended to chain subsequent activities together.

### Currently supported functionality

#### Merge Power data from separate fit file into a main fit file. 

This is useful when you have no connection from your powermeter, home trainer, or an app to your cycling computer. Using FIT File Merger, you can merge the separately recorded power data into the fit file recorded by your bike computer without loosing any data. This includes calculations of average and max values for each lap as well as for the total session.

## Usage
```bash
fitfilemerger.exe path\to\main.fit path\to\secondary.fit path\to\merged.fit 
```

## Setup in VSCode
1. Create a .NET Solution
    ```bash
    dotnet new sln
    ```
1. Clone the Repository into your workspace
    ```bash
    git clone https://github.com/joc59/fitfilemerger.git
    ```
1. Add fitfilemerger to your solution
    ```bash
    dotnet sln add fitfilemerger\fitfilemerger.csproj
    ```
1. Build fitfilemerger
    ```bash
    dotnet build
    ```
1. Publish fitfilemerger
    ```bash
    dotnet publish
    ```

## Debugging
1. Add the following to your ``launch.json``:
     ```json
    "version": "0.2.0",
    "configurations": [{
        "name": ".NET Core Launch (console)",
        "type": "coreclr",
        "request": "launch",
        "preLaunchTask": "build",
        "program": "${workspaceFolder}/fitfilemerger/bin/Debug/net8.0/fitfilemerger.dll",
        "args": [
            "examples/main.fit",
            "examples/secondary.fit",
            "examples/merged.fit"
        ],
        "cwd": "${workspaceFolder}/fitfilemerger",
        "justMyCode": true,
        "stopAtEntry": false,
        "console": "internalConsole"
    }]
    ```

1. Add the following to your ``tasks.json``:
     ```json
    "version": "2.0.0",
    "tasks": [
        {
            "label": "build",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}"
            ],
            "problemMatcher": "$msCompile"
        }
    ]
    ```


## Contributing
Pull requests are welcome!

## License
Distributed under the Unlicense License. See `LICENSE.txt` for more information.

FIT File Merger requires the [Garmin FIT File SDK](https://developer.garmin.com/fit/overview/), which is published by Garmin under a different license. Garmin FIT File SDK is not included in this repository but will be installed through your package manager upon build.