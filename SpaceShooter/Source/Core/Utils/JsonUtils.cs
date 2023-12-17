#nullable enable
using Newtonsoft.Json;
using System.IO;

namespace SpaceShooter.Source.Core.Utils;
public static class JsonUtils
{
    /// <summary>
    /// formats the file by serializing and then deserializing the object as <typeparamref name="T"/>
    /// </summary>
    public static T? InitFile<T>(string path, bool indented = false)
    {
        using FileStream fileStream = FileUtils.OpenOrCreate(path);

        //preform init
        T? obj = InitFile<T>(fileStream, indented);

        //finalize
        return obj;
    }

    /// <summary>
    /// serializes <paramref name="obj"/> to the a file
    /// </summary>
    public static void SerializeToFile(string path, object? obj, bool indented)
    {
        //use the write-only fileStream from the opened file to serialize the file
        using FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Write);
        SerializeToFile(fileStream, obj, indented);
    }

    /// <summary>
    /// deserializes the file to an instance of '<typeparamref name="T"/>'
    /// </summary>
    public static T? DeserializeFromFile<T>(string path)
    {
        //use the read-only fileStream from the opened file to deserialize the file
        using FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);
        return DeserializeFromFile<T>(fileStream);
    }

    /// <summary>
    /// uses a FileStream instead of a file path<br/>
    /// <inheritdoc cref="InitFile{T}(string, bool)"/>
    /// </summary>
    public static T? InitFile<T>(FileStream fileStream, bool indented = false)
    {
        //deserialize into the object and serialize it back to the file
        T? obj = DeserializeFromFile<T>(fileStream);
        SerializeToFile(fileStream, obj, indented);
        return obj; //return results
    }

    /// <summary>
    /// uses a FileStream instead of a file path<br/>
    /// <inheritdoc cref="SerializeToFile(string, object?, bool)"/>
    /// </summary>
    public static void SerializeToFile(FileStream fileStream, object? obj, bool indented)
    {
        using StreamWriter streamWriter = new(fileStream, leaveOpen: true); //use the file stream for a stream writer

        //serialize the object to json
        Formatting formatting = indented ? Formatting.Indented : Formatting.None;
        string jsonText = JsonConvert.SerializeObject(obj, formatting);

        //write the results
        streamWriter.BaseStream.SetLength(0); //clear the current contents of the file
        streamWriter.Write(jsonText); //write the json text to the file
        streamWriter.Flush(); //flush the results
    }

    /// <summary>
    /// uses a FileStream instead of a file path<br/>
    /// <inheritdoc cref="DeserializeFromFile{T}(string)"/>
    /// </summary>
    public static T? DeserializeFromFile<T>(FileStream fileStream)
    {
        using StreamReader streamReader = new(fileStream, leaveOpen: true); //use the file stream for a stream reader

        //serialize the object from json
        string jsonText = streamReader.ReadToEnd(); //read all the text at the path
        T? value = JsonConvert.DeserializeObject<T>(jsonText); //convert the text to an instance of T

        //return the result
        return value;
    }
}
