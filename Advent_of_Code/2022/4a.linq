<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "input_4.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)

// This can definitely be done better, but I was not braining well at the time of solving it.

let rangesToInts (assignment : string) = assignment.Split('-', ',') |> Array.map int

let isWithin aLow aUpp bLow bUpp = bLow <= aLow && aUpp <= bUpp

let eitherWithin (ranges : int array) = (isWithin ranges[0] ranges[1] ranges[2] ranges[3]) || (isWithin ranges[2] ranges[3] ranges[0] ranges[1])

let withinToVal ranges =
    if eitherWithin ranges then
        1
    else
        0

let result = Array.sumBy (rangesToInts >> withinToVal) inputLines
printfn "Count of contained ranges is %d" result