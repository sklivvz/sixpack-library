![http://sites.google.com/site/sixpacklibrary/_/rsrc/1237030016786/config/app/images/customLogo/customLogo.gif](http://sites.google.com/site/sixpacklibrary/_/rsrc/1237030016786/config/app/images/customLogo/customLogo.gif)

The SixPack rapid development library is a collection of classes for rapid development on the .NET/Mono platform. It has been built with medium to large scale sites in mind, and supports high performance requirements (thousands of concurrent users per server.)

[Version 1.2 released](Changelog.md)

The SixPack library can now be installed using the NuGet package manager.

http://nuget.org/Packages/Search?packageType=Packages&searchCategory=All+Categories&searchTerm=sixpack

## Main Features ##
  * Almost completely transparent [Caching](Caching.md) system (Simple, thread safe cache that can be used easily on existing systems, or a more advanced Caching system with prefetching)
  * Full, enterprise grade, [stored procedure support](StoredProcedure.md) (including deadlock retries, stored procedure timings)
  * Internationalization support (Portugal only, submissions welcome)
  * Minimal, high performance [logging class](Log.md) supporting filesystem and email
  * CAPTCHA generator
  * High quality image sizing (can perform scaling and thumbnailing for sites in real time)
  * XML over HTTP client (includes support for XSLTs, results as DataTable or Class)
  * Code Generation (programmatically define a class and output source code)
  * Small RSA implementation (generate billions of unique short codes for promotions in minutes)
  * Text Manipulation (string clipping, sanitization)
  * Emailing system (based on CDO and extending it to work with streams)
  * Validation (Email address, Credit Cards, ...)
  * Useful extensions to Entity Framework
  * Helper methods for reflection and manipulation of expression trees

The library has been build upon solid architectural principles:
  * **DON'T MAKE ME THINK** -- We strive to be invisible and do not surprise developers.
  * **KISS** -- We strive to keep everything simple and stupid.
  * **YAGNI** -- We strive to give you only what you are going to need.

## General Stuff ##
  * 73 new classes, 15,000 fully commented lines of code
  * FxCop 1.36 compliant
  * Unit tested
  * Field tested on dozens of [projects](Projects.md)
  * Load tested
  * Standards compliant