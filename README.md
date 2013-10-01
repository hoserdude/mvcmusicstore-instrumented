mvcmusicstore-instrumented
==========================

Yet another version of the MVC Music Store

## Why this version?
* The example was lacking a dependency injection framework
* The example was lacking a logging library
* The example was creating a less than ideal architecture and needed to be cleaned up (WIP)

## What's different
* added dependency injection (Unity)
* added Logging library (Log4Net)
* configured a instrumentation friendly log pattern
* configured interception to allow for instrumentation at the object call level
* configured an instrumentation filter at the controller level to instrument that tier

## TODO
* Properly architect the tiers in the app (ie, add a real service layer)
* Bring app up to the latest EF framework and best practices
* Convert to SPA UI


