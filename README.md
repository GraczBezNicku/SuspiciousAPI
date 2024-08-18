# SuspiciousAPI
SuspiciousAPI is a project of mine that was started with the goal of streamlining the modding process for the game "Among Us".
All modders that wish to create a mod for Among Us need to work out their own role system, change core game functions and update the mod each update (which is pain for il2cpp games).
This API aims for ease of use and compatibility with multiple mods active at once, as currently the only way to have two mods at once is to forcibly merge them toghether, usually breaking everything due to patch differences.
# Current Progress
Due to multiple factors, Suspicious API's development has slowed down to the point where no meaningfull progress is being made.
I will try to work on the API in my free time, however it may still take a while before we reach a usable beta.
# How to use?
Currently Suspicious API **is not meant to be used**, as about 70% of the desired features are still missing, and I'm still trying to figure out how to properly distribute it.
# How can I support development?
Currently there are no ways of supporting Suspicious API's development directly. If you wish to help, you can help developing it or sharing its existence to others.
# Development Information
As of now, the project uses references generated by BepInEx contained within a `References` folder, that's in the same directory as the `.sln` file.
This repository wasn't meant to be made public so early, so I'll replace the `References` folder with a system variable as soon as start working on it again.
To obatin said references, you need to own a copy of Among Us and install `BepInEx-6.0.0-be.688`. After launching the game once, you should get all required DLLs in the `BepInEx/interop` folder.
All base unity DLLs will be in the `BepInEx/unity-libs` folder.  
**Please contact me on the provided Discord server before contributing, so we can discuss if and how certain features should be implemented.**
# Discord Server
https://discord.gg/BqwB4XbpsV (Note: The server is very barebones since I haven't put much effort into it. We'll keep improving it the closer we get to a usable release)
