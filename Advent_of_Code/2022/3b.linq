<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "input_3.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)

// There's almost certainly a better way to do this (without resorting to making assumptions about ASCII code points), but I can't think of it right now
let alphabet = [| 'a'..'z'; |]

let prioritise item =
    let a = Char.ToLower item
    let pos = (Array.IndexOf(alphabet, a)) + 1
    if Char.IsUpper item then
        pos + 26
    else
        pos

let summation = inputLines 
                |> Seq.map Set // works because strings are already regarded as collections in .NET (I think that's why)
                |> Seq.chunkBySize 3
                |> Seq.sumBy (Set.intersectMany >> Seq.head >> prioritise)

printfn "Total priority is %d" summation