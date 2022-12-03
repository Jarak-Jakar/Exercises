<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "input_2.txt" |])
inputFilePath.Dump(nameof(inputFilePath))
let inputLines = File.ReadAllLines(inputFilePath)

type Move =
	| Rock
	| Paper
	| Scissors
    
type Result =
    | Win
    | Draw
    | Lose

let parseMove = function
	| "A" -> Rock
	| "B" -> Paper
	| "C" -> Scissors
	| x-> failwith "Non-valid move char"
    
let parseGoal = function
    | "X" -> Lose
    | "Y" -> Draw
    | "Z" -> Win
    | _ -> failwith "Non-valid goal char"
    
let chooseMove opponent result = 
    match opponent, result with
    | Rock, Win  | Paper, Draw | Scissors, Lose -> Paper
    | Rock, Draw | Paper, Lose | Scissors, Win -> Rock
    | Rock, Lose | Paper, Win  | Scissors, Draw -> Scissors

let scoreMove = function
 | Rock-> 1
 | Paper-> 2
 | Scissors-> 3

let scoreMatch opponent you =
	match opponent, you with
	| Rock, Scissors | Paper, Rock | Scissors, Paper-> 0
	| Rock, Rock | Paper, Paper | Scissors, Scissors-> 3
	| Rock, Paper | Paper, Scissors | Scissors, Rock-> 6

let splitOnSpace (inps : string) = inps.Split(' ')

let processLine [| a; b |] =
    let oppM = parseMove a
    let goal = parseGoal b
    let choice = chooseMove oppM goal
    [| oppM; choice |]

let scoreRound (round: Move array) =
	let moveSc = scoreMove round[1]
	let matchSc = scoreMatch round[0] round[1]
	moveSc + matchSc

let score =
    inputLines
    |> Array.map ( splitOnSpace >> processLine >> scoreRound )
    |> Array.sum

printfn "Total score is %d" score