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
let mutable currDir = root

let parseCd (cmd : string) =
    if cmd.EndsWith("..") then CdUp
    else if cmd.EndsWith("/") then CdRoot
    else
        let split = cmd.Split()
        CdDn split[1]
        
let parseLs arr =
    let listings = Array.takeWhile (fun (l : string) -> l.StartsWith("$") |> not) arr
    
    //listings.Dump("listings")
    
    let listingsLength = Array.length listings
    
    //listingsLength.Dump("listingsLength")
    
    (Ls listings, Array.skip listingsLength arr)
    
let parseCommand (cmd : string) arr =
    //cmd.Dump("parseCommand cmd")
    //cmd[..3].Dump("chuckah")
    match cmd[..3] with
    | "$ cd" -> (parseCd cmd, arr)
    | "$ ls" -> parseLs arr
    | _ -> failwith "Unrecognised command"

let parseCommands arr =
    let mutable cmds = List.empty
    let mutable remainingCmds = arr
    while Array.isEmpty remainingCmds |> not do
    
        //remainingCmds.Dump("remainingCmds")
    
        let nextCmd = Array.head remainingCmds
        
        //nextCmd.Dump("nextCmd")
        
        remainingCmds <- Array.tail remainingCmds
        
        //remainingCmds.Dump("remainingCmds2")
        
        let cmd, rest = parseCommand nextCmd remainingCmds
        cmds <- cmd :: cmds
        remainingCmds <- rest
        //remainingCmds.Dump("remainingCmds3")
        
    List.rev cmds
    
let cmds = parseCommands inputLines
cmds.Dump("cmds")