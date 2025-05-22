#Store Restaurant-Agaris
#Store Restaurant-Thora4
#Store Restaurant-Space





#GetStoreIngot
#GetStoreCiv
#GetStoreScrap
#GetStoreAmmo
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


#Special
def GetStoreSettlement(XML_Name:str,Faction:str, Name:str, Ingots:list,TradeIngots:str)->str:  
    string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Settlement</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:99]
        [MaxOfferItems:99]
        [MinOrderItems:99]
        [MaxOrderItems:99]

        [ItemsRequireInventory:false]

        {GenerateProfileIngotOre(Ingots,TradeIngots)}

        
        
        [Orders:Component/CivilianProductI] 
        [Orders:Component/CivilianProductII] 
        [Orders:Component/CivilianProductIII] 

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

        [Offers:Ammo/NATO_25x184mm] 
        [Offers:Ammo/missile] 
        [Offers:Ammo/MediumCalibreAmmo] 
        [Offers:Ammo/LargeCalibreAmmo] 
        [Offers:Ammo/LargeRailgunAmmo] 
        [Offers:Ammo/SmallRailgunAmmo] 
        [Offers:Ammo/AutocannonClip] 

        [Offers:Ammo/PreciseAutomaticRifleGun_Mag_5rd] 
        [Offers:Ammo/RapidFireAutomaticRifleGun_Mag_50rd] 
        [Offers:Ammo/AutomaticRifleGun_Mag_20rd] 
        [Offers:Ammo/NATO_5p56x45mm] 

        [Offers:Ammo/UltimateAutomaticRifleGun_Mag_30rd] 
        [Offers:Ammo/SemiAutoPistolMagazine] 
        [Offers:Ammo/FullAutoPistolMagazine] 
        [Offers:Ammo/ElitePistolMagazine] 




        [Offers:Tool/SemiAutoPistolItem] 
        [Offers:Tool/FullAutoPistolItem] 
        [Offers:Tool/MediumCalibreAmmo] 
        [Offers:Tool/AutomaticRifleItem] 
        [Offers:Tool/PreciseAutomaticRifleItem] 
        [Offers:Tool/RapidFireAutomaticRifleItem] 
  
      </Description>

    </EntityComponent>"""

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
        
        [MinOfferItems:99]
        [MaxOfferItems:99]
        [MinOrderItems:99]
        [MaxOrderItems:99]

        [ItemsRequireInventory:false]

        {GenerateProfileIngotOre(Ingots,TradeIngots)}

        [Offers:Ammo/NATO_25x184mm] 
        [Offers:Ammo/missile] 
        [Offers:Ammo/MediumCalibreAmmo] 
        [Offers:Ammo/LargeCalibreAmmo] 
        [Offers:Ammo/LargeRailgunAmmo] 
        [Offers:Ammo/SmallRailgunAmmo] 
        [Offers:Ammo/AutocannonClip] 

        [Offers:Ammo/PreciseAutomaticRifleGun_Mag_5rd] 
        [Offers:Ammo/RapidFireAutomaticRifleGun_Mag_50rd] 
        [Offers:Ammo/AutomaticRifleGun_Mag_20rd] 
        [Offers:Ammo/NATO_5p56x45mm] 

        [Offers:Ammo/UltimateAutomaticRifleGun_Mag_30rd] 
        [Offers:Ammo/SemiAutoPistolMagazine] 
        [Offers:Ammo/FullAutoPistolMagazine] 
        [Offers:Ammo/ElitePistolMagazine] 

        [Offers:Tool/SemiAutoPistolItem] 
        [Offers:Tool/FullAutoPistolItem] 
        [Offers:Tool/MediumCalibreAmmo] 
        [Offers:Tool/AutomaticRifleItem] 
        [Offers:Tool/PreciseAutomaticRifleItem] 
        [Offers:Tool/RapidFireAutomaticRifleItem] 
  
      </Description>

    </EntityComponent>"""

    return string    
    
#Special
def GetStoreTradestation(XML_Name:str,Faction:str, Name:str, Ingots:list,TradeIngots:str)->str:  
    string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Tradestation</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:99]
        [MaxOfferItems:99]
        [MinOrderItems:99]
        [MaxOrderItems:99]

        [ItemsRequireInventory:false]

        {GenerateProfileIngotOre(Ingots,TradeIngots)}

        
        
        [Orders:Component/CivilianProductI] 
        [Orders:Component/CivilianProductII] 
        [Orders:Component/CivilianProductIII] 

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

  
      </Description>

    </EntityComponent>"""

    return string

    

# [Offers:Component/ReinforcedDrillbit] 
# [Offers:Component/ReinforcedPlate] 
# [Offers:Component/LaserAmplifier]      
# [Offers:Component/ThrustComponent] 
# [Offers:Component/GravityGeneratorComponent] 
# [Offers:Component/Superconductor] 
#Special
def GetStoreIngot(XML_Name:str,Faction:str, Name:str, Ingots:list,TradeIngots:str)->str:  
    string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Ingot</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:20]
        [MaxOfferItems:20]
        [MinOrderItems:20]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]

        {GenerateProfileIngot(Ingots,TradeIngots)}

      </Description>

    </EntityComponent>"""

    return string

