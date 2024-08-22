import { DependencyContainer } from "tsyringe";
import { IPostDBLoadMod } from "@spt/models/external/IPostDBLoadMod";
import { ILogger } from "@spt/models/spt/utils/ILogger";
import { DatabaseServer } from "@spt/servers/DatabaseServer";
import { LogTextColor } from "@spt/models/spt/logging/LogTextColor";
import { LogBackgroundColor } from "@spt/models/spt/logging/LogBackgroundColor";
import { readFileSync } from "fs";
import { join } from "path";

class Mod implements IPostDBLoadMod
{
    public postDBLoad(container: DependencyContainer): void 
    {
        const logger = container.resolve<ILogger>("WinstonLogger");
        const databaseServer = container.resolve<DatabaseServer>("DatabaseServer");

        const profileName = "Joey's Zero to Hero++ Unheard Edition";
        
        const tables = databaseServer.getTables();
        
        
        const UnheardProfile = tables.templates.profiles["Unheard"];
        const zthProfile = JSON.parse(JSON.stringify(UnheardProfile));
        const bearInventoryData = JSON.parse(readFileSync(join(__dirname, "./bear_inventory.json"), "utf-8"));
        const usecInventoryData = JSON.parse(readFileSync(join(__dirname, "./usec_inventory.json"), "utf-8"));
        const traderStanding = JSON.parse(readFileSync(join(__dirname, "./traders.json"), "utf-8"));
        const description = JSON.parse(readFileSync(join(__dirname, "./descLocale.json"), "utf-8"));
        const skill_issue = JSON.parse(readFileSync(join(__dirname, "./skill_issue.json"), "utf-8"));
        
        zthProfile.bear.character.Inventory = bearInventoryData;
        zthProfile.usec.character.Inventory = usecInventoryData;
        zthProfile.bear.trader = traderStanding;
        zthProfile.usec.trader = traderStanding;
        zthProfile.descriptionLocaleKey = description;
        zthProfile.bear.character.Skills = skill_issue;
        zthProfile.usec.character.Skills = skill_issue;




        tables.templates.profiles[profileName] = zthProfile;

        logger.logWithColor("[Joey's ZTH Mod] Joey's ZTH++ [UNHEARD EDITION] Loaded successfully", LogTextColor.GREEN);
    }
}

module.exports = { mod: new Mod() };