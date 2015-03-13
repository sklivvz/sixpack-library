# Introduction #

The `MethodReference` class allows to obtain references to `MethodInfo` in a type-safe way.

# Details #

To retrieve a MethodInfo, you need to call one of the overloads of `MethodReference`.Get(). Reach of these overloads takes an Expression and returns the a reference to the method that is invoked by the expression.

```
MethodInfo writeLine = MethodReference.Get(
    // Actual argument values of WriteLine are ignored.
    // They are needed only to resolve the overload
    () => Console.WriteLine("", null)
);
```

The above code returns the same value as using the reflection API:

```
MethodInfo writeLine = typeof(Console).GetMethod(
    "WriteLine", // Name of the method
    BindingFlags.Static | BindingFlags.Public, // We want a public static method
    null,
    new[] { typeof(string), typeof(object[]) }, // WriteLine(string, object[]),
    null
);
```

Apart from convenience, using MethodReference offers a safe way of obtaining the method reference. If for some reason the name of WriteLine changed, the first example would detect it at compile time, while the second example would only detect it at runtime.