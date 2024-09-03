import os
import re

def replace_hyphens_in_specific_tags(file_path):
    with open(file_path, 'r', encoding='utf-8') as file:
        content = file.read()


    content = content.replace('[FactionOwner:REM]', '[FactionOwner:REMNANTS]')
    content = content.replace('[FactionOwner:SYN]', '[FactionOwner:SHIVAN]')
    content = content.replace('[FactionOwner:TIF]', '[FactionOwner:IRONFIST]')
    content = content.replace('SDC', 'CRUSADERS')
	
    # Replace hyphens with underscores within <SubtypeId> and <Behaviour> tags
    content = re.sub(r'(<SubtypeId>[^<]*)-([^<]*</SubtypeId>)', lambda m: m.group(0).replace('-', '_'), content)
    content = re.sub(r'(<Behaviour>[^<]*)-([^<]*</Behaviour>)', lambda m: m.group(0).replace('-', '_'), content)

    # Replace hyphens with underscores within <Description> tags but only inside ZoneConditions, TargetData, TriggerGroups
    def replace_within_description(match):
        description_content = match.group(1)
        description_content = re.sub(r'(\[ZoneConditions:[^\]]*?)-([^\]]*\])', lambda m: m.group(0).replace('-', '_'), description_content)
        description_content = re.sub(r'(\[TargetData:[^\]]*?)-([^\]]*\])', lambda m: m.group(0).replace('-', '_'), description_content)
        description_content = re.sub(r'(\[TriggerGroups:[^\]]*?)-([^\]]*\])', lambda m: m.group(0).replace('-', '_'), description_content)
        description_content = re.sub(r'(\[Triggers:[^\]]*?)-([^\]]*\])', lambda m: m.group(0).replace('-', '_'), description_content)
        description_content = re.sub(r'(\[FalseSandboxVariables:[^\]]*?)-([^\]]*\])', lambda m: m.group(0).replace('-', '_'), description_content)
        description_content = re.sub(r'(\[SecondaryAutopilotData:[^\]]*?)-([^\]]*\])', lambda m: m.group(0).replace('-', '_'), description_content)
        description_content = re.sub(r'(\[TertiaryAutopilotData:[^\]]*?)-([^\]]*\])', lambda m: m.group(0).replace('-', '_'), description_content)

        return f"<Description>{description_content}</Description>"

    content = re.sub(r'<Description>(.*?)</Description>', replace_within_description, content, flags=re.DOTALL)

    # Write the modified content back to the file
    with open(file_path, 'w', encoding='utf-8') as file:
        file.write(content)

def process_folder(folder_path):
    for root, dirs, files in os.walk(folder_path):
        for file in files:
            if file.endswith('.sbc'):
                file_path = os.path.join(root, file)
                replace_hyphens_in_specific_tags(file_path)

# Set the path to the folder you want to process
folder_path = 'C:/Users/Gebruiker/AppData/Roaming/SpaceEngineers/Mods/Ares at War part 1/Data/Encounters2'

# Start processing the folder and its subfolders
process_folder(folder_path)

# Wait for the user to press any key before closing
input("Druk op een toets om te sluiten...")







