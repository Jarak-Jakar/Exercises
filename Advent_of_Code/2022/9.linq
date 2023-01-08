<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "input_9.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)

type Point = { X: int; Y: int }
type State = { Head: Point; Tail: Point; Visited: Set<Point> }

type Movement =
    | Up
    | Down
    | Left
    | Right
    
let parseMove (move : string) =
    let split = move.Split()
    if (Array.length split) <> 2 then
        failwith "Invalid command shape provided"
    else
        let count = Int32.Parse split[1]
        let movement = 
            match split[0] with
            | "U" -> Up
            | "D" -> Down
            | "L" -> Left
            | "R" -> Right
            | _ -> failwith "Unrecognised command supplied"
            
        Array.create count movement
        
let applyMove (point : Point) = function
    | Up -> { point with Y = point.Y + 1 }
    | Down -> { point with Y = point.Y - 1 }
    | Left -> { point with X = point.X - 1 }
    | Right -> { point with X = point.X + 1 }
    
let isTailTouchingHead state =
    let sameSpace = state.Head = state.Tail
    
    let xDiff = state.Head.X - state.Tail.X
    let yDiff = state.Head.Y - state.Tail.Y
    
    let oneAway = (Int32.Abs xDiff) <= 1 && (Int32.Abs yDiff) <= 1
    
    (sameSpace || oneAway), xDiff, yDiff
    
let computeTailMovement point xDiff yDiff =
    if (Int32.Abs xDiff) > (Int32.Abs yDiff) then
        { point with X = point.X + (xDiff / 2); Y = point.Y + yDiff }
    else
        { point with X = point.X + xDiff; Y = point.Y + (yDiff / 2) }
    
let chaseHead state =
    let isTouching, xDiff, yDiff = isTailTouchingHead state
    if isTouching then
        state.Tail
    else
        computeTailMovement state.Tail xDiff yDiff
        
let moves = Array.collect parseMove inputLines

let state = { Head = { X = 0; Y = 0 }; Tail = { X = 0; Y = 0}; Visited = Set.singleton { X = 0; Y = 0 } }

let runMoveApplication state move =
    // Apply move to head
    let headMoved = { state with Head = applyMove state.Head move; }
    
    // Update tail's position to follow head
    let tailMoved = { headMoved with Tail = chaseHead headMoved }
    
    // Add new tail position to the visited set and return new state
    { tailMoved with Visited = Set.add tailMoved.Tail tailMoved.Visited }
    
let finalState = Array.fold runMoveApplication state moves

let result = Set.count finalState.Visited
printfn "Tail visited %d spaces" result