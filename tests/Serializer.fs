module JeanTests.Serializer

open System
open Jean
open Expecto

// Serialization
let tests =
    [ test "Can serialize document with nothing" {
          let txt = "{}"
          let decoder (j: JsonNode) = j.AsPropertyArrayOrNone()
          let struc = Json.parse decoder txt
          let encoder j = Map.empty |> JsonNode.make
          let actual = Json.serialize encoder struc
          Expect.equal actual txt ""
      }

      test "Can serialize document with int" {
          let txt = "{\"firstName\":\"John\",\"age\":25}"

          let decoder j =
              {| firstName = j?firstName.AsString()
                 age = j?age.AsInt() |}

          let struc = Json.parse decoder txt

          let encoder (st: {| firstName: string; age: int |}) =
              [| ("firstName", JsonNode.make st.firstName); ("age", JsonNode.make st.age) |]
              |> JsonNode.make

          let actual = Json.serialize encoder struc
          Expect.equal actual txt ""
      }
      test "Can serialize document with bool" {
          let txt = "{\"firstName\":\"John\",\"employed\":false}"

          let decoder j =
              {| firstName = j?firstName.AsString()
                 employed = j?employed.AsBool() |}

          let struc = Json.parse decoder txt

          let encoder (st: {| firstName: string; employed: bool |}) =
              [| ("firstName", JsonNode.make st.firstName)
                 ("employed", JsonNode.make st.employed) |]
              |> JsonNode.make

          let actual = Json.serialize encoder struc
          Expect.equal actual txt ""
      }
      test "Can serialize document with booleans" {
          let obj =
              [| "aa", JsonNode.make true; "bb", JsonNode.make false |]
              |> JsonNode.make
              |> Json.serializeObj

          Expect.equal obj "{\"aa\":true,\"bb\":false}" ""
      }
      test "Can serialize document with float" {
          let txt = "{\"firstName\":\"John\",\"weight\":25.25}"

          let decoder j =
              {| firstName = j?firstName.AsString()
                 weight = j?weight.AsFloat() |}

          let struc = Json.parse decoder txt

          let encoder (st: {| firstName: string; weight: float |}) =
              [| ("firstName", JsonNode.make st.firstName)
                 ("weight", JsonNode.make st.weight) |]
              |> JsonNode.make

          let actual = Json.serialize encoder struc
          Expect.equal actual txt ""

      }
      test "Can serialize document with iso 8601 date" {
          let txt = "{\"birthDate\":\"2020-05-19T14:39:22.500Z\"}"

          let decoder j =
              {| birthDate = (j?birthDate).AsDateTimeOffset() |}

          let struc = Json.parse decoder txt

          let encoder (st: {| birthDate: DateTimeOffset |}) =
              [| ("birthDate", JsonNode.make st.birthDate) |] |> JsonNode.make

          let actual = Json.serialize encoder struc
          Expect.equal (actual.Substring(0, 37)) (txt.Substring(0, 37)) ""
      }
      test "Can serialize document with timespan" {
          let txt = "{\"lapTime\":\"00:30:00\"}"

          let decoder j =
              {| lapTime = (j?lapTime).AsTimeSpan() |}

          let struc = Json.parse decoder txt

          let encoder (st: {| lapTime: TimeSpan |}) =
              [| ("lapTime", JsonNode.make st.lapTime) |] |> JsonNode.make

          let actual = Json.serialize encoder struc
          Expect.equal actual txt ""
      }
      test "Can serialize document with guid" {

          let txt = "{\"id\":\"f842213a-82fb-4eeb-ab75-7ccd18676fd5\"}"
          let decoder j = {| id = j?id.AsGuid() |}
          let struc = Json.parse decoder txt

          let encoder (st: {| id: Guid |}) =
              [| ("id", JsonNode.make st.id) |] |> JsonNode.make

          let actual = Json.serialize encoder struc
          Expect.equal actual txt ""
      }
      test "Can serialize document with null roundtrip" {
          let txt = "{\"firstName\":\"John\",\"age\":null}"

          let decoder j =
              {| firstName = j?firstName.AsString()
                 age = j?age.AsInt32OrNone() |}

          let struc = Json.parse decoder txt

          let encoder
              (st:
                  {| firstName: string
                     age: int32 option |})
              =
              [| ("firstName", JsonNode.make st.firstName); ("age", JsonNode.make st.age) |]
              |> JsonNode.make

          let actual = Json.serialize encoder struc
          Expect.equal actual txt ""
      }]
