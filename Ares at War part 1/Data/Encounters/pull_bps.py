import os
import re

BP_DIR = r"C:\Users\maart\AppData\Roaming\SpaceEngineers\Blueprints\local\#AaWPrefab"

factions_list = ["AnF","CIVILIAN", "CRUSADERS", "DRA",
                 "GC","IRONFIST", "ITC", "MILITIA",
                 "REMNANTS", "SCRAP", "SHIVAN",
                 "SPRT", "UNION", "UNKN"]


def find_faction_in_string(s):
    s = s.upper()
    for faction in factions_list:
        if faction.upper() in s:
            return faction
    return "Prefabs"


def main():

    files = []
    src = BP_DIR
    target = os.path.join(os.path.dirname(__file__), "pulled prefabs")

    for r, d, f in os.walk(src):
        for file in f:
            if file == 'bp.sbc':
                files.append(os.path.join(r, file))

    for filename in files:
        with open(os.path.join(src, filename), 'r', encoding="utf-8") as f:
            lines = f.read()
        f.close()

        if lines.find("<ShipBlueprints>") == -1:
            continue

        if '<Id Type="MyObjectBuilder_ShipBlueprintDefinition" Subtype="' in lines:
            bp_name = lines.split('<Id Type="MyObjectBuilder_ShipBlueprintDefinition" Subtype="')[1].split('"')[0]
        elif '<TypeId>MyObjectBuilder_ShipBlueprintDefinition</TypeId>' in lines:
            bp_name = lines.split('<SubtypeId>')[1].split('</SubtypeId>')[0]
        elif '<Id Type="MyObjectBuilder_ShipBlueprintDefinition" Subtype="' in lines:
            bp_name = lines.split('<Id Type="MyObjectBuilder_ShipBlueprintDefinition" Subtype="')[1].split('"')[0]

        prefab = lines.replace("<ShipBlueprints>", "<Prefabs>")
        prefab = prefab.replace("</ShipBlueprints>", "</Prefabs>")

        prefab = prefab.replace('<ShipBlueprint>', '<Prefab xsi:type="MyObjectBuilder_PrefabDefinition">')
        prefab = prefab.replace('<ShipBlueprint xsi:type="MyObjectBuilder_ShipBlueprintDefinition">', '<Prefab xsi:type="MyObjectBuilder_PrefabDefinition">')
        prefab = prefab.replace("</ShipBlueprint>", "</Prefab>")

        prefab = prefab.replace("MyObjectBuilder_ShipBlueprintDefinition", "MyObjectBuilder_PrefabDefinition")

        if "<DLC>" in lines:
            dlc_start = prefab.find("<DisplayName>")
            if dlc_start > 500:
                dlc_start = prefab.find("<DLC>")
            dlc_end = prefab.rfind("</DLC>") + len("</DLC>")
            prefab = prefab[:dlc_start:] + prefab[dlc_end:]

        faction = find_faction_in_string(bp_name)

        # Default target
        if faction:
            target = os.path.join(faction, f"[{faction}]-Prefabs")
        else:
            target = "Prefabs"  # Fallback if no faction found

        full_path = os.path.join(src, filename)

        target_file = os.path.join(target, bp_name + ".sbc")
        output = open(target_file, "w", encoding="utf-8")
        output.write(prefab)

        # Check and remove .sbcB5 if older than newly written .sbc
        sbcB5_file = os.path.join(target, bp_name + ".sbcB5")

        # Check for a matching .sbcB5 file in the same folder
        sbcB5_file = target_file + "B5"
        if os.path.exists(sbcB5_file):
            sbc_time = os.path.getmtime(full_path)
            sbcB5_time = os.path.getmtime(sbcB5_file)

            if sbc_time > sbcB5_time:
                print(f"Updates detected for {bp_name}")
                os.remove(sbcB5_file)
                continue
            else:
                #print(f"no updates detected for {bp_name}")
                continue

                
        print(f"new file blueprint found {bp_name}")


    input("klaar")
    return

if __name__ == '__main__':
    main()