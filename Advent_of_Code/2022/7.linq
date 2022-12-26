<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "example_7.txt" |])
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
    
let root = { name = "root"; files = List.empty; parent = None; children = Map.empty }

let parseCd (cmd : string) =
    if cmd.EndsWith("..") then CdUp
    else if cmd.EndsWith("/") then CdRoot
    else
        let split = cmd.Split()
        CdDn split[1]
        
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
    
let cmds = parseCommands inputLines

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

let processCmd cmd currDir =
    match cmd with
    | CdRoot -> root
    | CdUp -> currDir.parent.Value // Assuming "cd .." won't be called at root
    | CdDn d -> currDir.children[d]
    | Ls ls -> processLs ls currDir

root.Dump("root")

//let result = processCmd (cmds |> List.tail |> List.head) root
//
//result.Dump("result")