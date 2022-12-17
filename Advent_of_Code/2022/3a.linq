<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "input_3.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)

// There's almost certainly a better way to do this (without resorting to making assumptions about ASCII code points), but I can't think of it right now
let alphabet = [| 'a'..'z'; |]

let halve (line : string) = line.ToCharArray() |> Array.splitInto 2

let intersect lines =
    lines |> Array.map Set |> Set.intersectMany |> Seq.head
    
let prioritise item =
    let a = Char.ToLower item
    let pos = (Array.IndexOf(alphabet, a)) + 1
    if Char.IsUpper item then
        pos + 26
    else
        pos

let summation = Array.sumBy (halve >> intersect >> prioritise) inputLines
printfn "Total priority is %d" summation