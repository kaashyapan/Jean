module KitchensinkExt

open System
open System.Runtime.CompilerServices
open Jean
open Kitchensink

type Encoders() =
    static member make(p: Address) : JsonNode =
        seq {
            ("Street", JsonNode.make p.Street)
            ("State", JsonNode.make p.State')
        }
        |> JsonNode.make

    static member make(p: Address option) : JsonNode =
        match p with
        | Some address -> Encoders.make address
        | None -> JsonNode.make ()

    static member make(value: Pet) : JsonNode =
        match value with
        | Cat -> "Cat"
        | Dog -> "Dog"
        |> JsonNode.make

    static member make(value: Pet option) : JsonNode =
        match value with
        | Some pet -> Encoders.make pet
        | None -> JsonNode.make ()

    static member make(value: Sex) : JsonNode =
        match value with
        | Male -> "Male"
        | Female -> "Female"
        |> JsonNode.make

    static member make(value: Sex option) : JsonNode =
        match value with
        | Some sex -> Encoders.make sex
        | None -> JsonNode.make ()

    static member make(value: Choice<bool, int>) : JsonNode =
        match value with
        | Choice1Of2 v -> JsonNode.make v
        | Choice2Of2 v -> JsonNode.make v

    static member make(p: Payload) : JsonNode =
        seq {
            ("Name", JsonNode.make p.Name)
            ("Age", JsonNode.make p.Age)
            ("Address", Encoders.make p.Address)
            ("Sex", Encoders.make p.Sex)
            ("Pet", Encoders.make p.Pet)
            ("Cht", Encoders.make p.Cht)
        }
        |> JsonNode.make

type Payload with

    static member make(p: Payload) : JsonNode =
        seq {
            ("Name", JsonNode.make p.Name)
            ("Age", JsonNode.make p.Age)
            ("Address", Encoders.make p.Address)
            ("Sex", Encoders.make p.Sex)
            ("Pet", Encoders.make p.Pet)
            ("Cht", Encoders.make p.Cht)
        }
        |> JsonNode.make


[<Extension>]
type Decoders() =
    [<Extension>]
    static member AsAddress(j: JsonNode) =
        { Street = (j </> "Street").AsString()
          State' = (j </> "State").AsString() }

    [<Extension>]
    static member AsAddressOrNone(jnode: JsonNode) = jnode.AsAddress() |> Some

    [<Extension>]
    static member AsAddressOrNone(jnode: JsonNode option) =
        match jnode with
        | Some prop -> prop.AsAddressOrNone()
        | None -> None

    [<Extension>]
    static member AsSex(j: JsonNode) =
        match (j.AsString()) with
        | "Male" -> Male
        | "Female" -> Female
        | err -> failwith "Invalid value as Sex. Received {err}"

    [<Extension>]
    static member AsSexOrNone(j: JsonNode) = j.AsSex() |> Some

    [<Extension>]
    static member AsSexOrNone(j: JsonNode option) =
        match j with
        | Some node -> node.AsSexOrNone()
        | None -> None

    [<Extension>]
    static member AsPet(j: JsonNode) =
        match (j.AsString()) with
        | "Cat" -> Cat
        | "Dog" -> Dog
        | err -> failwith "Invalid value as Pet. Received {err}"

    [<Extension>]
    static member AsPetOrNone(j: JsonNode) = j.AsPet() |> Some

    [<Extension>]
    static member AsPetOrNone(j: JsonNode option) =
        match j with
        | Some node -> node.AsPetOrNone()
        | None -> None

    [<Extension>]
    static member AsCht(j: JsonNode) =
        match j.typ with
        | JBool v -> Choice1Of2 v
        | JNumber v -> Choice2Of2(int32 v)
        | _ -> failwith "Unexpected value"


    [<Extension>]
    static member AsPayload(j: JsonNode) : Payload =
        { Name = (j </> "Name").AsString()
          Age = (j </> "Age").AsInt()
          Address = (j?Address).AsAddressOrNone()
          Sex = (j </> "Sex").AsSex()
          Pet = (j </> "Pet").AsPetOrNone()
          Cht = (j </> "Cht").AsCht() }

    [<Extension>]
    static member AsPayloadOrNone(j: JsonNode) : Payload option = j.AsPayload() |> Some

    [<Extension>]
    static member AsPayloadOrNone(j: JsonNode option) : Payload option =
        match j with
        | Some node -> node.AsPayloadOrNone()
        | None -> None
