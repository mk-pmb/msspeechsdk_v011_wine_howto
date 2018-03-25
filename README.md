
Reliability: partially works
----------------------------

* [x] use Ubuntu to build a TTS app for Windows
* [x] make it work on Windows 8
* [ ] make it work in Wine



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
1. Install version 4.0 or the .NET framework. (Might take several minutes.)
    You might need to uninstall Mono first.
    An easy way to do both is to install `winetricks` and `cabextract`,
    then run `winetricks dotnet40`.
1. The above step might have reset your windows version, so
    make sure it's still Windows 8, adjust if needed.
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



Create your first TTS app
-------------------------

(Or you can just [download a ZIP with my `speakArgs.exe`][speakargs-exe-zip].)

  [speakargs-exe-zip]: http://l.proggr.de/?182heru3w

1. If you don't yet have a C# compiler, `apt-get install mono-mcs`
1. Clone this repo, or create a new directory and copy the `speakArgs.cs`
    from this repo. It's based on this example code:
    https://msdn.microsoft.com/en-us/library/hh378340(v=office.14).aspx
1. <del>Copy or symlink the `Microsoft.Speech.dll` from the SDK `Assembly`
    subdirectory.</del>
    Although that's the one used in the MSDN example, it won't speak.
    In order to get actual audio, you'll need another one:
1. Copy or symlink the `System.Speech.dll` from the .NET framework.
    * In my case it's in this path in wine's C: drive:
      `windows/Microsoft.NET/Framework/v4.0.30319/WPF/System.Speech.dll`
1. Compile it: `mcs /reference:System.Speech.dll speakArgs.cs`
    * It should succeed silently and create a file `speakArgs.exe`.

Verify it works on original Windows:

1. Borrow a computer with Windows 8.
1. Install the runtime and languages. (SDK should not be required.)
1. Configure and test a default language:
    Control Panel -> Speech Recognition -> (in sidebar) Text-to-Speech
1. Copy `speakArgs.exe` over and run it there, in a command prompt.
    * At first run, it needs a long time to initialize.
    * It speaks!

Now run it in wine:

1. Verify you have at least one TTS voice installed; they should be at
    `Program Files/Common Files/Microsoft Shared/Speech/Tokens`.
1. Run `wine speakArgs.exe |& tee wine.log`
1. It crashes because it can't find any voices:
    ```text
    Unhandled Exception: System.PlatformNotSupportedException:
    No voice installed on the system or none available with the current
    security setting.
    ```
    * My crashlog for `wine-stable` (wine v3.0):
      https://gist.github.com/mk-pmb/2ad02725db30b31921488e00c541cb40
    * My crashlog for `wine-devel` (wine v3.4):
      https://gist.github.com/mk-pmb/3efba8685a972fd21c4033b565ade8d2
1. ???










