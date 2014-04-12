# JukeSaver spikes

Not intended for public consumption. Just some preliminary feeler work for an as-yet vaporware music player and screensaver package.

**Build status**: [![master] (https://ci.appveyor.com/api/projects/status/lb6us660fonm7i4l/branch/master)](https://ci.appveyor.com/project/nathanchere/spike-jukesaver)

## Spike summary 

### Basic

* Audio playback
 * **NAudio.Basic** - basic NAudio usage; loading and playing an MP3
 * **NAudio.SpectrumAnalyser** - how to apply FFT to samples to get simplified data for visualisation
 * **Fmod.Basic** - basic FmodSharp usage; loading and playing an MP3
 * **Fmod.SpectrumAnalyser** - how to apply FFT to samples to get simplified data for visualisation
* IPC
 * **IPC.NamedPipes.Basic** - bare minimum effort required to get two separate applications talking over named pipes
 * **IPC.NamedPipes.LoadTest** - Named Pipes performance under load, and how both client and server can  handle the other side dis-/re-connecting
 * **IPC.MMF.Basic** - bare minimum effort required to get two separate applications talking over memory 
 * **IPC.MMF.Serialization** - sending/receiving CLR objects over memory-mapped files
* Graphics
 * **GFX.SFML.Basic** - basic window and drawing a circle on screen

### Integration

TODO: memory mapped files to pass audio analysis data between server/client

##TODO

* IPC - behavior across different machines
* IPC - low-bandwith performance / issues
* IPC - (de-)serialization over transports
* Visualisation - Unity
* Visualation - Mogre
* Portablility - Mono compatilibilty

## Notes / Observations

### NAudio

+ no additional dependencies / all managed code
+ more than good-enough performance
+ extremely stable
+ works with .Net Framework 2.0
+ very polished API
- very poor documentation; more discovery work than should be needed
- distinct lack of support classes; would benefit greatly from a helper/'Contrib' library.
? there is a Pluralsight course and a couple ebooks, but can't comment on how good they are

### FMOD Ex

+ much more feature-rich, extensive API with advanced DSP system
+ basic effects like reverb and delay built-in and highly configurable
+ underlying library is very stable
+ better documentation via official Fmod SDK
+ extremely performant
- additonal "fmodex.dll" dependency to be distributed
- no "AnyCPU" support - x86 only
- finicky; have experienced issues like a channel or sound handle changing or zeroing out for no apparent reason, and have been unable to reproduce reliably
- very C-like API (read: clumsy)
- 'pull' event model more than offsets core performance benefits
- typical non-managed code issues; more work required to manage resources cleanly
? have started [nFMOD](https://github.com/nathanchere/nFMOD) for the purpose of a cleaner API and enforcing correct usage from managed code

### Named pipes
+ Pretty easy to get a basic example running
- Not very robust; a lot of additional work needed for handing things like dropped/resumed connections, multiple clients etc 

### Memory-mapped files
+ Very easy to get a basic example running
+ Extremely robust - restarting both server and client does not cause errors for the other side, multiple clients work fine, multiple servers fine with a little extra work
+ Extremely fast - talking end-to-end times of an average for 2-5 milliseconds for the initial message and well under a tenth of a millisecond for subsequent messages.

### SFML.Net
+ Very stable wrapper library
+ Have not had any of the usual issues with using non-managed DLLs from managed code

### Misc summaries

* Memory mapped files benchmark better than named pipes but the real-world performance difference is negligible. Both are easily more than fast enough for what I want.

## Misc references

* [IPC via named pipes](http://msdn.microsoft.com/en-us/library/bb546085(v=vs.110).aspx)
* [IPC via memory mapped files](http://code.msdn.microsoft.com/windowsdesktop/Inter-process-communication-e96e94e7)
* [MMF with data structures and objects](http://coders-corner.net/2013/03/22/inter-process-communication-with-memory-mapped-files-part-01-transfer-a-data-structure-and-an-object/)
* [NAudio documentation](http://naudio.codeplex.com/documentation)
* [FMOD wrapper: FMODSharp](https://gitorious.org/fmodsharp)
* [FMOD wrapper: nFMOD](https://github.com/nathanchere/nFMOD)
* [SFML official tutorials (for 2.1)](http://www.sfml-dev.org/tutorials/2.1)
