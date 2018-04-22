using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Speech.Synthesis;

//#require cmd_voice.cs
//#require lib_input_util.cs
//#require lib_str_util.cs

public class Program {


public static strList readLnInputQ;
public static bool interactiveMode = false;
public static SpeechSynthesizer synth = new SpeechSynthesizer();

public static string readLn() {
  if (readLnInputQ.Count > 0) { return readLnInputQ.shift(); }
  if (interactiveMode) { return libInputUtil.readStdinLn(); }
  return null;
}

public static void Main(string[] cliArgs) {
  readLnInputQ = new strList(cliArgs);
  if (readLnInputQ.Count == 0) { cmdInteractive(); }
  while (true) {
    string ln = readLn();
    if (ln == null) { break; }
    cmd(ln);
  }

  Console.WriteLine("hint quit");
  // Probably implicit, but serves as a reminder for me about how
  // to exit non-zero.      – mk
  Environment.Exit(0);
}


public static bool cmd(string ln) {
  strList words = strList.splitWords(ln);
  string cmd = words.shift();
  if (cmd == "") { return true; }
  if (cmd == "#") { return true; }
  if (cmd == "interactive") { return cmdInteractive(); }
  if (cmd == "voice") { return cmdVoice.run(words); }
  Console.Error.WriteLine("err unsupported_command");
  return false;
}


public static bool cmdInteractive() {
  interactiveMode = true;
  Console.WriteLine("ok begin_interactive_mode");
  return true;
}
















}
