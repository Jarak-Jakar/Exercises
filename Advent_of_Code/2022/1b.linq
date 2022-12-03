<Query Kind="FSharpProgram">
  <NuGetReference>FSharpPlus</NuGetReference>
  <Namespace>FSharpPlus</Namespace>
</Query>

open FSharpPlus

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "input_1.txt" |])
inputFilePath.Dump(nameof(inputFilePath))
let inputLines = File.ReadAllLines(inputFilePath) |> Seq.ofArray
//inputL.Dump(nameof(inputLines))

let splitted = Seq.split [ [ String.Empty ] ] inputLines
//splitted.Dump(nameof(splitted))

let answer = 
    splitted
    |> Seq.map ((Seq.map int) >> Seq.sum)
    |> Seq.sortDescending // This bit might be slow...  Could make it faster by tracking the three largest as we go
    |> Seq.take 3
    |> Seq.sum
          
printfn "Sum of three greatest calories is %d" answer