using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils.Cloners;
using Path = System.IO.Path;

namespace Zero2HeroPlusPlus_Unheard_Edition;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.thedevilsrevenge.zerotoherounheard";
    public override string Name { get; init; } = "Zero2HeroUnheardEdition";
    public override string Author { get; init; } = "TheDevilsrevenge";
    public override List<string>? Contributors { get; init; }
    public override SemanticVersioning.Version Version { get; init; } = new("2.0.0");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; }
    public override bool? IsBundleMod { get; init; }
    public override string License { get; init; } = "MIT";

}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class Zero2HeroPlusPlus_Unheard_Edition(
    DatabaseServer databaseServer,
    ICloner cloner,
    ModHelper modHelper,
    LocaleService profileDesc,
    ISptLogger<Zero2HeroPlusPlus_Unheard_Edition> logger) : IOnLoad
{
 public Task OnLoad()
    {
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var profileDescription = profileDesc.GetDesiredServerLocale();
        var unheardCopy = cloner.Clone(databaseServer.GetTables().Templates.Profiles["Unheard"])!;
        if (unheardCopy.Bear is null)
        {
            logger.Error("Targeted profile could not be copied");
            return Task.CompletedTask;
        }

        unheardCopy.DescriptionLocaleKey = profileDescription switch
        {
            "en" => "Zero2Hero with Unheard stash, pockets and of course the Gamma container, no trader rep boost"
        };
        databaseServer.GetTables().Templates.Profiles["Joey's Zero2Hero++ Unheard Edition"] = unheardCopy;
        logger.Success("Joey's Zero2Hero++ loaded succesfully!");
        return Task.CompletedTask;
    }

}
