# TupleSplatter
Splat (or deconstruct or unpack) tuples into method arguments

``install-package TupleSplatter``

Example:

```cs
using TupleSplatter;

// Splat over a void method
public static void DoNothing(int a, string b, int c) { };
(1, "xxx", 2).Splat(DoNothing);

// Splat over a method with a return value
public static string GetCombined(int p1, int p2) => a.ToString() + b.ToString();
var combined = (4, 2).Splat(GetCombined);

// Invoke actions, funcs and delegates by splatting
var writeStuff = new Action((writeMe, andMe) => Console.WriteLine($"{writeMe} {andMe}"));
writeStuff.SplatInvoke(("Hello", "world!"));
```
