using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public class PlayerOptionConverter : JsonConverter<PlayerOption>
{
    public override PlayerOption ReadJson(JsonReader reader, Type objectType, PlayerOption existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        PlayerOption playerOption = new PlayerOption();
        JObject jObj = JObject.Load(reader);
        playerOption.bgmValue = (float)jObj["bgmValue"];
        playerOption.sfxValue = (float)jObj["sfxValue"];
        playerOption.isCameraShake = (bool)jObj["isCameraShake"];

        return playerOption;
    }

    public override void WriteJson(JsonWriter writer, PlayerOption value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("bgmValue");
        writer.WriteValue(value.bgmValue);
        writer.WritePropertyName("sfxValue");
        writer.WriteValue(value.sfxValue);
        writer.WritePropertyName("isCameraShake");
        writer.WriteValue(value.isCameraShake);
        writer.WriteEndObject();
    }
}

public class RecordConverter : JsonConverter<List<RecordData>>
{
    public override List<RecordData> ReadJson(JsonReader reader, Type objectType, List<RecordData> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        List<RecordData> records = new List<RecordData>();
        JArray jArr = JArray.Load(reader);

        foreach(var jObj in jArr)
        {
            RecordData record = new RecordData();

            record.characterDataId = (int)jObj["CharacterDataId"];
            record.weaponDataId = (int)jObj["WeaponDataId"];
            record.score = (int)jObj["Score"];

            records.Add(record);
        }

        return records;
    }

    public override void WriteJson(JsonWriter writer, List<RecordData> value, JsonSerializer serializer)
    {
        writer.WriteStartArray();

        for(int i = 0; i < value.Count; ++i)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("CharacterDataId");
            writer.WriteValue(value[i].characterDataId);
            writer.WritePropertyName("WeaponDataId");
            writer.WriteValue(value[i].weaponDataId);
            writer.WritePropertyName("Score");
            writer.WriteValue(value[i].score);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }
}