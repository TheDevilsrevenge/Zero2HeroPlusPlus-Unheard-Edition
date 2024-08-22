"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const LogTextColor_1 = require("C:/snapshot/project/obj/models/spt/logging/LogTextColor");
const fs_1 = require("fs");
const path_1 = require("path");
class Mod {
    postDBLoad(container) {
        const logger = container.resolve("WinstonLogger");
        const databaseServer = container.resolve("DatabaseServer");
        const profileName = "Joey's Zero to Hero++";
        const tables = databaseServer.getTables();
        const EoDProfile = tables.templates.profiles["Edge Of Darkness"];
        const zthProfile = JSON.parse(JSON.stringify(EoDProfile));
        const bearInventoryData = JSON.parse((0, fs_1.readFileSync)((0, path_1.join)(__dirname, "./bear_inventory.json"), "utf-8"));
        const usecInventoryData = JSON.parse((0, fs_1.readFileSync)((0, path_1.join)(__dirname, "./usec_inventory.json"), "utf-8"));
        const traderStanding = JSON.parse((0, fs_1.readFileSync)((0, path_1.join)(__dirname, "./traders.json"), "utf-8"));
        const description = JSON.parse((0, fs_1.readFileSync)((0, path_1.join)(__dirname, "./descLocale.json"), "utf-8"));
        zthProfile.bear.character.Inventory = bearInventoryData;
        zthProfile.usec.character.Inventory = usecInventoryData;
        zthProfile.bear.trader = traderStanding;
        zthProfile.usec.trader = traderStanding;
        zthProfile.descriptionLocaleKey = description;
        tables.templates.profiles[profileName] = zthProfile;
        logger.logWithColor("[Joey's ZTH Mod] Joey's ZTH++ Loaded successfully", LogTextColor_1.LogTextColor.GREEN);
    }
}
module.exports = { mod: new Mod() };
//# sourceMappingURL=mod.js.map