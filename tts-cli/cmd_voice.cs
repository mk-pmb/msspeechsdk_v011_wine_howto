using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Speech.Synthesis;

//#require lib_str_util.cs
//#require cmd_voice.describe.cs

public class cmdVoice {


  public static bool run(strList words) {
    string cmd = words.shift();
    if (cmd == "curid") { return curid(); }
    if (cmd == "info") { return cmdVoiceDescribe.info(words); }
    if (cmd == "list") { return cmdVoiceDescribe.list(words); }
    return Program.errUnsuppCmd();
  }


  public static bool curid() {
    Console.WriteLine("ok {0}", Program.currentVoice().Id);
    return true;
  }








}
