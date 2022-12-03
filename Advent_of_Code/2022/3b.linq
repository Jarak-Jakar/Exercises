<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "input_3.txt" |])
inputFilePath.Dump(nameof(inputFilePath))
let inputLines = File.ReadAllLines(inputFilePath)

// I LOATHE this doubly-nested Array.map, but I'm struggling to think of a better tidy way to do it at the moment :(
let inputGroups = Array.chunkBySize 3 inputLines |> Array.map (fun x -> Array.map (fun (y : string) -> y.ToCharArray()) x)

// There's almost certainly a better way to do this (without resorting to making assumptions about ASCII code points), but I can't think of it right now
let alphabet = [| 'a'; 'b'; 'c'; 'd'; 'e'; 'f'; 'g'; 'h'; 'i'; 'j'; 'k'; 'l'; 'm'; 'n'; 'o'; 'p'; 'q'; 'r'; 's'; 't'; 'u'; 'v'; 'w'; 'x'; 'y'; 'z'; |]

// I'm pretty sure this whole thing is super inefficient, but it makes it easy
let intersect arrs =
	let sets = Array.map Set.ofArray arrs
    let intersection = Set.intersectMany sets
	Set.toArray intersection

let prioritise item =
	let a = Char.ToLower item
	let pos = (Array.findIndex (fun b->b = a) alphabet) + 1
	if Char.IsUpper item then
		pos + 26
	else
	    pos

let intersected = Array.collect intersect inputGroups

let summation = intersected |> Array.map prioritise |> Array.sum
printfn "Total priority is %d" summation