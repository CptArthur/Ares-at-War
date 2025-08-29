
def GenerateProfileIngot(Ingots:list,TradeIngots:str):
    string = "\n"
    for ingot in Ingots:
        if ingot.Name not in TradeIngots:
           continue
        string += f"        [Orders:Ingot/{ingot}] \n"
    string += "\n"
    for ingot in Ingots:
        if ingot.scoreint > 2:
            continue
        if ingot.Name not in TradeIngots:
           continue
        string += f"        [Offers:Ingot/{ingot}] \n"
    return string

def GenerateProfileIngotOre(Ingots:list,TradeIngots:str):
    string = "\n"
    for ingot in Ingots:
        if ingot.scoreint > 2:
            continue
        if ingot.Name not in TradeIngots:
           continue
        string += f"        [Orders:Ore/{ingot}] \n"
    string += "\n"
    for ingot in Ingots:
        if ingot.scoreint > 2:
            continue
        if ingot.Name not in TradeIngots:
           continue
        string += f"        [Offers:Ingot/{ingot}] \n"
    return string


def GenerateMissionIds(MissionIds:list):
    if len(MissionIds) == 0:
        return ""
    string = "\n"
    for id in MissionIds:
        string += f"        [MissionIds:AaW_Mission_{id}] \n"

    return string


#Special
def GetContractFaction(Faction:str, Name:str,contractFaction ,MissionIds)->str:  
    string = f"""
    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_ContractBlockProfile_{Name}_{contractFaction}</SubtypeId>
      </Id>
      <Description>
        [MES Contract Block]

        [StoreProfileId:{Faction}_StoreProfile_{Name}_Ingot]
        
        [MinContracts:5]
        [MaxContracts:15]
        {GenerateMissionIds(MissionIds)}
        
      </Description>
    </EntityComponent>	"""

    return string


    
