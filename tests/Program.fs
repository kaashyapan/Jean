module Program

open Expecto

[<EntryPoint>]
let main _ =
    let tests1 = testList "Serialization tests" JeanTests.Serializer.tests
    (runTestsWithCLIArgs [] [||] tests1) |> ignore

    let tests2 = testList "Deserialization tests" JeanTests.Deserializer.tests
    (runTestsWithCLIArgs [] [||] tests2) |> ignore

    0
