<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "example_10.txt" |])
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
instructions.Dump("instructions")

type Cpu = { Cycle: int; Register: int; Delay: int; CurrInst: Instruction } // Where delay is how many cycles remain before the instruction is applied

let cpu = { Cycle = 0; Register = 1; Delay = 0; CurrInst = Array.head instructions }

let processNextInstruction cpu instruction = 
    if cpu.Delay > 0 then
        { cpu with Cycle = cpu.Cycle + 1; Delay = cpu.Delay - 1}
    else
        match instruction with
        | Noop -> { cpu with Cycle = cpu.Cycle + 1; Delay = 0 }