using System;
using System.Collections.Generic;

public class strList : List<string> {
  public strList(IEnumerable<string> items) : base(items) {}

  public string shift() { return this.pop(0); }

  public string pop(int i = -1) {
    int n = this.Count;
    if (n < 1) { return null; }
    if (i < 0) { i += n; }
    if (i < 0) { return null; }
    if (i >= n) { return null; }
    string s = this[i];
    this.RemoveAt(i);
    return s;
  }

  public static strList splitWords(string input,
    char[] sep = null,
    int maxWords = -1
  ) {
    if (sep == null) { sep = new char[] {' '}; }
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
