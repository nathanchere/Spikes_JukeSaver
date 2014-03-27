JukeSaver spikes
================

Not intended for public consumption. Just some preliminary feeler work for an as-yet vaporware music player and screensaver package.

Branch status
-------------

[![master] (https://ci.appveyor.com/api/projects/status/lb6us660fonm7i4l/master](https://ci.appveyor.com/project/nathanchere/spike-jukesaver)
[![NAudio](https://ci.appveyor.com/api/projects/status/lb6us660fonm7i4l/NAudio)](https://ci.appveyor.com/project/nathanchere/spike-jukesaver)


Things to investigate
---------------------

* IPC options
 * reliability
 * ease of use
 * can be used local + remote
 * high bandwidth performance
* Audio options
 * native DLL + wrapper? eg Fmod
 * managed lib? eg NAudio
* rendering options
 * Unity (probably depends on IPC method)
 * Mogre
* cross-platform considerations for all-of-the-above
 * Mono compatible? ie easy Linux port?


Misc references
---------------

[IPC via named pipes](http://msdn.microsoft.com/en-us/library/bb546085(v=vs.110).aspx)
[IPC via memory mapped files](http://code.msdn.microsoft.com/windowsdesktop/Inter-process-communication-e96e94e7)
[NAudio documentation](http://naudio.codeplex.com/documentation)
[FMODSharp](https://gitorious.org/fmodsharp)