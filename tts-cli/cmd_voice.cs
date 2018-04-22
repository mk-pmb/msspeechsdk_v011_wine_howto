using System;
using System.Collections.Generic;
using Microsoft.Speech.Synthesis;


//#require lib_str_util.cs


public class cmdVoice {
  public static bool run(strList words) {
    string cmd = words.shift();
    if (cmd == "info") { return cmdVoiceInfo.run(words); }
    Console.Error.WriteLine("err unsupported_command");
    return false;
  }
}

public class cmdVoiceInfo {
  public static bool run(strList words) {
    strDict info = vi2dict(Program.synth.Voice);
    Console.WriteLine("ok dict {0}", info.Count);
    Console.WriteLine("---");
    Console.WriteLine(String.Join("\n", info.toColonLines()));
    Console.WriteLine("...");
    /* example output:
      ok dict 10
      ---
      id: TTS_MS_en-US_Helen_11.0
      name: Helen
      culture: en-US
      age: adult
      gender: female
      descr: Microsoft Server Speech Text to Speech Voice (en-US, Helen)
      AudioFormats: 18
      Language: 409
      Vendor: Microsoft
      Version: 11.0
      ...
    */
    return true;
  }

  public static strDict vi2dict(VoiceInfo voice) {
    var dict = new strDict();
    dict["id"]      = voice.Id;

    string name = voice.Name;
    string culture = voice.Culture.ToString();
    string prefix = "Microsoft Server Speech Text to Speech Voice (";
    if (name.StartsWith(prefix)) {
      name = name.Substring(prefix.Length).TrimEnd(')');
    }
    prefix = culture + ", ";
    if (name.StartsWith(prefix)) {
      name = name.Substring(prefix.Length);
    }
    dict["name"] = name;

    dict["culture"] = culture;
    dict["age"]     = voice.Age.ToString().ToLower();
    dict["gender"]  = voice.Gender.ToString().ToLower();
    dict["descr"] = voice.Description;

    foreach (KeyValuePair<string, string> pair in voice.AdditionalInfo) {
      var key = pair.Key;
      var val = pair.Value;
      if ((key == "") && (val == "")) { continue; }
      var lcKey = key.ToLower();
      if (lcKey == "name") {
        if (val == name) { continue; }
        if (val == voice.Name) { continue; }
        if (val == voice.Description) { continue; }
      }
      if (dict.ContainsKey(lcKey)) {
        var lcHasVal = dict[lcKey];
        if (lcHasVal == val) { continue; }
        if (lcHasVal == val.ToLower()) { continue; }
      }
      dict[key] = val;
    }

    return dict;
  }
}














// scroll
