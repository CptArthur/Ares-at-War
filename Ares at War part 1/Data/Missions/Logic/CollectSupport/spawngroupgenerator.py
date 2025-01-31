destinations = ["UNION", "MILITIA", "REMNANTS"]

spawn_group_template = '''<SpawnGroup>
    <Id>
        <TypeId>SpawnGroupDefinition</TypeId> 
        <SubtypeId>AaW_SpawnGroup_CollectSupport_{destination}</SubtypeId>
    </Id>
    <Description>
    [Modular Encounters SpawnGroup]

    [SpawnConditionsProfiles:AaW_SpawnProfile_RivalAISpawn]

    [UseRandomNameGenerator:true]
    [RandomGridNamePrefix:]
    [RandomGridNamePattern:Supplies for the {destination}]    
[ReplaceBeaconNameWithRandomizedName:Beacon]
[ReplaceBeaconHudTextInsteadOfName:true]

    [UseRivalAi:true]
    [RivalAiReplaceRemoteControl:true]

    </Description>
    <Icon>Textures\GUI\Icons\Fake.dds</Icon>  
    <IsPirate>true</IsPirate>
    
    <Frequency>5.0</Frequency>
    <Prefabs>

        <Prefab SubtypeId="AaW_Transport_Cargo">
            <Position>
                <X>0.0</X>
                <Y>0.0</Y>
                <Z>0.0</Z>
            </Position>
            <Speed>25</Speed>
            <Behaviour>AaW_Transport_Behavior</Behaviour>
        </Prefab>

    </Prefabs>            
</SpawnGroup>'''

# Build the XML
output = "<?xml version=\"1.0\"?>\n<Definitions xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n\t<SpawnGroups>\n"

for destination in destinations:
    spawn_group = spawn_group_template.format(destination=destination)
    output += f"\t\t{spawn_group}\n"

output += "\t</SpawnGroups>\n</Definitions>"

# Save to a file
with open("CollectSupport_SpawnGroup.sbc", "w") as file:
    file.write(output)

print("Spawn groups XML file generated as 'spawn_groups.xml'")
input()
