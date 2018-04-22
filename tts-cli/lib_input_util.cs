using System;
using System.Collections.Generic;

//#require lib_str_util.cs

public class libInputUtil {


public static string readStdinLn() {
  try {
    return Console.ReadLine();
  } catch (NullReferenceException) {
    // stdin was closed while we tried to read
    return null;
  } catch (UnauthorizedAccessException) {
    string explain = "Not authorized to read from stdin."
      + " If stdin is a TTY, try connecting a pipe instead."
      + " This may be an instance of Wine bug #45039, see"
      + " https://bugs.winehq.org/show_bug.cgi?id=45039 .";
    Console.Error.WriteLine("warn {0}" + explain);
    Program.die("stdin_unauthorized_access");
    return null;
    // throw new UnauthorizedAccessException(errMsg, errAccess);
  }
}










}
