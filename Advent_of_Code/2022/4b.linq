<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "input_4.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)

// This can definitely be done better, but I was not braining well at the time of solving it.

let rangesToInts (assignment : string) = assignment.Split('-', ',') |> Array.map int
        
let isOverlap aLow aUpp bLow bUpp = not (((aUpp < bLow) && (aLow < bUpp)) || ((bUpp < aLow) && (bLow < aUpp)))

let overlapTotal (ranges : int array) =
    if isOverlap ranges[0] ranges[1] ranges[2] ranges[3] then
        1
    else
        0

let result = Array.sumBy (rangesToInts >> overlapTotal) inputLines
printfn "Count of contained ranges is %d" result