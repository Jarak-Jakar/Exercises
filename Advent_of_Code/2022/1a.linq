<Query Kind="FSharpProgram">
  <NuGetReference>FSharpPlus</NuGetReference>
  <Namespace>FSharpPlus</Namespace>
</Query>

open FSharpPlus

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "input_1.txt" |])
inputFilePath.Dump(nameof(inputFilePath))
let inputLines = File.ReadAllLines(inputFilePath) |> Seq.ofArray

let splitted = Seq.split [ [ String.Empty ] ] inputLines // I'm not sure why it requires double-nesting of string.empty - I imagine I don't understand the split function well enough

let answer = 
    splitted
    |> Seq.map (Seq.sumBy int)
    |> Seq.max // Pretty sure this could become a 'maxBy', but I can't be bothered
          
printfn "Greatest calories is %d" answer