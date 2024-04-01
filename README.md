
Reliability: works for me
-------------------------

* [x] use Ubuntu to build a TTS app for Windows
* [x] make it work on Windows 8
* [x] make it work in Wine



Installing MS Speech SDK v11
----------------------------

Tested with Ubuntu trusty and `winehq-stable` v3.0.

* WineHQ install instructions: https://wiki.winehq.org/Ubuntu
  * When I read them, the PPA was incomplete, so I had to use the official
    (non-PPA) apt repo described in the instructions.

1.  Recommended: To avoid side effects, create a new wine prefix and work there.

1.  Decide the CPU architecture to use inside your wineprefix and/or
    on actual windows target machines.
    * Configure your wine prefix appropriately.
      This probably means setting the WINEARCH environment variable.
    * I always just use `WINEARCH=win32` anyway because I build inside a
      wine prefix in which I run some ancient proprietary abandonware anyway.

1.  Collect the files you'll need to install.
    * When there are multiple versions of a download, make sure to pick
      the one appropriate for your targeted CPU architecture.
    * In case the downloads won't easily start: The Download Center has a
      noscript tag with direct download links between the "Thank you for
      downloading" headline and the "Install Instructions" button.
    * Runtime:
      http://web.archive.org/web/20180324173437/https://www.microsoft.com/en-us/download/confirmation.aspx?id=27225
    * SDK:
      http://web.archive.org/web/20180304124818/https://www.microsoft.com/en-us/download/confirmation.aspx?id=27226
    * Languages (you'll need at least one):
      http://web.archive.org/web/20180304122215/https://www.microsoft.com/en-us/download/confirmation.aspx?id=27224
      * SR = speech recognition, TTS = text-to-speech

1.  Set your wine prefix's windows version to Windows 8:
    run `winecfg` to GUI-configure it, or `winetricks win8` for automatic.

1.  Install version 4.0 or the .NET framework. (Might take several minutes.)
    You might need to uninstall Mono first.
    An easy way to do both is to install `winetricks` and `cabextract`,
    then run `winetricks dotnet40`.
    * Hint for future me: To detect which version of .NET is required,
      make a fresh wine prefix, uninstall Mono (`wine uninstaller`),
      then run the self-made TTS app from next chapter.
      Its error message will list acceptable .NET versions.

1.  The above step might have reset your windows version, so
    make sure it's still Windows 8, adjust if needed.

1.  Install the Runtime:
    `wine msiexec /i setups/SpeechPlatformRuntime.msi`
    should start the install dialog.

1.  Install the SDK: msiexec (as above), dialog-powered.
    * Remember into which directory you install the SDK.
      In this tutorial I'll assume `C:\MS_SpeechSDK_v011`.
      You'll need DLL files from the SDK's `Assembly` subdirectory.

1.  Install languages: msiexec (as above), non-interactive.
    * On Windows, this means the only dialog you'll see
      (if your machine is slow enough ;-) )
      is a progress dialog, which auto-closes on success.
    * In wine, it installs completely silently,
      so you'll have to just hope it succeeds.
      (Or you may `strace` it.)



Create your first TTS app
-------------------------

1.  If you don't yet have a C# compiler, `apt-get install mono-mcs`
1.  Clone this repo, or create a new directory and copy the `speakArgs.cs`
    from this repo. It's based on this example code:
    https://msdn.microsoft.com/en-us/library/hh378340(v=office.14).aspx
1.  Copy or symlink the `Microsoft.Speech.dll` from the SDK `Assembly`
    subdirectory.
    * NB: There are at least
      [two versions of the SAPI][sapi-server-vs-desktop]:
      * `Microsoft.Speech.*` is the server version.
        This one I managed to make work in wine.
      * `System.Speech.*` is the desktop version.
        Trying to use this one in wine made my program crash.
        If you'd like to tinker with it anyway,
        copy or symlink the `System.Speech.dll` from the .NET framework;
        in my case it's in this path in wine's C: drive:
        `windows/Microsoft.NET/Framework/v4.0.30319/WPF/System.Speech.dll`
1.  Compile it: `mcs /reference:System.Speech.dll speakArgs.cs`
    * It should succeed silently and create a file `speakArgs.exe`.

Run it in wine:

1.  Verify you have at least one TTS voice installed; they should be at
    `Program Files/Common Files/Microsoft Shared/Speech/Tokens`.
1.  Check your audio settings, set volume tentatively low for your
    default output device.
1.  Run `wine speakArgs.exe`
1.  If you didn't hear anything, retry at louder volumes.
    If it still doesn't work, let's investigate.











  [sapi-server-vs-desktop]: http://web.archive.org/web/20180403220404/https://stackoverflow.com/questions/2977338/what-is-the-difference-between-system-speech-recognition-and-microsoft-speech-re

