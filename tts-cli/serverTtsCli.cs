using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Speech.Synthesis;

//#require lib_input_util.cs
//#require lib_str_util.cs
//#require cmd_voice.cs

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
public static bool readLnEcho = false;
public static SpeechSynthesizer synth = new SpeechSynthesizer();


public static void Main(string[] cliArgs) {
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
  // Probably implicit, but serves as a reminder for me about how
  // to exit non-zero.      – mk
  Console.WriteLine("hint quit rv={0}", retVal);
  Environment.Exit(retVal);
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
  if (readLnEcho) { Console.WriteLine("echo {0} {1}", ln.Length, ln); }
  return ln;
}


static string readLnCore() {
  if (readLnInputQ.Count > 0) { return readLnInputQ.shift(); }
  if (interactiveMode) { return libInputUtil.readStdinLn(); }
  return null;
}


public static bool cmd() {
  string ln = readLn();
  if (ln == null) { return die("quit"); }
  strList words = strList.splitWords(ln);
  if (words.Count < 1) { return true; }
  string cmd = words.shift();
  if (cmd == "#") { return true; }
  if (cmd == "quit") { return die("quit"); }
  if (cmd == "echo") { return cmdEcho(words); }
  if (cmd == "interactive") { return cmdInteractive(); }
  if (cmd == "voice") { return cmdVoice.run(words); }
  return errUnsuppCmd();
}


public static bool cmdEcho() {
  Console.WriteLine("ok config_echo " + (readLnEcho ? "on" : "off"));
  return true;
}


public static bool cmdEcho(strList words) {
  if (words.Count < 1) { return cmdEcho(); }
  string cmd = words[0];
  if (cmd == "on") {
    readLnEcho = true;
    return cmdEcho();
  }
  if (cmd == "off") {
    readLnEcho = false;
    return cmdEcho();
  }
  return errUnsuppCmd();
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


public static List<InstalledVoice> findVoicesByIds(strList voiceIds) {
  List<InstalledVoice> all = allVoices();
  var collect = new List<InstalledVoice>();
  foreach (string wantId in voiceIds) {
    int found = all.FindIndex(v => v.VoiceInfo.Id == wantId);
    if (found >= 0) { collect.Add(all[found]); }
  }
  return collect;
}















}
