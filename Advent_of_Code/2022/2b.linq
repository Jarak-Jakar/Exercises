<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "input_2.txt" |])
inputFilePath.Dump(nameof(inputFilePath))
let inputLines = File.ReadAllLines(inputFilePath)

type Move = | Rock | Paper | Scissors
    
type Result = | Win | Draw | Lose

let parseMove = function
    | "A" -> Rock
    | "B" -> Paper
    | "C" -> Scissors
    | _ -> failwith "Invalid move char"
    
let parseGoal = function
    | "X" -> Lose
    | "Y" -> Draw
    | "Z" -> Win
    | _ -> failwith "Invalid goal char"
    
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
    | Rock, Scissors | Paper, Rock     | Scissors, Paper-> 0
    | Rock, Rock     | Paper, Paper    | Scissors, Scissors-> 3
    | Rock, Paper    | Paper, Scissors | Scissors, Rock-> 6

let splitOnSpace (inps : string) = inps.Split(' ')

let processLine [| a; b |] = // I don't like getting the warning here, but I can't be bothered doing any more to avoid it.
    let oppM = parseMove a
    let goal = parseGoal b
    let choice = chooseMove oppM goal
    (oppM, choice)

let scoreRound (them, me) =
    let moveSc = scoreMove me
    let matchSc = scoreMatch them me
    moveSc + matchSc

let score =
    inputLines
    |> Array.sumBy (splitOnSpace >> processLine >> scoreRound)

printfn "Total score is %d" score