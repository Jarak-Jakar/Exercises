<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "example_8.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)
let inputArrs = Array.map (fun (line : string) -> line.ToCharArray()) inputLines

let jj = '4'
let kk = '7'
printfn "%A" (jj < kk)

//inputArrs.Dump("inputArrs")

let strGrid = array2D inputArrs
strGrid.Dump("strGrid")
// I freaking hate having to go back to strings here, but it seems to be the easiest way to get the number parsing working
//let grid = Array2D.map (string >> int) strGrid
//grid.Dump("grid")

let gridLength = Array.length inputLines
let gLMO = gridLength - 1
let gLMT = gridLength - 2

let slice' = strGrid[1..gLMT,0]
slice'.Dump("slice'")

let isVisibleInSlice height ( slice : 'T [] when 'T : comparison ) =
    //let height = slice[pos - 1]
    Array.forall (fun h -> height > h) slice
    
let getUpSlice (grid : 'T [,]) x y =
    if y = 0 then Array.empty
    else grid[..y - 1, x]
    
//let usli = getUpSlice grid 0 1
//usli.Dump("usli")

let getDownSlice (grid : 'T [,]) x y =
    if y = gLMO then Array.empty
    else grid[y + 1.., x]
    
let dsli = getDownSlice strGrid 0 0
dsli.Dump("dsli")

let isVisibleFromLeft x y h = false

let isVisibleFromRight x y h = false

let isVisibleFromTop x y h =
    let slice = getUpSlice strGrid x y
    isVisibleInSlice h slice

let isVisibleFromBottom x y h = 
    let slice = getDownSlice strGrid x y
    isVisibleInSlice h slice

let isTreeVisible x y h =
    if x = 0 || x = gLMO || y = 0 || y = gLMO then
        true
    else
        isVisibleFromLeft x y h || isVisibleFromRight x y h || isVisibleFromTop x y h || isVisibleFromBottom x y h
        
let boolToNum b = if b then 1 else 0
        
let joe = 
    strGrid |>
    Array2D.mapi isTreeVisible |>
    Array2D.map boolToNum
    
joe.Dump()