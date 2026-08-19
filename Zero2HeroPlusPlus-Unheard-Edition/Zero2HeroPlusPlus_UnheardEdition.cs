using System.Reflection;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Server.Core.Services.Locales;
using SPTarkov.Server.Core.Utils.Cloners;
using Path = System.IO.Path;

namespace Zero2HeroPlusPlus_Unheard_Edition;

// Data for the mod
public record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "com.thedevilsrevenge.zerotoherounheard";
    public string Name { get; init; } = "Zero2HeroUnheardEdition";
    public string Author { get; init; } = "TheDevilsrevenge";
    public List<string>? Contributors { get; init; }
    public SemanticVersioning.Version Version { get; init; } = new("2.1.0");
    public SemanticVersioning.Range SptVersion { get; init; } = new("~4.1.0");
    public List<string>? Incompatibilities { get; init; }
    public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public string? Url { get; init; }
    public string License { get; init; } = "MIT";
    public bool? IsBundleMod { get; init; }
    public bool HasPrepatcher { get; init; }
}

[Injectable(TypePriority = OnLoadOrder.PostLoad + 1)]
public class Zero2HeroPlusPlusUnheardEdition(
    TemplateTable templateTable,
    ICloner cloner,
    ModHelper modHelper,
    LocaleService profileDesc,
    ISptLogger<Zero2HeroPlusPlusUnheardEdition> logger) : IOnLoad
{
    // Copying Unheard profile
    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var profileDescription = profileDesc.GetDesiredServerLocale();
        var unheardCopy = cloner.Clone(templateTable.Profiles["Unheard"])!;
        if (unheardCopy.Bear?.Character is null
            || unheardCopy.Usec?.Character is null)
        {
            logger.Error("Targeted profile could not be copied");
            return Task.CompletedTask;
        }
        // Removing trader bonus
        unheardCopy.Bear.Trader = modHelper.GetJsonDataFromFile<ProfileTraderTemplate>
            (pathToMod, Path.Combine("jsonData", "traders.json"));
        unheardCopy.Usec.Trader = modHelper.GetJsonDataFromFile<ProfileTraderTemplate>
            (pathToMod, Path.Combine("jsonData", "traders.json"));
        // Removing skill bonus
        unheardCopy.Bear.Character.Skills = modHelper.GetJsonDataFromFile<Skills>
            (pathToMod, Path.Combine("jsonData", "skills.json"));
        unheardCopy.Usec.Character.Skills = modHelper.GetJsonDataFromFile<Skills>
            (pathToMod, Path.Combine("jsonData", "skills.json"));
        // Clearing inventory
        unheardCopy.Bear.Character.Inventory = modHelper.GetJsonDataFromFile<BotBaseInventory>
            (pathToMod, Path.Combine("jsonData", "bear_inventory.json"));
        unheardCopy.Usec.Character.Inventory = modHelper.GetJsonDataFromFile<BotBaseInventory>
            (pathToMod, Path.Combine("jsonData", "usec_inventory.json"));

        // Custom description (need to add more languages later down the line)
        unheardCopy.DescriptionLocaleKey = profileDescription switch
        {
            "en" => "Zero2Hero with Unheard stash, pockets and of course the Gamma container, no trader rep boost and no skill boost",
            _ => ""
        };
        templateTable.Profiles["Joey's Zero2Hero++ Unheard Edition"] = unheardCopy;
        logger.Success("Joey's Zero2Hero++ loaded successfully!");
       
        return Task.CompletedTask;
    }
}
