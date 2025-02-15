using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Speech.Synthesis;
using System.Threading; // for Thread
using System.Globalization; // for CultureInfo

//#require cmd_set_text_buf.cs
//#require cmd_speak.cs
//#require cmd_voice.cs
//#require lib_input_util.cs
//#require lib_str_util.cs

public class CodedError : Exception {
  public string errorCode = null;
  public int fatalRetVal = -1;
  public CodedError(string errorCode) : base(errorCode) {
    this.errorCode = errorCode;
  }
}

public class Program {


public static strList readLnInputQ;
public static bool interactiveMode = false;
public static Dictionary<string, bool> flags;
public static string textBuf;
public static SpeechSynthesizer synth = new SpeechSynthesizer();


public static void Main(string[] cliArgs) {
  flags = new Dictionary<string, bool>() {
    { "echo", false },
    { "lockdown", false },
    { "paused", false },
  };

  readLnInputQ = new strList(cliArgs);
  if (readLnInputQ.Count == 0) { cmdInteractive(); }
  synth.SetOutputToDefaultAudioDevice();
  int retVal = 0;
  while (true) {
    try {
      cmd();
    } catch (CodedError err) {
      string code = err.errorCode;
      if (err.fatalRetVal == 0) { break; }
      Console.Error.WriteLine("err {0}", code);
      if (err.fatalRetVal > 0) {
        retVal = err.fatalRetVal;
        break;
      }
    }
  }

  synth.Dispose();
  Console.WriteLine("hint quit rv={0}", retVal);
  Environment.Exit(retVal); /* Probably implicit, but serves as a reminder
    for me about how to exit non-zero. – mk */
}


public static bool fail(string why) { throw new CodedError(why); }
public static bool errUnsuppCmd() { return fail("unsupported_command"); }


public static bool die(string errorCode) {
  var err = new CodedError(errorCode);
  int rv = 2;
  if (errorCode == "quit") { rv = 0; }
  err.fatalRetVal = rv;
  throw err;
}


public static string readLn() {
  string ln = readLnCore();
  if (ln == null) { return ln; }
  ln = ln.Trim('\r');
  if (flags["echo"]) { Console.WriteLine("echo {0} {1}", ln.Length, ln); }
  return ln;
}


static string readLnCore() {
  if (readLnInputQ.Count > 0) { return readLnInputQ.shift(); }
  if (interactiveMode) { return libInputUtil.readStdinLn(); }
  return null;
}


public static bool cmd(strList words = null) {
  if (words == null) {
    string ln = readLn();
    if (ln == null) { return die("quit"); }
    words = strList.splitWords(ln);
  }
  if (words.Count < 1) { return true; }
  string cmd = words.shift();
  if (cmd == "#") { return true; }
  if (cmd == "must") { return cmdMust(words); }
  if (cmd == "quit") { return die("quit"); }
  if (cmd == "flag") { return cmdFlag(words); }
  if (cmd == "interactive") { return cmdInteractive(); }
  if (cmd == "voice") { return cmdVoice.run(words); }
  if (cmd == "vol") { return cmdMasterVolume(words.getOr(0)); }
  if (cmd == "audio_output") { return cmdOutput(words); }
  if (cmd == "set_text") { return cmdSetTextBuf.run(words); }
  if (cmd == "speak") { return cmdSpeak.run(words); }
  if (cmd == "speak_sync") { return cmdSpeak.sync(); } // legacy v1.0.1 compat
  if (cmd == "sleep") { return cmdSleep(words); }
  return errUnsuppCmd();
}


public static bool cmdMust(strList words) {
  try {
    return cmd(words);
  } catch (CodedError err) {
    if (err.fatalRetVal < 1) { err.fatalRetVal = 7; }
    throw err;
  }
}


public static bool failIfLockedDown() {
  if (flags["lockdown"]) { fail("locked_down"); }
  return true;
}


public static bool cmdFlag_str2bool(string s) {
  if (s == "on")  { return true; }
  if (s == "yes") { return true; }
  if (s == "off") { return false; }
  if (s == "no")  { return false; }
  return errUnsuppCmd();
}


public static bool cmdFlag(strList words) {
  string key = words[0];
  if (!flags.ContainsKey(key)) { return errUnsuppCmd(); }
  bool val = flags[key];
  if (words.Count >= 2) {
    bool upd = cmdFlag_str2bool(words[1]);
    if ((key == "lockdown") && (!upd)) { failIfLockedDown(); }
    flags[key] = upd;
    val = upd;
    if (key == "paused") {
      if (upd) { synth.Pause(); } else { synth.Resume(); }
    }
  }
  Console.WriteLine("ok config_flag {0} {1}", key, (val ? "on" : "off"));
  return true;
}


public static bool cmdInteractive() {
  interactiveMode = true;
  Console.WriteLine("ok begin_interactive_mode");
  return true;
}


public static VoiceInfo currentVoice() {
  VoiceInfo vi = synth.Voice;
  if (vi == null) { fail("no_voice"); }
  return vi;
}


public static List<InstalledVoice> allVoices() {
  List<InstalledVoice> all = synth.GetInstalledVoices().ToList();
  if (all.Count < 1) { fail("no_voice"); }
  return all;
}


public static List<InstalledVoice> findVoicesByIds(
  strList voiceIds,
  int nMax = -1
) {
  List<InstalledVoice> all = allVoices();
  var collect = new List<InstalledVoice>();
  foreach (string wantId in voiceIds) {
    int found = all.FindIndex(v => v.VoiceInfo.Id == wantId);
    if (found < 0) { continue; }
    collect.Add(all[found]);
    if ((0 <= nMax) && (nMax <= collect.Count)) { break; }
  }
  return collect;
}


public static bool cmdOutput(strList words) {
  string destType = words.shift();
  if (destType == "default") {
    failIfLockedDown();
    synth.SetOutputToDefaultAudioDevice();
    Console.WriteLine("ok audio_destination {0}", destType);
    return true;
  }
  return errUnsuppCmd();
}


public static bool cmdMasterVolume(string want) {
  if ((want != null) && (want != "")) {
    synth.Volume = Int32.Parse(want);
  }
  Console.WriteLine("ok master_volume {0}%", synth.Volume);
  return true;
}


public static bool cmdSleep(strList words) {
  float val = float.Parse(words.shift(), CultureInfo.InvariantCulture);
  string unit = words.shift();
  if (unit != "sec") { fail("unsupported_time_unit"); }

  int msec = (int)Math.Ceiling(val * 1.0e3);
  Thread.Sleep(msec);
  Console.WriteLine("ok slept");
  return true;
}













}
