using System;

//#require lib_str_util.cs

public class cmdSetTextBuf {


  public static bool run(strList words) {
    string how = words.shift();
    if (how == "arg") { return exact(String.Join(" ", words)); }
    if (how == "next") { return exact(Program.readLn()); }
    if (how == "until") { return untilMark(words.getOr(0)); }
    return Program.errUnsuppCmd();
  }


  public static bool exact(string text) {
    if ((text == null) || (text == "")) { Program.fail("no_data"); }
    Console.WriteLine("ok read {0}", text.Length);
    Program.textBuf = text;
    return true;
  }


  public static bool untilMark(string mark) {
    if (mark == null) { mark = ""; }
    string text = "";
    while (true) {
      string ln = Program.readLn();
      if ((ln == null) || (ln == mark)) { break; }
      text += ln + " ";
    }
    return exact(text);
  }






}
