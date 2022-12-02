<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "input_1.txt" |])
inputFilePath.Dump(nameof(inputFilePath))
let inputLines = File.ReadAllLines(inputFilePath) |> List.ofArray
//inputL.Dump(nameof(inputLines))

let split input divider =
    let rec splitHelper inp div outp =

        if List.isEmpty inp then
            List.rev outp
    	else
            let idx = List.tryFindIndex(fun l->l = div) inp
            match idx with
            | Some(i)->splitHelper(List.skip(i + 1) inp) div ((List.take i inp) :: outp)
            | None->List.rev(inp::outp)

    splitHelper input divider List.Empty

let splitted = split inputLines ""
//splitted.Dump(nameof(splitted))

let answer = 
    splitted
    |> List.map ((List.map int) >> List.sum)
    |> List.max
          
printfn "Greatest calories is %d" answer