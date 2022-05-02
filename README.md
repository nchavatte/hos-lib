# hos-lib

![CI](https://github.com/nchavatte/hos-lib/actions/workflows/main.yml/badge.svg?branch=develop)

This .NET library provides conversion of bytes into/from human-friendly
[serial form](https://github.com/nchavatte/hos-lib/wiki/Serial-form).

## Example of use

```csharp
using NChavatte.HumanOrientedSerialization.Common;
using System.IO;
using System.Text;

class ExampleClass
{
    string ExampleSerializeFile(string sourceFilePath)
    {
        byte[] source = File.ReadAllBytes(sourceFilePath);
        return HOS.Serialize(source);
    }

    byte[] ExampleDeserializeFile(string serialFormFilePath)
    {
        string serialForm = File.ReadAllText(serialFormFilePath, Encoding.ASCII);
        DeserializationResult result = HOS.Deserialize(serialForm);
        return result.Content;
    }
}
```
