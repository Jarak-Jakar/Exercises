<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "example_9.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)

type Point = { X: int; Y: int }
type State = { Head: Point; Tail: Point; Visited: Set<Point> }
type StatePart2 = { Rope: Point[]; Visited: Set<Point> }

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
    
let isTailTouchingHead head tail =
    let sameSpace = head = tail
    
    let xDiff = head.X - tail.X
    let yDiff = head.Y - tail.Y
    
    let oneAway = (Int32.Abs xDiff) <= 1 && (Int32.Abs yDiff) <= 1
    
    (sameSpace || oneAway), xDiff, yDiff
    
let computeTailMovement point xDiff yDiff =
    if (Int32.Abs xDiff) > (Int32.Abs yDiff) then
        { point with X = point.X + (xDiff / 2); Y = point.Y + yDiff }
    else
        { point with X = point.X + xDiff; Y = point.Y + (yDiff / 2) }
    
let chaseHead head tail =
    let isTouching, xDiff, yDiff = isTailTouchingHead head tail
    if isTouching then
        tail
    else
        computeTailMovement tail xDiff yDiff
        
let moves = Array.collect parseMove inputLines

// Part One

let state = { Head = { X = 0; Y = 0 }; Tail = { X = 0; Y = 0}; Visited = Set.singleton { X = 0; Y = 0 } }

let runMoveApplication state move =
    // Apply move to head
    let headMoved = { state with Head = applyMove state.Head move; }
    
    // Update tail's position to follow head
    let tailMoved = { headMoved with Tail = chaseHead headMoved.Head headMoved.Tail }
    
    // Add new tail position to the visited set and return new state
    { tailMoved with Visited = Set.add tailMoved.Tail tailMoved.Visited }
    
let finalState = Array.fold runMoveApplication state moves

let result = Set.count finalState.Visited
printfn "Part One: Tail visited %d spaces" result

// Part Two
let startingRope = Array.init 10 (fun _ -> { X = 0; Y = 0 })
let stateTwo = { Rope = startingRope; Visited = Set.singleton { X = 0; Y = 0 } }

let runMoveRope state move =
    
    state.Rope.Dump("state.Rope")
    
    state.Rope[0] <- applyMove state.Rope[0] move
    
    for i = 1 to (Array.length state.Rope) - 1 do
        state.Rope[i] <- chaseHead state.Rope[i - 1] state.Rope[i]
        
    let tail = Array.last state.Rope
    { state with Visited = Set.add tail state.Visited }
    
let finalStateTwo = Array.fold runMoveRope stateTwo moves
printfn "Part Two: Tail visited %d spaces" (Set.count finalStateTwo.Visited)
//finalStateTwo.Rope.Dump("fST Rope")

// Sanity check with part one

let startingRopePartOne = Array.init 2 (fun _ -> { X = 0; Y = 0; })
let stateRedux = { Rope = startingRopePartOne; Visited = Set.singleton { X = 0; Y = 0 } }
let finalStateRedux = Array.fold runMoveRope stateRedux moves
printfn "Part One Redux: Tail visited %d spaces" (Set.count finalStateRedux.Visited)

//finalStateRedux.Rope.Dump("finalStateRedux Rope")
    