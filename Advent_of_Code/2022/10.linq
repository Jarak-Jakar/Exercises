<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "input_10.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)

type Instruction =
    | Noop
    | Addx of int
    
let makeAddx (inst : string) = 
    let split = inst.Split()
    if (Array.length split) <> 2 then
        failwith "Invalid Addx instruction passed in"
    else
        let num = Int32.Parse(split[1])
        Addx num
    
let parseInstruction (inst : string) =
    let lcInst = inst.ToLowerInvariant()
    match lcInst[..3] with
    | "noop" -> Noop
    | "addx" -> makeAddx inst
    | _ -> failwith "Unrecognised command passed in."
    
let instructions = Array.map parseInstruction inputLines

let makeStep instruction =
    match instruction with
    | Noop -> seq { Noop }
    | Addx x -> seq {Addx 0; Addx x}

let processSteps register step =
    match step with
    | Noop -> register
    | Addx x -> register + x

let computeSignalStrength (cycle, register) = cycle * register

let indicesOfInterest = seq { 20; 60; 100; 140; 180; 220 }
let startingX = 1
    
//let steps = Seq.collect makeStep instructions    
//let appliedSteps = Seq.scan processSteps startingX steps
//let indexedSteps = Seq.indexed appliedSteps |> Seq.map (fun (i,v) -> (i + 1, v)) // Add 1 to each index
//let interestingSteps = Seq.filter (fun (i, _) -> Seq.contains i indicesOfInterest) indexedSteps

//let interestingSteps = 
//    instructions
//    |> Seq.collect makeStep
//    |> Seq.scan processSteps startingX
//    |> Seq.indexed
//    |> Seq.map (fun (i,v) -> (i + 1, v)) // Use 1-based indexing
//    |> Seq.filter (fun (i, _) -> Seq.contains i indicesOfInterest)

let result = 
    instructions
    |> Seq.collect makeStep
    |> Seq.scan processSteps startingX
    |> Seq.indexed
    |> Seq.map (fun (i,v) -> (i + 1, v)) // Use 1-based indexing
    |> Seq.filter (fun (i, _) -> Seq.contains i indicesOfInterest)
    |> Seq.map computeSignalStrength 
    |> Seq.sum

//let result = interestingSteps |> Seq.map computeSignalStrength |> Seq.sum
printfn "The summed signal strength  is %d" result
