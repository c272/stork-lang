![storklogo](https://i.imgur.com/5yYjzC9.png)

![deps](https://img.shields.io/badge/dependencies-none-green.svg)      ![license](https://img.shields.io/badge/license-MIT-blue.svg) ![version](https://img.shields.io/badge/version-v0.04-orange.svg) ![support](https://img.shields.io/badge/platform-c%23.net%20%3E%3D%207-lightgrey.svg)

*A programming language which aims to make mundane tasks less mundane.*
## Introduction
Stork is a C-like programming language that aims to make the tasks that would be considered "boilerplate code" easier, by stripping the required work down, and by exporting the tasks into a vast standard library.

The language is currently not ready for general use, and is currently in the lexing/action tree stage of development. If, however, you want to contribute to the library, feel free to fork and make a pull request, which are considered on a case-by-case basis. Please remember to check the pull request templates beforehand.

## Building Stork
To build the interpreter or transpiler for Stork, you need at least Visual Studio 2015 (Update 3) or higher. Since Stork uses .NET Standard, this is not compileable (at least through traditional methods) on Linux or Mac.

(*Note: The transpiler **will** output files that are compileable on Mac and Linux, however the transpiler itself and the interpreter are strictly Windows-only.*)

Building in "Debug" mode is recommended for all contributions to the update branch, but if you are using it for a personal modification, using "Release" is a better option, for size and compute efficiency.

## Code Examples
**The basic "Hello World" program in Stork:**

    ~import "log";
    log("Hello World");

**C-like "if" and preprocessor statements.**

    ~preprocesscommand "parameter1" "parameter2";
    if (somestatement) {
	    dosomething();
    }

## Using the Interpreter
For debugging purposes, there is a rudimentary interpreter in place. Please note that this language is intended to be compiled, and this will likely be discontinued in the future when the transpiler and compiler are finished and functional with the latest builds.

You can use the interpreter like so:

    stki somefile.stork
And for further command help,

    stki --help