#Static from here
def GetStoreCiv(XML_Name:str,Faction:str, Name:str)->str:  
    string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Civ</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:5]
        [MaxOfferItems:20]
        [MinOrderItems:5]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]

        [Orders:Component/CivilianProductI] 
        [Orders:Component/CivilianProductII] 
        [Orders:Component/CivilianProductIII] 


        [Offers:Tool/HandDrill2Item] 
        [Offers:Tool/AngleGrinder2Item] 
        [Offers:Tool/Welder2Item] 
        [Offers:Consumable/Medkit] 
        [Offers:Consumable/Powerkit] 

        
        [Offers:Component/SteelPlate] 
        [Offers:Component/Motor]         
        [Offers:Component/Girder] 
        [Offers:Component/InteriorPlate] 
        [Offers:Component/Construction]        
        [Offers:Component/Display] 
        [Offers:Component/MedicalComponent] 
        [Offers:Component/MetalGrid] 

      </Description>

    </EntityComponent>"""

    if "IO" in Name:
     string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Civ</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:5]
        [MaxOfferItems:20]
        [MinOrderItems:5]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]


        [Orders:Component/CivilianProductI] 
        [Orders:Component/CivilianProductII] 
        [Orders:Component/CivilianProductIII] 


        [Offers:Tool/HandDrill2Item] 
        [Offers:Tool/AngleGrinder2Item] 
        [Offers:Tool/Welder2Item] 
        [Offers:Consumable/Medkit] 
        [Offers:Consumable/Powerkit] 

        [Offers:Component/SteelPlate] 
        [Offers:Component/Motor]         
        [Offers:Component/Girder] 
        [Offers:Component/InteriorPlate] 
        [Offers:Component/Construction]        
        [Offers:Component/Display] 
        [Offers:Component/MedicalComponent] 
        [Offers:Component/MetalGrid] 
        [Offers:Component/Cupper] 
        [Offers:Component/Lightbulb] 
        [Offers:Component/AcidPowerCell] 
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
        [Offers:Component/ThrustComponent] 
        [Offers:Component/GravityGeneratorComponent] 
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
        [Offers:Component/ThrustComponent] 
        [Offers:Component/GravityGeneratorComponent] 
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

def GetStoreVendingMachine(XML_Name:str,Faction:str, Name:str)->str:  
    string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_VendingMachine</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:99]
        [MaxOfferItems:99]
        [MinOrderItems:99]
        [MaxOrderItems:99]

        [ItemsRequireInventory:false]

        [Offers:Tool/HandDrill2Item] 
        [Offers:Tool/HandDrill3Item] 
        [Offers:Tool/AngleGrinder2Item] 
        [Offers:Tool/AngleGrinder3Item] 
        [Offers:Tool/Welder2Item] 
        [Offers:Tool/Welder3Item] 

        [Offers:Consumable/Medkit] 
        [Offers:Consumable/Powerkit] 


      </Description>

    </EntityComponent>"""


    if "IO" in Name:
     string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Tool</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:5]
        [MaxOfferItems:20]
        [MinOrderItems:5]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]

        [Offers:Tool/HandDrill2Item] 
        [Offers:Tool/HandDrill3Item] 
        [Offers:Tool/AngleGrinder2Item] 
        [Offers:Tool/AngleGrinder3Item] 
        [Offers:Tool/Welder2Item] 
        [Offers:Tool/Welder3Item] 
      </Description>

    </EntityComponent>"""       

    return string



def GetStoreITC(XML_Name:str,Faction:str, Name:str)->str:  
    string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_ITC</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:20]
        [MaxOfferItems:20]
        [MinOrderItems:20]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]

        [Orders:Ingot/CrystalPrism] 
        [Orders:Ingot/StabilizedCronyx] 
        [Orders:Ingot/PurifiedSpice] 

        [Offers:Ingot/DoriumIngot]
        [Offers:Component/ReinforcedDrillbit]
        [Offers:Component/ReinforcedPlate]


      </Description>

    </EntityComponent>"""


    if "IO" in Name:
     string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_ITC</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:20]
        [MaxOfferItems:20]
        [MinOrderItems:20]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]

        [Orders:Ingot/CrystalPrism] 
        [Orders:Ingot/StabilizedCronyx] 
        [Orders:Ingot/PurifiedSpice] 

        [Offers:Ingot/DoriumIngot]
        [Offers:Component/ReinforcedDrillbit]
        [Offers:Component/ReinforcedPlate]
      </Description>

    </EntityComponent>"""       

    return string


def GetStoreUNION(XML_Name:str,Faction:str, Name:str)->str:  
    string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_UNION</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:20]
        [MaxOfferItems:20]
        [MinOrderItems:20]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]

        [Orders:Ammo/NATO_25x184mm] 
        [Orders:Ammo/missile] 
        [Orders:Ammo/MediumCalibreAmmo] 
        [Orders:Ammo/LargeCalibreAmmo] 
        [Orders:Ammo/LargeRailgunAmmo] 
        [Orders:Ammo/SmallRailgunAmmo] 
        [Orders:Ammo/AutocannonClip] 





      </Description>

    </EntityComponent>"""


    if "IO" in Name:
     string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_UNION</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:20]
        [MaxOfferItems:20]
        [MinOrderItems:20]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]

        [Orders:Ammo/NATO_25x184mm] 
        [Orders:Ammo/missile] 
        [Orders:Ammo/MediumCalibreAmmo] 
        [Orders:Ammo/LargeCalibreAmmo] 
        [Orders:Ammo/LargeRailgunAmmo] 
        [Orders:Ammo/SmallRailgunAmmo] 
        [Orders:Ammo/AutocannonClip] 

      </Description>

    </EntityComponent>"""       

    return string

def GetStoreSOLCOOP(XML_Name:str,Faction:str, Name:str)->str:  
    string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_UNION</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:20]
        [MaxOfferItems:20]
        [MinOrderItems:20]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]

        [Orders:Ammo/NATO_25x184mm] 
        [Orders:Ammo/missile] 
        [Orders:Ammo/MediumCalibreAmmo] 
        [Orders:Ammo/LargeCalibreAmmo] 
        [Orders:Ammo/LargeRailgunAmmo] 
        [Orders:Ammo/SmallRailgunAmmo] 
        [Orders:Ammo/AutocannonClip] 





      </Description>

    </EntityComponent>"""


    if "IO" in Name:
     string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_UNION</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:20]
        [MaxOfferItems:20]
        [MinOrderItems:20]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]

        [Orders:Ammo/NATO_25x184mm] 
        [Orders:Ammo/missile] 
        [Orders:Ammo/MediumCalibreAmmo] 
        [Orders:Ammo/LargeCalibreAmmo] 
        [Orders:Ammo/LargeRailgunAmmo] 
        [Orders:Ammo/SmallRailgunAmmo] 
        [Orders:Ammo/AutocannonClip] 

      </Description>

    </EntityComponent>"""       

    return string

def GetStoreSYN(XML_Name:str,Faction:str, Name:str)->str:  

    string = f"""
    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_SYN</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:5]
        [MaxOfferItems:20]
        [MinOrderItems:5]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]

        [Offers:Consumable/SpiceInhaler]
        [Orders:Consumable/SpiceInhaler]		
      </Description>
    </EntityComponent>"""


    if "IO" in Name:
     string = f"""
    <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_SYN</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:20]
        [MaxOfferItems:20]
        [MinOrderItems:20]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]

        [Offers:Consumable/SpiceInhaler]
        [Orders:Consumable/SpiceInhaler]		
      </Description>

    </EntityComponent>		"""   

    return string

#Outpost
def GetStoreOutpost(XML_Name:str,Faction:str, Name:str)->str:  
    string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Outpost</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:5]
        [MaxOfferItems:20]
        [MinOrderItems:20]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]

        [Orders:Component/CivilianProductI] 

        [Offers:Tool/HandDrill2Item] 
        [Offers:Tool/AngleGrinder2Item] 
        [Offers:Tool/Welder2Item] 
        [Offers:Consumable/Medkit] 
        [Offers:Consumable/Powerkit] 
        [Offers:HydrogenOffer] 


        

        [Offers:Component/SteelPlate] 
        [Offers:Component/Motor]         
        [Offers:Component/Girder] 
        [Offers:Component/InteriorPlate] 
        [Offers:Component/Construction]        
        [Offers:Component/Display] 
      </Description>

    </EntityComponent>"""

    if "IO" in Name:
     string = f"""
  <EntityComponent xsi:type="MyObjectBuilder_InventoryComponentDefinition">
      <Id>
        <TypeId>Inventory</TypeId>
        <SubtypeId>{Faction}_StoreProfile_{Name}_Outpost</SubtypeId>
      </Id>
      <Description>

        [MES Store]

        [FileSource:AaW_StoreItems_{XML_Name}.xml]
        
        [MinOfferItems:5]
        [MaxOfferItems:20]
        [MinOrderItems:20]
        [MaxOrderItems:20]

        [ItemsRequireInventory:false]


        [Orders:Component/CivilianProductI] 


        [Offers:Tool/HandDrill2Item] 
        [Offers:Tool/AngleGrinder2Item] 
        [Offers:Tool/Welder2Item] 
        [Offers:Consumable/Medkit] 
        [Offers:Consumable/Powerkit] 

        [Offers:Component/SteelPlate] 
        [Offers:Component/Motor]         
        [Offers:Component/Girder] 
        [Offers:Component/InteriorPlate] 
        [Offers:Component/Construction]        
        [Offers:Component/Display] 
        [Offers:Component/Cupper] 
        [Offers:Component/Lightbulb] 
        [Offers:Component/AcidPowerCell] 
      </Description>

    </EntityComponent>"""       

    return string





