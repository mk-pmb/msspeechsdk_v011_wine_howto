using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Speech.Synthesis;

//#require lib_str_util.cs
//#require cmd_voice.describe.cs
//#require cmd_voice.select.cs

public class cmdVoice {


  public static bool run(strList words) {
    string cmd = words.shift();
    if (cmd == "curid") { return curId(); }
    if (cmd == "info") { return cmdVoiceDescribe.info(words); }
    if (cmd == "list") { return cmdVoiceDescribe.list(words); }
    if (cmd == "select") { return cmdVoiceSelect.run(words); }
    if (cmd == "vol") { return cmdSpeechVolume(words.getOr(0)); }
    if (cmd == "rate") { return cmdSpeechRate(words.getOr(0)); }
    return Program.errUnsuppCmd();
  }


  public static bool curId() {
    Console.WriteLine("ok voice_id {0}", Program.currentVoice().Id);
    return true;
  }


  public static bool cmdSpeechVolume(string want) {
    if ((want != null) && (want != "")) {
      Program.synth.TtsVolume = Int32.Parse(want);
    }
    Console.WriteLine("ok speech_volume {0}%", Program.synth.TtsVolume);
    return true;
  }


  public static bool cmdSpeechRate(string want) {
    if ((want != null) && (want != "")) {
      Program.synth.Rate = Int32.Parse(want);
      // range -10 .. +10
    }
    Console.WriteLine("ok speech_rate {0}", Program.synth.Rate);
    return true;
  }










}
