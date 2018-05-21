using System;

//#require lib_str_util.cs

public class cmdSetTextBuf {


  public static bool run(strList words) {
    string how = words.shift();
    if (how == "arg") { return exact(String.Join(" ", words)); }
    if (how == "next") { return exact(Program.readLn()); }
    if (how == "until") { return untilMark(words.getOr(0)); }
    if (how == "decode") { return decode(words); }
    if (how == "_debug_dump_json") { return dumpTextAsJson(); }
    return Program.errUnsuppCmd();
  }


  public static bool exact(string text, string verb = "read") {
    if (strUtil.empty(text)) { Program.fail("no_data"); }
    Console.WriteLine("ok {0} {1}", verb, text.Length);
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


  public static bool dumpTextAsJson() {
    Program.failIfLockedDown();
    string text = Program.textBuf;
    Console.WriteLine("ok {0} {1}", text.Length,
      strUtil.jsonEncodeString(text));
    return true;
  }


  public static bool decode(strList words) {
    string text = Program.textBuf;
    byte[] bytes = {};
    string how;
    while (true) {
      how = words.shift();
      if (how == null) { break; }
      if (how == "base64") {
        bytes = System.Convert.FromBase64String(text);
        continue;
      }
      if (how == "utf8") {
        text = System.Text.Encoding.UTF8.GetString(bytes);
        continue;
      }
      Program.fail("unsupported_decoder " + how);
    }
    return exact(text, "decoded");
  }









}
