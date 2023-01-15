<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "input_7.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)

type File = { name: string; size: int }
type Directory = { name: string; mutable files: File list; parent: Option<Directory>; mutable children: Map<string, Directory>; }

type Command = 
    | CdRoot
    | CdUp
    | CdDn of string
    | Ls of string[]

let rec computeDirectorySize dir =
    let subdirsSize = Seq.sumBy computeDirectorySize (Map.values dir.children)
    let filesSize = List.sumBy (fun f -> f.size) dir.files
    subdirsSize + filesSize

let rec sizeChoose maxSize dir = 
    let chosenChildSizes = Seq.map (sizeChoose maxSize) dir.children.Values
    let totalChosenChildSizes = Seq.sum chosenChildSizes
    let size = computeDirectorySize dir
    totalChosenChildSizes + if size <= maxSize then size else 0

let parseCd (cmd : string) =
    if cmd.EndsWith("/") then CdRoot
    else if cmd.EndsWith("..") then CdUp
    else
        let split = cmd.Split()
        CdDn split[2]
        
let parseLs arr =
    let listings = Array.takeWhile (fun (l : string) -> l.StartsWith("$") |> not) arr
    
    let listingsLength = Array.length listings
    
    (Ls listings, Array.skip listingsLength arr)
    
let parseCommand (cmd : string) arr =
    match cmd[..3] with
    | "$ cd" -> (parseCd cmd, arr)
    | "$ ls" -> parseLs arr
    | _ -> failwith "Unrecognised command"

let parseCommands arr =
    let mutable cmds = List.empty
    let mutable remainingCmds = arr
    while Array.isEmpty remainingCmds |> not do
    
        let nextCmd = Array.head remainingCmds
        
        remainingCmds <- Array.tail remainingCmds
        
        let cmd, rest = parseCommand nextCmd remainingCmds
        cmds <- cmd :: cmds
        remainingCmds <- rest
        
    List.rev cmds

let processL currDir (l : string) = 
    let split = l.Split()
    if String.Equals(split[0], "dir") then
        if currDir.children.ContainsKey split[1] |> not then
            let newDir = { name = split[1]; files = List.empty; parent = Some(currDir); children = Map.empty }
            currDir.children <- currDir.children.Add(split[1], newDir)
    else
        let newFile = { name = split[1]; size = split[0] |> int }
        currDir.files <- newFile :: currDir.files

let processLs ls currDir = 
    Array.iter (processL currDir) ls    
    currDir

let processCmd root currDir cmd =
    match cmd with
    | CdRoot -> root
    | CdUp -> currDir.parent.Value // I'm assuming "cd .." won't be called at root
    | CdDn d -> currDir.children[d]
    | Ls ls -> processLs ls currDir
    
let root = { name = "root"; files = List.empty; parent = None; children = Map.empty }
    
let cmds = parseCommands inputLines

let foldFunc currDir cmd = 
    processCmd root currDir cmd

let _ = List.fold foldFunc root cmds // The underscore is a clear sign I'm not doing it very 'functionally'...
let maxSize = 100_000
let filteredChildrenSize = sizeChoose maxSize root

let result1 = filteredChildrenSize
printfn "Total filtered size is %d" result1

let occupiedSpace = computeDirectorySize root
let totalSpace = 70_000_000
let requiredSpace = 30_000_000
let freeSpace = totalSpace - occupiedSpace
let minSpaceToFree = requiredSpace - freeSpace

let rec sizeChoose2 minSize dir = 
    let chosenChildSizes = Seq.collect (sizeChoose2 minSize) dir.children.Values
    let size = computeDirectorySize dir
    if size >= minSize then Seq.append chosenChildSizes (Seq.singleton size) else chosenChildSizes

let result2 = sizeChoose2 minSpaceToFree root |> Seq.min
printfn "Smallest sufficiently-large dir has size %d" result2