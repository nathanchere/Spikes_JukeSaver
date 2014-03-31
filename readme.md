JukeSaver spikes
================

Not intended for public consumption. Just some preliminary feeler work for an as-yet vaporware music player and screensaver package.

Branch status
-------------

master: [![master] (https://ci.appveyor.com/api/projects/status/lb6us660fonm7i4l/branch/master)](https://ci.appveyor.com/project/nathanchere/spike-jukesaver)

Branch summary 
--------------

* Audio playback
 * **NAudio.Basic** - basic NAudio usage; loading and playing an MP3
 * **NAudio.SpectrumAnalyser** -how to apply FFT to samples to get simplified data for visualisation
* IPC
 * **IPC.NamedPipes.Basic** - bare minimum effort required to get two separate applications talking over named pipes
 * **IPC.NamedPipes.LoadTest** - Named Pipes performance under load, and how both client and server can  handle the other side dis-/re-connecting
 * **IPC.MMF.Basic** - bare minimum effort required to get two separate applications talking over memory mapped files

TODO
----

* Audio - FMod
* IPC - behavior across different machines
* IPC - low-bandwith performance / issues
* IPC - (de-)serialization over transports
* IPC - memory-mapped files
* Visualisation - Unity
* Visualation - Mogre
* Portablility - Mono compatilibilty


Misc references
---------------

[IPC via named pipes](http://msdn.microsoft.com/en-us/library/bb546085(v=vs.110).aspx)
[IPC via memory mapped files](http://code.msdn.microsoft.com/windowsdesktop/Inter-process-communication-e96e94e7)
[NAudio documentation](http://naudio.codeplex.com/documentation)
[FMODSharp](https://gitorious.org/fmodsharp)
