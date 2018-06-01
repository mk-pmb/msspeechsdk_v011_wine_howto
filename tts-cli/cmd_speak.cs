using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Speech.Synthesis;

//#require lib_str_util.cs

public class cmdSpeak {


  public static bool run(strList words) {
    string cmd = words.shift();
    if (cmd == "sync") { return sync(); }
    if (cmd == "start") { return start(); }
    if (cmd == "stop") { return stop(); }
    // pause/resume: toggle the "paused" flag
    return Program.errUnsuppCmd();
  }


  public static bool sync() {
    if (Program.flags["paused"]) { Program.fail("not_ready"); }
    Program.synth.SpeakAsyncCancelAll();
    Program.synth.Speak(Program.textBuf);
    Console.WriteLine("ok spoken");
    return true;
  }


  public static bool start() {
    Program.synth.SpeakAsyncCancelAll();
    Program.synth.SpeakAsync(Program.textBuf);
    Console.WriteLine("ok now speaking");
    return true;
  }


  public static bool stop() {
    Program.synth.SpeakAsyncCancelAll();
    Console.WriteLine("ok stopped speaking");
    return true;
  }












}
