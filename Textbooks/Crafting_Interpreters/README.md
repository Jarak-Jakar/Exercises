# Crafting Interpreters

## By Bob Nystrom

ISBN:  9780990582939
Publisher:  Genever Benning
Year:  2021
(link to purchase from [Blackwell's UK](https://blackwells.co.uk/bookshop/product/9780990582939))

My answers/solutions to the challenges presented at the end of each chapter of *Crafting Interpreters* by Bob Nystrom.  I am reading through the paperback edition, but using the online web version as the reference for the challenges.

## Development Environments & Editors

I use Windows for my day-to-day computating, which sometimes necessitates using things like containers (e.g. Docker) or virtual machines, either directly through something like Virtualbox, or indirectly through something like Vagrant.

### Java

#### Environment

I looked around at a few options for this, but ulitmately found that most of them were more hassle than they were worth.  Turns out that installing Java into a Linux VM is much more complicated than it seems like it shoudl be ...  In the end, I just installed a version of the JDK directly into Windows.  Specifically, the printout from running `java --version` gave the following output:

```bash
openjdk 17.0.5 2022-10-18
OpenJDK Runtime Environment Temurin-17.0.5+8 (build 17.0.5+8)
OpenJDK 64-Bit Server VM Temurin-17.0.5+8 (build 17.0.5+8, mixed mode, sharing)
```

#### Editor

JetBrains IntelliJ Community Edition.  I started with the Ultimate edition at first while I had an educational pack with JetBrains, but that expired partway through so I fell back to using the Community version.

### C

#### Environment

I simply used the latest version of GCC available in my chosen [WSL distribution](https://learn.microsoft.com/en-nz/windows/wsl/about).  There wasn't really any point in fussing around with something more advanced or complicated for relatively simple C programming.  In my case, I have been using the OpenSUSE Tumbleweed distro, but I don't think there's any terribly significant differences between most WSL for these purposes.

#### Editor

I used Visual Studio Code with the C/C++ extension published by Microsoft.

## Future Efforts?

I *might* come back in the future and make my own re-implementations of a Lox interpreter in other languages that interest me.  Especially if they claim to be a systems programming language (e.g. Rust, Zig, D, Red, Carbon to name some relatively current ones).
