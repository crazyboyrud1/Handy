using SPTarkov.Server.Core.Models.Spt.Mod;
using Range = SemanticVersioning.Range;

namespace lilFilter;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.Benjamin.LittleFilter";
    public override string Name { get; init; } = "Little filter";
    public override string Author { get; init; } = "crazyboyrud";
    public override SemanticVersioning.Version Version { get; init; } = new("1.0.1");
    public override Range SptVersion { get; init; } = new("4.0.13");
    public override string License { get; init; } = "MIT";
    public override bool? IsBundleMod { get; init; } = true;
    public override Dictionary<string, Range>? ModDependencies { get; init; } =
        new() { { "com.wtt.commonlib", new Range("~2.0.15") } };
    public override string? Url { get; init; }
    public override List<string>? Contributors { get; init; }
    public override List<string>? Incompatibilities { get; init; }
}
