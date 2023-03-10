module Kitchensink

open System
open Jean

type Address = { Street: string; State': string }

type Sex =
    | Male
    | Female

type Pet =
    | Cat
    | Dog

[<JsonConverter(NamingPolicy = "Camel", AllowNullFields = "true")>]
type Payload =
    { Name: string
      Age: int
      Address: Address option
      Sex: Sex
      Pet: Pet option
      [<JsonName("Chkt")>]
      Cht: Choice<bool, int> }

let testPayload =
    { Name = "Elon Musk"
      Age = 50
      Address =
        Some
            { Street = "San Fransisco"
              State' = "California" }
      Sex = Male
      Pet = Some Dog
      Cht = Choice2Of2 32 }
