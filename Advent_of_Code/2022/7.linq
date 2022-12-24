<Query Kind="FSharpProgram" />

let inputFilePath = Path.Combine([| Path.GetDirectoryName(Util.CurrentQueryPath); "inputs"; "example_7.txt" |])
let inputLines = File.ReadAllLines(inputFilePath)

type File = { name: string; size: int }
type Directory = { name: string; mutable files: File list; parent: Option<Directory>; mutable children: Map<string, Directory>; }

let rec computeDirectorySize dir =
    let subdirsSize = Seq.sumBy computeDirectorySize (Map.values dir.children)
    let filesSize = List.sumBy (fun f -> f.size) dir.files
    subdirsSize + filesSize
    
let root = { name = "root"; files = List.empty; parent = None; children = Map.empty }
let mutable currDir = root

let processLs (cmd : string) cmds =
    let listing = Array.takeWhile (fun (c : string) -> c.StartsWith("$")) cmds

    for entry in listing do
        if cmd.StartsWith("dir") then
            let newDirName = cmd.Split()[1]
            let newDir = { name = newDirName; files = List.empty; parent = Some(currDir); children = Map.empty }
            //currDir <- { currDir with children = currDir.children.Add(newDirName, newDir) }
            currDir.children <- currDir.children.Add(newDirName, newDir)
        else
            let fileInfo = cmd.Split()
            let fileSize = int fileInfo[0]
            let newFile = { name = fileInfo[1]; size = fileSize}
            currDir.files <- newFile :: currDir.files
            
    currDir, Array.skip (Array.length listing) cmds
    
let (|CdUp|_|) (str: string) = 
    if String.Equals(str, "cd ..") then Some(CdUp String.Empty)
    else None
    
let (|CdDn|_|) (str : string) = 
    let split = str.Split()
    if Array.length split = 2 && String.Equals(split[0], "cd") && (String.Equals(split[1], "..") |> not) then Some(split[1])
    else None
    
//let processCd (cmd : string) = 
//    match cmd with
//    | CdUp _ -> currDir <- currDir.parent.Value // I'm assuming this won't be called when at the root...
//    | CdDn d -> currDir <- currDir.children[d]
//    | _ -> failwith "Not a known cd command."

let processCd (currDir : Directory) (cmd : string) = 
    match cmd with
    | CdUp _ -> currDir.parent.Value // I'm assuming this won't be called when at the root...
    | CdDn d -> currDir.children[d]
    | _ -> failwith "Not a known cd command."
    
