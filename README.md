
Spoiler: Doesn't work yet
-------------------------

For now, this tutorial just documents my attempts at getting it to work,
so you can help me debug it. (Please do!)



Installing MS Speech SDK v11
----------------------------

Tested with Ubuntu trusty and `winehq-stable` v3.0.
* WineHQ install instructions: https://wiki.winehq.org/Ubuntu
  * When I read them, the PPA was incomplete, so I had to use the official
    (non-PPA) apt repo described in the instructions.

1. Collect the files you'll need to install.
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

1. Recommended: To avoid side effects, create a new wine prefix and work there.

1. Set your wine prefix's windows version to Windows 8:
    run `winecfg` to GUI-configure it, or `winetricks win8` for automatic.
1. Install the Runtime:
    `wine msiexec /i setups/SpeechPlatformRuntime.msi`
    should start the install dialog.
1. Install the SDK: msiexec, dialog-powered.
    * Remember into which directory you install the SDK.
      In this tutorial I'll assume `C:\MS_SpeechSDK_v011`.
      You'll need DLL files from the SDK's `Assembly` subdirectory.
1. Install languages: msiexec, non-interactive.
    On Windows, this means the only dialog you'll see (if your machine is slow
    enough ;-) ) is a progress dialog, which auto-closes on success.
    In wine, it installs completely silently,
    so you'll have to just hope it succeeds.
    (or `strace` it.)
1. If you don't yet have a C# compiler, `apt-get install mono-mcs`
1. Install version 4.0 or the .NET framework. (Might take several minutes.)
    You might need to uninstall Mono first.
    An easy way to do both is to install `winetricks` and `cabextract`,
    then run `winetricks dotnet40`.



Create your first TTS app
-------------------------

1. Clone this repo, or create a new directory and copy the `speakArgs.cs`
    from this repo. It's based on this example code:
    https://msdn.microsoft.com/en-us/library/hh378340(v=office.14).aspx
1. Copy or symlink the `Microsoft.Speech.dll` from the SDK `Assembly`
    subdirectory.
1. Compile it: `mcs /reference:Microsoft.Speech.dll speakArgs.cs`
    * It should succeed silently and create a file `speakArgs.exe`.

Verify it works on original Windows:

1. Borrow a computer with Windows 8.
1. Install the runtime and languages. (SDK should not be required.)
1. Configure and test a default language:
    Control Panel -> Speech Recognition -> (in sidebar) Text-to-Speech
1. Copy `speakArgs.exe` over and run it there, in a command prompt.
    * At first run, it needs a long time to initialize.
    * Observe it outputs the progress information but no audio.
    * :-(










