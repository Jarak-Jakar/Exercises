<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "input_10.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)