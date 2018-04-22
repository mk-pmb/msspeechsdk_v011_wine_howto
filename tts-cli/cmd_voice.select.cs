using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Speech.Synthesis;

//#require lib_str_util.cs
//#require cmd_voice.cs

public class cmdVoiceSelect {


  public static bool run(strList words) {
    string how = words.shift();
    if (how == "id") { return byId(words); }
    if (how == "namepart") { return byNamePart(words.getOr(0)); }
    return Program.errUnsuppCmd();
  }


  public static bool byNamePart(string namePart) {
    if ((namePart != null) && (namePart != "")) {
      Program.synth.SelectVoice(namePart);
    }
    return cmdVoice.curId();
  }


  public static bool byId(strList words) {
    // unfortunately .SelectVoice() seems to not accept an ID,
    // not even exact names, but only name substrings. Duh.
    List<InstalledVoice> found = Program.findVoicesByIds(words, nMax: 1);
    if (found.Count < 1) { Program.fail("no_voice"); }
    return byNamePart(found[0].VoiceInfo.Name);
  }






}
