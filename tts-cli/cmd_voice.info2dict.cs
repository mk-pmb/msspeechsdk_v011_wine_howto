using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Speech.Synthesis;

//#require lib_str_util.cs
//#require cmd_voice.describe.cs

public class VoiceInfoDict : strDict {

  public VoiceInfoDict (VoiceInfo voice) : base() {
    var dict = this;
    dict["id"]      = voice.Id;
    string name = cmdVoiceDescribe.saneName(voice);
    dict["name"] = name;
    dict["culture"] = voice.Culture.ToString();
    string agegroup = voice.Age.ToString().ToLower();
    dict["agegroup"] = agegroup;
    dict["gender"]  = voice.Gender.ToString().ToLower();
    dict["descr"] = voice.Description;

    foreach (KeyValuePair<string, string> pair in voice.AdditionalInfo) {
      var key = pair.Key;
      var val = pair.Value;
      if ((key == "") && (val == "")) { continue; }
      var lcKey = key.ToLower();
      if (lcKey == "age") {
        if (val == agegroup) { continue; }
        if (val.ToLower() == agegroup) { continue; }
      }
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
  }

}
