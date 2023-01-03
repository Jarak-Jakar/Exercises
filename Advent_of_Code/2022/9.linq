<Query Kind="FSharpProgram" />

// Since I somewhat struggle to follow the explanation for day 9, I am skipping it for the time being.
let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "example_9.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)