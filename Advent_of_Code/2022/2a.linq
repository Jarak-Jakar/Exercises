<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "input_2.txt" |])
inputFilePath.Dump(nameof(inputFilePath))
let inputLines = File.ReadAllLines(inputFilePath)

type Move = | Rock | Paper | Scissors
    
let parseMove = function
    | "A" | "X" -> Rock
    | "B" | "Y" -> Paper
    | "C" | "Z" -> Scissors
    | _ -> failwith "Invalid move char"
    
let scoreMove = function
    | Rock -> 1
    | Paper -> 2
    | Scissors -> 3
    
let scoreMatch opponent you =
    match opponent, you with
    | Rock, Scissors | Paper, Rock     | Scissors, Paper -> 0
    | Rock, Rock     | Paper, Paper    | Scissors, Scissors -> 3
    | Rock, Paper    | Paper, Scissors | Scissors, Rock -> 6
    
let splitOnSpace (inps : string) = inps.Split(' ')

let parseLine = Array.map parseMove

let scoreRound (round : Move array) =
    let moveSc = scoreMove round[1]
    let matchSc = scoreMatch round[0] round[1]
    moveSc + matchSc

let score =
    inputLines
    |> Array.sumBy (splitOnSpace >> parseLine >> scoreRound)
    
printfn "Total score is %d" score