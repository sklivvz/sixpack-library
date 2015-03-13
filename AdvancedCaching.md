http://sites.google.com/site/sixpacklibrary/_/rsrc/1237036619924/Home/sixpack-icon.gif?height=48&width=48
# Advanced Caching #

When you have to cache a method from a derived class, or when the method is static, you can't use the `[Cached]` and `[CachedMethod]` attributes.
The workaround to this is to use a proxy class.

## Derived classes ##
```
public class ClassA
{
}

public class ClassB: ClassA
{
  public int MethodToCache()
  {
    return 1;
  }
} 
```

becomes

```
public class ClassA
{
}

public class ClassB: ClassA
{
  public DateTime MethodToCache()
  {
    ClassBProxy proxy = new ClassBProxy();
    return proxy.MethodToCache();
  }
} 

[Cached]
internal class ClassBProxy: ContextBoundObject
{
  [CachedMethod(60)]
  internal DateTime MethodToCache()
  {
    return DateTime.Now;
  }
}
```