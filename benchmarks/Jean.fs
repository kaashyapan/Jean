namespace JsonBenchmarks

open System
open System.Runtime.CompilerServices
open Jean

[<Extension>]
type Decode() =
    [<Extension>]
    static member AsThumbnail(j: JsonNode) : Thumbnail =
        { Thumbnail.height = (j </> "height").AsInt()
          width = (j </> "width").AsInt() }

    [<Extension>]
    static member AsThumbnailOrNone(j: JsonNode) = j.AsThumbnail() |> Some

    [<Extension>]
    static member AsThumbnailOrNone(j: JsonNode option) : Thumbnail option =
        match j with
        | Some prop -> prop.AsThumbnailOrNone()
        | None -> None

    [<Extension>]
    static member AsImage(j: JsonNode) : Image =
        { Image.thumbnail = (j </?> "thumbnail").AsThumbnailOrNone() }

    [<Extension>]
    static member AsImageOrNone(json: JsonNode option) : Image option = Option.map Decode.AsImage json

    [<Extension>]
    static member AsVideo(j: JsonNode) : Video =
        { Video.thumbnail = (j </?> "thumbnail").AsThumbnailOrNone() }

    [<Extension>]
    static member AsVideoOrNone(json: JsonNode option) : Video option = Option.map Decode.AsVideo json

    [<Extension>]
    static member AsProvider(json: JsonNode) : Provider = { name = json?name.AsString() }

    [<Extension>]
    static member AsProviders(json: JsonNode) : Provider list =
        json.AsArray() |> Array.toList |> List.map Decode.AsProvider

    [<Extension>]
    static member AsSearchResult(json: JsonNode) : SearchResult =
        { name = (json </> "name").AsString()
          url = (json </> "url").AsString()
          description = (json </> "description").AsString()
          datePublished = (json </> "datePublished").AsString()
          image = (json </?> "image").AsImageOrNone()
          video = (json </?> "video").AsVideoOrNone()
          provider = json?provider.AsProviders() }


    [<Extension>]
    static member AsSearchResults(json: JsonNode) : SearchResult list =
        json.AsArray() |> Array.toList |> List.map Decode.AsSearchResult

    [<Extension>]
    static member AsPayload(json: JsonNode) : Payload =
        { readLink = json?readLink.AsString()
          totalEstimatedMatches = json?totalEstimatedMatches.AsInt()
          value = json?value.AsSearchResults() }

[<AutoOpen>]
module Encoders =
    type Thumbnail with

        static member toJsonObj(j: Thumbnail) =
            [| ("height", JsonNode.make j.height); ("width", JsonNode.make j.width) |]
            |> JsonNode.make

        static member toJsonObj(oj: Thumbnail option) =
            match oj with
            | Some j -> Thumbnail.toJsonObj j
            | None -> JsonNode.make ()

    type Image with

        static member toJsonObj(j: Image) =
            [| ("thumbnail", Thumbnail.toJsonObj j.thumbnail) |] |> JsonNode.make

        static member toJsonObj(oj: Image option) =
            match oj with
            | Some j -> Image.toJsonObj j
            | None -> JsonNode.make ()

    type Video with

        static member toJsonObj(j: Video) =
            [| ("thumbnail", Thumbnail.toJsonObj j.thumbnail) |] |> JsonNode.make

        static member toJsonObj(oj: Video option) =
            match oj with
            | Some j -> Video.toJsonObj j
            | None -> JsonNode.make ()

    type Provider with

        static member toJsonObj(j: Provider) =
            [| "name", JsonNode.make j.name |] |> JsonNode.make

        static member toJsonObj(oj: Provider seq) =
            oj |> Seq.map Provider.toJsonObj |> JsonNode.make

    type SearchResult with

        static member toJsonObj(j: SearchResult) =
            [| ("name", JsonNode.make j.name)
               ("image", Image.toJsonObj j.image)
               ("url", JsonNode.make j.url)
               ("description", JsonNode.make j.description)
               ("datePublished", JsonNode.make j.datePublished)
               ("video", Video.toJsonObj j.video)
               ("provider", Provider.toJsonObj j.provider) |]
            |> JsonNode.make

        static member toJsonObj(oj: SearchResult seq) =
            oj |> Seq.map SearchResult.toJsonObj |> JsonNode.make

    type Payload with

        static member toJsonObj(j: Payload) =
            [| ("readLink", JsonNode.make j.readLink)
               ("totalEstimatedMatches", JsonNode.make j.totalEstimatedMatches)
               ("value", SearchResult.toJsonObj j.value) |]
            |> JsonNode.make

        static member toJson(j: Payload) = Json.serialize Payload.toJsonObj j

        static member fromJson(str: String) = Json.parse Decode.AsPayload str
