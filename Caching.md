http://sites.google.com/site/sixpacklibrary/_/rsrc/1237036619924/Home/sixpack-icon.gif?height=48&width=48
# Introduction to caching #

Here is a basic example of how to use caching:

```
using System;
using System.Threading;
using SixPack.ComponentModel;
namespace SixPack.Examples
{
	public class MainClass
	{
		public static void Main(string[] args)
		{
			MyTime myTime = new MyTime();

			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine("{0:HH:mm:ss:fffff}", myTime.Get());
				Console.WriteLine("{0:HH:mm:ss:fffff}", myTime.GetNoCache());
				Thread.Sleep(600);
			}
		}
	}

	[Cached]
	public class MyTime : ContextBoundObject
	{
		[CachedMethod(1)]
		public DateTime Get()
		{
			Console.WriteLine("Get invoked.");
			return DateTime.Now;
		}
	
		public DateTime GetNoCache()
		{
			Console.WriteLine("GetNoCache invoked.");
			return DateTime.Now;
		}
	}
}
```

Result:
```
marco@marco-laptop:~/develop/test$ mono cache.exe 
[Cache] Starting.
[Cache] Using local caching. Cached items will not expire.
Get invoked.
08:53:20:21415
GetNoCache invoked.
08:53:20:26567
08:53:20:21415
GetNoCache invoked.
08:53:20:86702
08:53:20:21415
GetNoCache invoked.
08:53:21:46841
08:53:20:21415

...
```

# Notes #
  * Without an HttpContext loaded, the class uses a dictionary for caching, therefore cached items will not expire.
  * You have to derive from ContextBoundObject to make the Cache work. For examples on how to use caching when you can't do this see AdvancedCaching.
  * Your methods have to be non-static. For examples on how to use caching with static methods see AdvancedCaching.