<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "input_5.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)

let emptyLineIdx = Array.IndexOf(inputLines, String.Empty)

let diagram, steps' = Array.splitAt emptyLineIdx inputLines

let diagramStacks, stackNumbers = Array.splitAt ((Array.length diagram) - 1) diagram

let stackLen = stackNumbers[0] |> fun x -> x.Split([|' '|], StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries) |> Array.length
let (stacks : char list array) = Array.create stackLen List.Empty

for i = ((Array.length diagramStacks) - 1) downto 0 do
    for j = 0 to (stackLen - 1) do
        let entry = diagramStacks[i][1 + (j * 4)]
        if Char.IsLetter entry then
            stacks[j] <- entry :: stacks[j]

let stepsRaw = Array.skip 1 steps' // Trim out the blank separation line

type crate = char
type crateStack = crate list
type step = {count: int; src: int; dst: int}

// I SERIOUSLY doubt this is the best way to do this, but it works and lets me kinda use pattern matching for it.
let parseStep stepStr =
    let stepRegex = "move (?<count>\d+) from (?<src>\d+) to (?<dst>\d+)"
    
    let r = new Regex(stepRegex, RegexOptions.IgnoreCase ||| RegexOptions.NonBacktracking, TimeSpan.FromMilliseconds(100.0))
    let m = r.Match(stepStr).Groups
    // Minus one to account for the zero-based indexing of the stack array
    { count = int m["count"].Value; src = (int m["src"].Value) - 1; dst = (int m["dst"].Value) - 1 }
    
let steps = Array.map parseStep stepsRaw

for step in steps do
    let top = List.take step.count stacks[step.src]
    stacks[step.src] <- List.skip step.count stacks[step.src]
    
    stacks[step.dst] <- List.append (List.rev top) stacks[step.dst]
    
let result = Array.map List.head stacks
printfn "The tops of the stacks are %s" (new String(result))