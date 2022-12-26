<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "input_8.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)
let inputArrs = Array.map (fun (line : string) -> line.ToCharArray()) inputLines

let strGrid = array2D inputArrs

let gridLength = Array.length inputLines
let gLMO = gridLength - 1

let isVisibleInSlice height ( slice : 'T [] when 'T : comparison ) =
    Array.forall (fun h -> height > h) slice

// Get directional slices
// I'm sure these quadruply-defined functions could be vastly tidied up, but I can't be bothered right now
let getUpSlice (grid : 'T [,]) x y =
    if y = 0 then Array.empty
    else grid[..y - 1, x]

let getDownSlice (grid : 'T [,]) x y =
    if y = gLMO then Array.empty
    else grid[y + 1.., x]
    
let getLeftSlice (grid : 'T [,]) x y = 
    if x = 0 then Array.empty
    else grid[y, ..x - 1]

let getRightSlice (grid : 'T [,]) x y =
    if x = gLMO then Array.empty
    else grid[y, x + 1..]

// Determine directional visibility
let isVisibleFromLeft x y h = 
    let slice = getLeftSlice strGrid x y
    isVisibleInSlice h slice

let isVisibleFromRight x y h = 
    let slice = getRightSlice strGrid x y
    isVisibleInSlice h slice

let isVisibleFromTop x y h =
    let slice = getUpSlice strGrid x y
    isVisibleInSlice h slice

let isVisibleFromBottom x y h = 
    let slice = getDownSlice strGrid x y
    isVisibleInSlice h slice

let isTreeVisible y x h =
    if x = 0 || x = gLMO || y = 0 || y = gLMO then
        true
    else
        isVisibleFromLeft x y h || isVisibleFromRight x y h || isVisibleFromTop x y h || isVisibleFromBottom x y h
        
// Count directionally visible trees        
let countTreesUp x y h = 
    let slice = getUpSlice strGrid x y |> Array.rev // By default, the slicing operation returns things in the opposite order to how we consider them on the diagram
    let idxOpt = Array.tryFindIndex (fun i -> i >= h) slice
    match idxOpt with
    | Some(idx) -> idx + 1
    | None -> Array.length slice
    
let countTreesDown x y h = 
    let slice = getDownSlice strGrid x y
    let idxOpt = Array.tryFindIndex (fun i -> i >= h) slice
    match idxOpt with
    | Some(idx) -> idx + 1
    | None -> Array.length slice
    
let countTreesLeft x y h = 
    let slice = getLeftSlice strGrid x y |> Array.rev
    let idxOpt = Array.tryFindIndex (fun i -> i >= h) slice
    match idxOpt with
    | Some(idx) -> idx + 1
    | None -> Array.length slice

let countTreesRight x y h = 
    let slice = getRightSlice strGrid x y
    let idxOpt = Array.tryFindIndex (fun i -> i >= h) slice
    match idxOpt with
    | Some(idx) -> idx + 1
    | None -> Array.length slice
        
let countVisibleTrees y x h =
    let up = countTreesUp x y h
    let dow = countTreesDown x y h
    let lef = countTreesLeft x y h
    let rig = countTreesRight x y h
    up * dow * lef * rig
        
let boolToNum b = if b then 1 else 0

// Taken from https://stackoverflow.com/a/12871091 -  to be honest, I don't really understand it...
let toArray (arr: 'T [,]) = arr |> Seq.cast<'T> |> Seq.toArray
        
let resultGrid = 
    strGrid |>
    Array2D.mapi isTreeVisible |>
    Array2D.map boolToNum
    
let result1 = toArray resultGrid |> Array.sum
printfn "Total visible trees is %d" result1

let resultGrid2 = 
    strGrid |>
    Array2D.mapi countVisibleTrees

let result2 = toArray resultGrid2 |> Array.max
printfn "Highest scenic score is %d" result2