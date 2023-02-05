namespace Jean

#nowarn "42"

open System
open System.Runtime.InteropServices

type JsonNamingPolicy =
    | Camel
    | Snake
    | AsIs

type Opts =
    { jsonNamingPolicy: JsonNamingPolicy
      allowNullFields: bool }

    static member defaults() =
        { jsonNamingPolicy = AsIs
          allowNullFields = false }

[<AttributeUsage(AttributeTargets.Class ||| AttributeTargets.Struct)>]
type JsonConverterAttribute([<Optional>] NamingPolicy: string, [<Optional>] AllowNullFields: string) =
    inherit Attribute()

    member _.NamingPolicy =
        match NamingPolicy with
        | "Camel" -> Camel
        | "Snake" -> Snake
        | _ -> AsIs

    member _.AllowNullFields =
        match AllowNullFields with
        | "true" -> true
        | "True" -> true
        | _ -> false

    new(jsonNamingPolicy: string) = JsonConverterAttribute()

    new(allowNullFields: bool) = JsonConverterAttribute()

[<RequireQualifiedAccess>]
type JsonName =
    | String of string

    member this.AsString() =
        match this with
        | String name -> name

[<AttributeUsage(AttributeTargets.Property, AllowMultiple = false)>]
type JsonNameAttribute(name: JsonName) =
    inherit Attribute()

    member _.Name = name

    new(name: string) = JsonNameAttribute(JsonName.String name)
