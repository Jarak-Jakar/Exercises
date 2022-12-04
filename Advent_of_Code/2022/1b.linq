<Query Kind="FSharpProgram">
  <NuGetReference>FSharpPlus</NuGetReference>
  <Namespace>FSharpPlus</Namespace>
</Query>

open FSharpPlus

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "input_1.txt" |])
inputFilePath.Dump(nameof(inputFilePath))
let inputLines = File.ReadAllLines(inputFilePath) |> Seq.ofArray

// I originally had a little recursive function to do the splitting, but swapped it out for the much easier and tidier FSharpPlus solution.
// If you're interested in my (probably garbage) recursive solution, it should be way back in the git history :)
let splitted = Seq.split [ [ String.Empty ] ] inputLines

let answer = 
    splitted
    |> Seq.map (Seq.sumBy int)
    |> Seq.sortDescending // This bit will be slow...  Could make it faster by tracking the three largest as we go instead
    |> Seq.take 3
    |> Seq.sum
          
printfn "Sum of three greatest calories is %d" answer