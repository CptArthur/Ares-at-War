import os

def replace_text_in_files(directory):
    trigger_lines_to_remove = [
        "[TriggerGroups:MSB_BountyHunter_TriggerGroup]",
        "[TriggerGroups:MSB_Cargoship_TriggerGroup]",
        "[TriggerGroups:MSB_ConvoyLeader_TriggerGroup]",
        "[TriggerGroups:MSB_DefensiveEscort_TriggerGroup]",
        "[TriggerGroups:MSB_OffensiveEscort_TriggerGroup]",
        "[TriggerGroups:MSB_Guard_TriggerGroup]",
        "[TriggerGroups:MSB_Hunter_TriggerGroup]",
        "[TriggerGroups:MSB_Merchant_TriggerGroup]",
        "[TriggerGroups:MSB_Patrol_TriggerGroup]",
        "[TriggerGroups:MSB_Raider_TriggerGroup]",
        "[TriggerGroups:MSB_Salvager_TriggerGroup]",
        "[TriggerGroups:MSB_Scout_TriggerGroup]",
        "[TriggerGroups:MSB_StrikeForceLeader_TriggerGroup]",
        "[TriggerGroups:MSB_TaskForceLeader_TriggerGroup]",
        "[TriggerGroups:MSB_BountyHunter_TriggerGroup_Complete]",
        "[TriggerGroups:MSB_Cargoship_TriggerGroup_Complete]",
        "[TriggerGroups:MSB_ConvoyLeader_TriggerGroup_Complete]",
        "[TriggerGroups:MSB_DefensiveEscort_TriggerGroup_Complete]",
        "[TriggerGroups:MSB_OffensiveEscort_TriggerGroup_Complete]",
        "[TriggerGroups:MSB_Guard_TriggerGroup_Complete]",
        "[TriggerGroups:MSB_Hunter_TriggerGroup_Complete]",
        "[TriggerGroups:MSB_Merchant_TriggerGroup_Complete]",
        "[TriggerGroups:MSB_Patrol_TriggerGroup_Complete]",
        "[TriggerGroups:MSB_Raider_TriggerGroup_Complete]",
        "[TriggerGroups:MSB_Salvager_TriggerGroup_Complete]",
        "[TriggerGroups:MSB_Scout_TriggerGroup_Complete]",
        "[TriggerGroups:MSB_StrikeForceLeader_TriggerGroup_Complete]",
        "[TriggerGroups:MSB_TaskForceLeader_TriggerGroup_Complete]",
    ]

    for root, _, files in os.walk(directory):
        for filename in files:
            if filename.endswith(".sbc"):
                filepath = os.path.join(root, filename)

                try:
                    with open(filepath, 'r', encoding='utf-8') as file:
                        content = file.read()

                    # Replace target string
                    content = content.replace("FAC-Shipyard-Shipyard", "AaW_Shipyard_MainProfile")

                    # Remove trigger lines
                    for trigger_line in trigger_lines_to_remove:
                        content = content.replace(trigger_line, "")

                    with open(filepath, 'w', encoding='utf-8') as file:
                        file.write(content)

                    print(f"Updated: {filepath}")

                except UnicodeDecodeError as e:
                    print(f"Error reading {filepath}: {e}")

replace_text_in_files('.')
input("Done. Press Enter to exit.")
