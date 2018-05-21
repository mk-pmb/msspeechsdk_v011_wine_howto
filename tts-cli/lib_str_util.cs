using System;
using System.Collections.Generic;

public class strUtil {

  public static bool empty(string s) {
    return ((s == null) || (s == ""));
  }

  public static string simplifySpaceChars(string orig) {
    return (orig
      ).Replace('\r', ' '
      ).Replace('\n', ' '
      ).Replace('\t', ' '
      ).Replace('\f', ' '
      ).Replace('\v', ' '
      ).Replace('\xA0', ' '
      ).Trim(' ');
  }

  public static string jsonEncodeString(string orig) {
    string json = "\"";
    int cnum;
    foreach (char ch in orig) {
      cnum = (int)ch;
      if ((0x20 <= cnum) && (cnum <= 0x7E)) {
        json += ch;
      } else {
        json += "\\u" + cnum.ToString("X4");
      }
    }
    json += "\"";
    return json;
  }

}


public class strList : List<string> {
  public strList() : base() {}
  public strList(int capacity) : base(capacity) {}
  public strList(IEnumerable<string> items) : base(items) {}


  public static char[] inLineSpace = new char[] {' ', '\t'};
  public string shift() { return this.pop(0); }


  public int wrapIdx(int i) {
    int n = this.Count;
    if (n < 1) { return -1; }
    if (i < 0) { i += n; }
    if (i < 0) { return -1; }
    if (i >= n) { return -1; }
    return i;
  }


  public string pop(int i = -1, string none = null) {
    i = wrapIdx(i);
    if (i < 0) { return none; }
    string s = this[i];
    this.RemoveAt(i);
    return s;
  }


  public string getOr(int i, string none = null) {
    i = wrapIdx(i);
    if (i < 0) { return none; }
    return this[i];
  }


  public static strList splitWords(string input,
    char[] sep = null,
    int maxWords = -1
  ) {
    if (input == null) { return new strList(); }
    if (sep == null) { sep = inLineSpace; }
    input = input.Trim(sep);
    if (input == "") { return new strList(); }
    return new strList(
      (maxWords < 1)
        ? input.Split(separator: sep)
        : input.Split(separator: sep, count: maxWords)
      );
  }

}


public class strDict : Dictionary<string, string> {
  public strDict() : base() {}
  public strDict(Int32 initialSize) : base(initialSize) {}

  public strDict update(IDictionary<string, string> srcDict) {
    foreach (KeyValuePair<string, string> pair in srcDict) {
      this[pair.Key] = pair.Value;
    }
    return this;
  }

  public strList toColonLines() {
    var lines = new List<string>(this.Count);
    foreach (KeyValuePair<string, string> pair in this) {
      lines.Add(String.Format("{0}: {1}", pair.Key, pair.Value));
    }
    return new strList(lines);
  }
}

















// scroll
