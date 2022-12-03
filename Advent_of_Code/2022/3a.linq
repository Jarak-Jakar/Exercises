<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "input_3.txt" |])
inputFilePath.Dump(nameof(inputFilePath))
let inputLines = File.ReadAllLines(inputFilePath)

// There's almost certainly a better way to do this (without resorting to making assumptions about ASCII code points), but I can't think of it right now
let alphabet = [| 'a'; 'b'; 'c'; 'd'; 'e'; 'f'; 'g'; 'h'; 'i'; 'j'; 'k'; 'l'; 'm'; 'n'; 'o'; 'p'; 'q'; 'r'; 's'; 't'; 'u'; 'v'; 'w'; 'x'; 'y'; 'z'; |]

let halve (line : string) = line.ToCharArray() |> Array.splitAt (line.Length / 2)

// I'm pretty sure this whole thing is super inefficient, but it makes it easy
let intersect (a,b) =
    let c = Set.ofArray a
    let d = Set.ofArray b
    Set.toArray (Set.intersect c d)
    
let prioritise item =
    let a = Char.ToLower item
    let pos = (Array.findIndex (fun b -> b = a) alphabet) + 1
    if Char.IsUpper item then
        pos + 26
    else
        pos

let intersected = Array.collect (halve >> intersect) inputLines

let prioritised = Array.map prioritise intersected

let summation = Array.sum prioritised
printfn "Total priority is %d" summation