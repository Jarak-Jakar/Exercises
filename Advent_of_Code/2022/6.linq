<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "input_6.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)

let input = Array.exactlyOne inputLines

let inputArr = input.ToCharArray()

let isCharReplicatedInWindow windowSize window = 
    Array.sortInPlace window
    let mutable isReplicated = false
    for i = 1 to (windowSize - 1) do
        if window[i - 1] = window[i] then
            isReplicated <- true
    
    isReplicated
    
let findMarkerIdx windowSize input =
    let windows = Seq.windowed windowSize input
    let falseIndex = Seq.findIndex ((isCharReplicatedInWindow windowSize) >> not) windows
    falseIndex + windowSize // windowSize - 1 because we skip that many chars at the start, and 1 because of zero-based indexing
    
    
// Part A
let resultA = findMarkerIdx 4 inputArr
printfn "The first marker for part A is found at %d" resultA

// Part B
let resultB = findMarkerIdx 14 inputArr
printfn "The first marker for part B is found at %d" resultB
