using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Speech.Synthesis;

//#require lib_str_util.cs
//#require cmd_voice.info2dict.cs

public class cmdVoiceDescribe {


  public static string saneName(VoiceInfo voice) {
    string name = voice.Name;

    string prefix = "Microsoft Server Speech Text to Speech Voice (";
    if (name.StartsWith(prefix)) {
      name = name.Substring(prefix.Length).TrimEnd(')');
    }

    string culture = voice.Culture.ToString();
    prefix = culture + ", ";
    if (name.StartsWith(prefix)) { name = name.Substring(prefix.Length); }

    name = name.Trim(strList.inLineSpace);
    return name;
  }


  public static bool info(strList words) {
    List<VoiceInfo> voices = null;
    if (words.Count < 1) {
      voices = new List<VoiceInfo>();
      voices.Add(Program.currentVoice());
    } else if (words[0] == "*") {
      voices = new List<VoiceInfo>(Program.allVoices(
        ).Select(v => v.VoiceInfo));
    } else {
      voices = new List<VoiceInfo>(Program.findVoicesByIds(words
        ).Select(v => v.VoiceInfo));
    }
    int nVoices = voices.Count;
    if (nVoices < 1) {
      Console.WriteLine("dict 0\n---\n...");
      return true;
    }
    var dictLengths = new List<int>(nVoices);
    var voiceDescrs = new strList(nVoices);
    foreach (VoiceInfo vi in voices) {
      strDict info = new VoiceInfoDict(vi);
      dictLengths.Add(info.Count);
      voiceDescrs.Add(String.Join("\n", info.toColonLines()));
    }
    Console.WriteLine("dict {0}", String.Join(" ", dictLengths));
    foreach (string descr in voiceDescrs) {
      Console.WriteLine("---");
      Console.WriteLine(descr);
    }
    Console.WriteLine("...");
    return true;
  }


  public static bool list(strList words) {
    var voiceIds = new strList(Program.allVoices().Select(v => v.VoiceInfo.Id));
    Console.WriteLine("list {0}", voiceIds.Count);
    Console.WriteLine(String.Join("\n", voiceIds));
    return true;
  }










}
