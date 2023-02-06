# How to run/test

To execute a test program, `cd` into the `jlox` src directory, and execute

```powershell
java -classpath "..\..\out\production\jlox" jlox.Lox ..\sample-program.lox
```

## (or, alternatively)

To execute a test program, `cd` into the `src` directory, and execute

```powershell
java -classpath "..\out\production\jlox" jlox.Lox sample-program.lox
```

Chop off the `sample-program.lox` bit at the end to run the REPL.