#r "nuget: YamlDotNet, 18.1.0"

open System
open System.IO
open YamlDotNet.Serialization
open YamlDotNet.Serialization.NamingConventions

let args = fsi.CommandLineArgs |> Array.skip 1

if args.Length < 2 then
    eprintfn "Usage: dotnet fsi LocaleLookup.fsx <yaml-file> <key-path>"
    exit 1

let yamlFile = args.[0]
let keyPath = args.[1]

if not (File.Exists yamlFile) then
    eprintfn "File not found: %s" yamlFile
    exit 1

let deserializer =
    DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build()

let yamlText = File.ReadAllText yamlFile

let root = deserializer.Deserialize<obj>(yamlText)

let rec lookup (node: obj) (keys: string list) =
    match keys, node with
    | [], value -> Some value

    | key :: rest, (:? System.Collections.Generic.IDictionary<obj, obj> as dict) ->
        match dict.TryGetValue(key) with
        | true, value -> lookup value rest
        | false, _ -> None

    | _ -> None

match lookup root (keyPath.Split('.') |> Array.toList) with
| Some value -> printfn "%O" value
| None ->
    eprintfn "Key not found: %s" keyPath
    exit 2
