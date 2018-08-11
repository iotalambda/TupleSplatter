# TupleSplatter
Splat tuples into method arguments

``install-package TupleSplatter``

Example:

```cs
using TupleSplatter;

public static void DoNothing(int a, string b, int c) { };
public static string GetCombined(int p1, int p2) => a.ToString() + b.ToString();

...

(1, "xxx", 2).Splat(DoNothing);
var combined = (4, 2).Splat(GetCombined);

var writeStuff = new Action((writeMe, andMe) => Console.WriteLine($"{writeMe} {andMe}"));
writeStuff.SplatInvoke(("Hello", "world!"));
```
