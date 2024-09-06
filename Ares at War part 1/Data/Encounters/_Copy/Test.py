import os

# Specify the directory where your folders are located
directory = 'C:/Users/Gebruiker/AppData/Roaming/SpaceEngineers/Mods/Ares at War part 1/Data/Encounters/_Copy'

# XML content templates
behavior_template = '''<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>{{FAC}}_{folder_name}_Behavior</SubtypeId>
      </Id>
      <Description>
    [RivalAI Behavior]
    [BehaviorName:Passive]
    [TargetData:MSB_{folder_name}_Target]

    [DialogueBanks:{{FAC}}.xml] 
    [DialogueBanks:AaW.xml] 
    [DialogueBanks:MSB.xml] 


    [TriggerGroups:{{FAC}}_StaticCommon_TriggersGroup]
    [TriggerGroups:AaW_StaticCommon_TriggersGroup]    
    [TriggerGroups:MSB_StaticCommon_TriggersGroup]    

    [TriggerGroups:{{FAC}}_FixedStatic_TriggersGroup]        
    [TriggerGroups:AaW_FixedStatic_TriggersGroup]
    [TriggerGroups:MSB_FixedStatic_TriggersGroup]    

    [TriggerGroups:{{FAC}}_{folder_name}_TriggersGroup]    
    [TriggerGroups:AaW_{folder_name}_TriggersGroup]     
    [TriggerGroups:MSB_{folder_name}_TriggersGroup]     

      </Description>
      
    </EntityComponent>        
  </EntityComponents>
</Definitions>
'''

trigger_group_template = '''<?xml version="1.0"?>
<Definitions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <EntityComponents>

    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
          <TypeId>Inventory</TypeId>
          <SubtypeId>{{FAC}}_{folder_name}_TriggersGroup</SubtypeId>
      </Id>
      <Description>

    [RivalAI TriggerGroup]
 
      </Description>
      
    </EntityComponent>    
  </EntityComponents>
</Definitions>
'''

# Loop through each folder in the directory
for folder_name in os.listdir(directory):
    folder_path = os.path.join(directory, folder_name)
    
    # Check if it is a directory
    if os.path.isdir(folder_path):
        # Create the Behavior .sbc file
        behavior_content = behavior_template.format(folder_name=folder_name)
        behavior_file_path = os.path.join(folder_path, f'_{folder_name}Behavior.sbc')
        with open(behavior_file_path, 'w') as behavior_file:
            behavior_file.write(behavior_content)
        
        # Create the TriggersGroup .sbc file
        trigger_group_content = trigger_group_template.format(folder_name=folder_name)
        trigger_group_file_path = os.path.join(folder_path, f'_{folder_name}TriggersGroup.sbc')
        with open(trigger_group_file_path, 'w') as trigger_group_file:
            trigger_group_file.write(trigger_group_content)

print("Files created successfully!")