#Special
def GetStoreMilitary(XML_Name:str,Faction:str, Name:str, Ingots:list,TradeIngots:str)->str:  
    string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Military</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:2]
        [MaxOfferItems:5]
        [MinOrderItems:7]
        [MaxOrderItems:11]

        [ItemsRequireInventory:false]


        [RequiredOrders:Component/SteelPlate] 
        [RequiredOrders:Component/MetalGrid]
        [Orders:Ammo/NATO_25x184mm] 
        [Orders:Ammo/missile] 
        [Orders:Ammo/MediumCalibreAmmo] 
        [Orders:Ammo/LargeCalibreAmmo] 
        [Orders:Ammo/LargeRailgunAmmo] 
        [Orders:Ammo/SmallRailgunAmmo] 
        [Orders:Ammo/AutocannonClip] 

        
        [RequiredOffers:Tool/AutomaticRifleItem] 
        [RequiredOffers:Tool/PreciseAutomaticRifleItem] 
        [RequiredOffers:Tool/RapidFireAutomaticRifleItem] 
        [RequiredOffers:Ammo/PreciseAutomaticRifleGun_Mag_5rd] 
        [RequiredOffers:Ammo/RapidFireAutomaticRifleGun_Mag_50rd] 
        [RequiredOffers:Ammo/AutomaticRifleGun_Mag_20rd] 
        [RequiredOffers:Ammo/UltimateAutomaticRifleGun_Mag_30rd] 

        [Offers:Component/Construction] 
        [Offers:Component/Computer] 
        [Offers:Component/Motor] 
        [Offers:Component/SolarCell] 
        [Offers:Component/AlkalinePowerCell] 
        [Offers:Component/Girder] 
        [Offers:Component/InteriorPlate] 
          
      </Description>
    </EntityComponent>"""

    return string    

def GetStoreScrap(XML_Name:str,Faction:str, Name:str)->str:  
    string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Salvage</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:5]
        [MaxOfferItems:20]
        [MinOrderItems:5]
        [MaxOrderItems:5]

        [ItemsRequireInventory:false]
        
        [Offers:Component/SteelPlate] 
        [Offers:Component/Construction] 
        [Offers:Component/Girder] 
        [Offers:Component/InteriorPlate] 
        [Offers:Component/Construction] 
        [Offers:Component/SmallTube] 
        [Offers:Component/LargeTube] 
        [Offers:Component/BulletproofGlass] 
        [Offers:Component/Motor] 
        [Offers:Component/Display] 
        [Offers:Component/Computer] 
        [Offers:Component/RadioCommunicationComponent] 
        [Offers:Component/Detector] 
        [Offers:Component/Canvas] 
        [Offers:Component/SolarCell] 
        [Offers:Component/PowerCell] 
        [Offers:Component/Explosives] 
        [Offers:Component/MedicalComponent] 
        [Offers:Component/ReactorComponent] 
        [Offers:Component/MetalGrid] 
        [Offers:Component/Thrust] 
        [Offers:Component/GravityGenerator] 
        [Offers:Component/Superconductor] 
        [Offers:Component/ReinforcedDrillbit] 
        [Offers:Component/ReinforcedPlate] 
        [Offers:Component/LaserAmplifier] 

      </Description>

    </EntityComponent>"""


    if "IO" in Name:
     string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Salvage</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:5]
        [MaxOfferItems:5]
        [MinOrderItems:5]
        [MaxOrderItems:5]

        [ItemsRequireInventory:false]

        [Offers:Component/SteelPlate] 
        [Offers:Component/Construction] 
        [Offers:Component/Girder] 
        [Offers:Component/Construction] 
        [Offers:Component/SmallTube] 
        [Offers:Component/LargeTube] 
        [Offers:Component/BulletproofGlass] 
        [Offers:Component/Motor] 
        [Offers:Component/Computer] 
        [Offers:Component/RadioCommunicationComponent] 
        [Offers:Component/Detector] 
        [Offers:Component/Canvas] 
        [Offers:Component/SolarCell] 
        [Offers:Component/Display] 
        [Offers:Component/InteriorPlate] 
        [Offers:Component/PowerCell] 
        [Offers:Component/Explosives] 
        [Offers:Component/MedicalComponent] 
        [Offers:Component/ReactorComponent] 
        [Offers:Component/MetalGrid] 
        [Offers:Component/Thrust] 
        [Offers:Component/GravityGenerator] 
        [Offers:Component/Superconductor] 
        [Offers:Component/FSSolarCell] 
        [Offers:Component/ElectronMatrix] 
        [Offers:Component/Capacitor] 
        [Offers:Component/CompositeArmor] 
        [Offers:Component/TokamakBlanket] 
        [Offers:Component/SuperMagnet] 
        [Offers:Component/Cryocooler] 
        [Offers:Component/Thermocouple] 
        [Offers:Component/ArmorGlass] 
        [Offers:Component/Cupper] 
        [Offers:Component/GoldWire] 
        [Offers:Component/Rubber] 
        [Offers:Component/Plastic] 
        [Offers:Component/AdvancedComputer] 
        [Offers:Component/QuantumComputer] 
        [Offers:Component/Concrete] 
        [Offers:Component/TitaniumPlate] 
        [Offers:Component/ArmoredPlate] 
        [Offers:Component/Electromagnet] 
        [Offers:Component/Lightbulb] 
        [Offers:Component/AcidPowerCell] 
        [Offers:Component/AlkalinePowerCell] 
        [Offers:Component/Ceramic] 
        [Offers:Component/LaserEmitter] 
        [Offers:Component/HeatingElement] 
        [Offers:Component/ReinforcedDrillbit] 
        [Offers:Component/ReinforcedPlate] 
        [Offers:Component/LaserAmplifier] 
      </Description>

    </EntityComponent>"""       

    return string

#Static from here
def GetStoreAmmo(XML_Name:str,Faction:str, Name:str)->str:  
    string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Ammo</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:5]
        [MaxOfferItems:20]
        [MinOrderItems:5]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]


        [Offers:Ammo/NATO_25x184mm] 
        [Offers:Ammo/missile] 
        [Offers:Ammo/MediumCalibreAmmo] 
        [Offers:Ammo/LargeCalibreAmmo] 
        [Offers:Ammo/LargeRailgunAmmo] 
        [Offers:Ammo/SmallRailgunAmmo] 
        [Offers:Ammo/AutocannonClip] 
      </Description>

    </EntityComponent>"""


    if "IO" in Name:
     string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Ammo</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:5]
        [MaxOfferItems:20]
        [MinOrderItems:5]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]


        [Offers:Ammo/NATO_25x184mm] 
        [Offers:Ammo/missile] 
        [Offers:Ammo/MediumCalibreAmmo] 
        [Offers:Ammo/LargeCalibreAmmo] 
        [Offers:Ammo/LargeRailgunAmmo] 
        [Offers:Ammo/SmallRailgunAmmo] 
        [Offers:Ammo/AutocannonClip] 
      </Description>

    </EntityComponent>"""       

    return string



#Static from here
def GetStoreSECURITY(XML_Name:str,Faction:str, Name:str)->str:  
    string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_SECURITY</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:5]
        [MaxOfferItems:20]
        [MinOrderItems:5]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]


        [Offers:Ammo/NATO_25x184mm] 
        [Offers:Ammo/missile] 
        [Offers:Ammo/MediumCalibreAmmo] 
        [Offers:Ammo/LargeCalibreAmmo] 
        [Offers:Ammo/LargeRailgunAmmo] 
        [Offers:Ammo/SmallRailgunAmmo] 
        [Offers:Ammo/AutocannonClip] 

        [Offers:Tool/SemiAutoPistolItem] 
        [Offers:Tool/FullAutoPistolItem] 
        [Offers:Tool/MediumCalibreAmmo] 
        [Offers:Tool/AutomaticRifleItem] 
        [Offers:Tool/PreciseAutomaticRifleItem] 
        [Offers:Tool/RapidFireAutomaticRifleItem] 

      </Description>

    </EntityComponent>"""


    if "IO" in Name:
     string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_SECURITY</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:5]
        [MaxOfferItems:20]
        [MinOrderItems:5]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]


        [Offers:Ammo/NATO_25x184mm] 
        [Offers:Ammo/missile] 
        [Offers:Ammo/MediumCalibreAmmo] 
        [Offers:Ammo/LargeCalibreAmmo] 
        [Offers:Ammo/LargeRailgunAmmo] 
        [Offers:Ammo/SmallRailgunAmmo] 
        [Offers:Ammo/AutocannonClip] 

        [Offers:Tool/SemiAutoPistolItem] 
        [Offers:Tool/FullAutoPistolItem] 
        [Offers:Tool/MediumCalibreAmmo] 
        [Offers:Tool/AutomaticRifleItem] 
        [Offers:Tool/PreciseAutomaticRifleItem] 
        [Offers:Tool/RapidFireAutomaticRifleItem] 

      </Description>

    </EntityComponent>"""       

    return string

