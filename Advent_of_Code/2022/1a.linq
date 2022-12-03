<Query Kind="FSharpProgram">
  <NuGetReference>FSharpPlus</NuGetReference>
  <Namespace>FSharpPlus</Namespace>
</Query>

open FSharpPlus

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "input_1.txt" |])
inputFilePath.Dump(nameof(inputFilePath))
let inputLines = File.ReadAllLines(inputFilePath) |> Seq.ofArray
//inputL.Dump(nameof(inputLines))

let splitted = Seq.split [ [ String.Empty ] ] inputLines // I'm not sure why it requires double-nesting of string.empty
//splitted.Dump(nameof(splitted))

let answer = 
    splitted
    |> Seq.map ((Seq.map int) >> Seq.sum)
    |> Seq.max
          
printfn "Greatest calories is %d" answer