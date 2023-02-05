open System
open Jean
open Kitchensink
open KitchensinkJson


[<EntryPoint>]
let main argv =

    let p1 = testPayload
    let jsonStr = Json.serialize Payload.make p1
    printfn "%A" jsonStr
    let p2 = Json.parse Decoders.AsPayload jsonStr
    printfn "%A" p2

    0
