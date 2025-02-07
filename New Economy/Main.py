from enum import Enum
import ProfileBuilder as PB
import XMLBuilder as XB
import pandas as pd



Ingots = []
Tools = []
Ammos = []
Consumables = []
Components = []

Ingot1 = []
Ingot2 = []
Ingot3 = []
Ingot4 = []
Ingot5 = []
Component1 = []
Component2 = []
Component3 = []
Ammo1 = []
Ammo2 = []
Ammo3 = []
Tool1 = []
Tool2 = []
Tool3 = []
Consumable1 = []
Consumable2 = []
Consumable3 = []

class Ore():
    def __init__(self, name, Yield):
        self.Name = name
        self.Yield = Yield
        
    def AddScore(self,score):
        if score == 1.0:
            self.Score = Score.one
            Ingot1.append(self)
        if score == 2.0:
            self.Score = Score.two 
            Ingot2.append(self)
        if score == 3.0:
            self.Score = Score.three   
            Ingot3.append(self)
        if score == 4.0:
            self.Score = Score.four 
            Ingot4.append(self)
        if score == 5.0:
            self.Score = Score.five 
            Ingot5.append(self)

        self.scoreint = score
        Ingots.append(self)
        return self
    
    def __str__(self) -> str:
        return self.Name
    
class Requirement():
    def __init__(self, ore:Ore, amount: int):
        self.Ore = ore
        self.Amount = amount

class Score(Enum):
    one = 1
    two = 2
    three = 3
    four = 4
    five = 5

class ItemEnum(Enum):
    Suffice = 1
    Shortage = 2
    BigShortage = 3

class ItemTypeEnum(Enum):
    Component = 1
    Ammo = 2
    Tool = 3
    Consumable = 4


class Item():
    def __init__(self,Type,name, Requirements):
        self.type = Type
        self.Name = name
        self.Requirements = Requirements
        self.Classify()
        self.AddToList()

    def __str__(self) -> str:
        return self.Name

    def Classify(self):

        if self.Requirements == 0:
            self.Class = ItemEnum.Suffice
            return self
        
        Total = 0
        TotalMissing = 0

        for requirement in self.Requirements:
            amount = round(requirement.Amount/requirement.Ore.Yield,1)
            Total += amount
            if requirement.Ore.Score == Score.one:
                continue
            if requirement.Ore.Score == Score.two:
                TotalMissing += 0.1 * amount
                continue
            if requirement.Ore.Score == Score.three:
                TotalMissing += 0.3 * amount
                continue
            if requirement.Ore.Score == Score.four:
                TotalMissing += 0.6 * amount
                continue
            if requirement.Ore.Score == Score.five:
                TotalMissing += 0.8 * amount
                continue

        factor = TotalMissing/Total
        if factor < 0.1:
            self.Class = ItemEnum.Suffice
        if factor >=0.1 and factor < 0.5:
            self.Class = ItemEnum.Shortage
        if factor >=0.5:
            self.Class = ItemEnum.BigShortage     

        return self
    
    def AddToList(self):
        if self.type == ItemTypeEnum.Tool:
           Tools.append(self)
           if self.Class == ItemEnum.Suffice:
                Tool1.append(self)
           if self.Class == ItemEnum.Shortage:
                Tool2.append(self)
           if self.Class == ItemEnum.BigShortage:
                Tool3.append(self)
        if self.type == ItemTypeEnum.Consumable:
           Consumables.append(self)
           if self.Class == ItemEnum.Suffice:
                Consumable1.append(self)
           if self.Class == ItemEnum.Shortage:
                Consumable2.append(self)
           if self.Class == ItemEnum.BigShortage:
                Consumable3.append(self)
        if self.type == ItemTypeEnum.Ammo:
           Ammos.append(self)
           if self.Class == ItemEnum.Suffice:
                Ammo1.append(self)
           if self.Class == ItemEnum.Shortage:
                Ammo2.append(self)
           if self.Class == ItemEnum.BigShortage:
                Ammo3.append(self)              
        if self.type == ItemTypeEnum.Component:
           Components.append(self)
           if self.Class == ItemEnum.Suffice:
                Component1.append(self)
           if self.Class == ItemEnum.Shortage:
                Component2.append(self)
           if self.Class == ItemEnum.BigShortage:
                Component3.append(self)



df = pd.read_csv('XML.csv', header=0)



for index, row in df.iterrows():
    Ingots = []
    Tools = []
    Ammos = []
    Consumables = []
    Components = []

    Ingot1 = []
    Ingot2 = []
    Ingot3 = []
    Ingot4 = []
    Ingot5 = []
    Component1 = []
    Component2 = []
    Component3 = []
    Ammo1 = []
    Ammo2 = []
    Ammo3 = []
    Tool1 = []
    Tool2 = []
    Tool3 = []
    Consumable1 = []
    Consumable2 = []
    Consumable3 = []


    #Name
    XML_ID = row['ZoneName']
    IO = row['IO']  
    XML_Name = row['ZoneName']

    if IO:
        XML_Name = XML_ID + "IO"

    #Setup
    if IO:
        print("It is IO")
        Iron = Ore("Iron",0.7).AddScore(row['Iron'])
        Nickel = Ore("Nickel",4).AddScore(row['Nickel'])
        Silicon = Ore("Silicon",0.7).AddScore(row['Silicon'])
        Copper = Ore("Copper", 0.6).AddScore(row['Copper'])


        Cobalt = Ore("Cobalt",0.3).AddScore(row['Cobalt'])

        Silver = Ore("Silver", 0.1).AddScore(row['Silver'])
        Gold = Ore("Gold",0.01).AddScore(row['Gold'])

        Aluminum = Ore("Aluminum", 0.6).AddScore(row['Aluminum'])
        Titanium = Ore("Titanium", 0.4).AddScore(row['Titanium'])
        Sulfur = Ore("Sulfur", 0.3).AddScore(row['Sulfur'])


        Carbon = Ore("Carbon", 0.75).AddScore(row['Carbon'])
        Polymer = Ore("Polymer", 0.3).AddScore(row['Polymer'])
        FuelOil = Ore("FuelOil", 0.45).AddScore(row['FuelOil'])

        Niter = Ore("Niter",0.007).AddScore(row['Niter'])
        Platinum = Ore("Platinum",0.005).AddScore(row['Platinum'])
        Uranium = Ore("Uranium", 0.01).AddScore(row['Uranium'])
        Lithium = Ore("Lithium", 0.2).AddScore(row['Lithium'])
        Tantalum = Ore("Tantalum", 0.01).AddScore(row['Tantalum'])


        CrystalPrism = Ore("CrystalPrism", 1).AddScore(row['CrystalPrism'])
        DoriumIngot = Ore("DoriumIngot", 1).AddScore(row['DoriumIngot'])
        StabilizedCronyx = Ore("StabilizedCronyx", 1).AddScore(row['StabilizedCronyx'])
        PurifiedSpice = Ore("PurifiedSpice", 1).AddScore(row['PurifiedSpice'])

        Item(ItemTypeEnum.Component, "SteelPlate", 0)
        Item(ItemTypeEnum.Component, "Construction", 0)
        Item(ItemTypeEnum.Component, "Girder", 0)

        Item(ItemTypeEnum.Component, "Construction", 0)
        Item(ItemTypeEnum.Component, "SmallTube", 0)
        Item(ItemTypeEnum.Component, "LargeTube", 0)
        Item(ItemTypeEnum.Component, "BulletproofGlass", 0)
        Item(ItemTypeEnum.Component, "Motor", 0)

        Item(ItemTypeEnum.Component, "Computer", 0)
        Item(ItemTypeEnum.Component, "RadioCommunicationComponent", 0)
        Item(ItemTypeEnum.Component, "Detector", 0)
        Item(ItemTypeEnum.Component, "Canvas", 0)
        Item(ItemTypeEnum.Component, "SolarCell", 0)


        Item(ItemTypeEnum.Component, "Display", [Requirement(Copper, 1),
                                Requirement(Silicon, 4),
                                Requirement(Polymer, 3)])

        Item(ItemTypeEnum.Component, "InteriorPlate", [Requirement(Aluminum, 2)])

        Item(ItemTypeEnum.Component, "PowerCell", [Requirement(Lithium, 10),
                                Requirement(Aluminum, 4),
                                Requirement(Carbon, 3),
                                Requirement(Copper, 3)])

        Item(ItemTypeEnum.Component, "Explosives", [Requirement(Niter, 2),
                                Requirement(Silicon, 0.5)])
        
        Item(ItemTypeEnum.Component, "MedicalComponent", [Requirement(Iron, 60),
                                Requirement(Silver, 20),
                                Requirement(Nickel, 70)])
        Item(ItemTypeEnum.Component, "ReactorComponent", [Requirement(Iron, 15),
                                Requirement(Silver, 5),
                                Requirement(Polymer, 10)])
        Item(ItemTypeEnum.Component, "MetalGrid", [Requirement(Iron, 12),
                                Requirement(Nickel, 5),
                                Requirement(Cobalt, 3)])
        Item(ItemTypeEnum.Component, "ThrustComponent", [Requirement(Iron, 12),
                                Requirement(Platinum, 0.4),
                                Requirement(Gold, 1),                       
                                Requirement(Cobalt, 10)])
        Item(ItemTypeEnum.Component, "GravityGeneratorComponent", [Requirement(Tantalum, 5),
                                Requirement(Silver, 20),
                                Requirement(Polymer, 60),                       
                                Requirement(Cobalt, 25),
                                Requirement(Silver, 20)])
        Item(ItemTypeEnum.Component, "Superconductor", [Requirement(Polymer, 6), Requirement(Gold, 15)])


    # FSSolarCell
        Item(ItemTypeEnum.Component, "FSSolarCell", [Requirement(Titanium, 0.5),
                                Requirement(Tantalum, 0.2),
                                Requirement(Polymer, 10),                       
                                Requirement(Gold, 1)])
    # ElectronMatrix
        Item(ItemTypeEnum.Component, "ElectronMatrix", [Requirement(Tantalum, 0.5),
                                                        Requirement(Titanium, 3),
                                                        Requirement(Polymer, 1),
                                                        Requirement(Silver, 3),
                                                        Requirement(Lithium, 3)])
        

    # Capacitor
        Item(ItemTypeEnum.Component, "Capacitor", [Requirement(Polymer, 1),
                                                        Requirement(Carbon, 3),
                                                        Requirement(Aluminum, 3)])

    # CompositeArmor
        Item(ItemTypeEnum.Component, "CompositeArmor", [Requirement(Titanium, 3),
                                                        Requirement(Carbon, 3)])

    # TokamakBlanket
        Item(ItemTypeEnum.Component, "TokamakBlanket", [Requirement(Lithium, 2),
                                                        Requirement(Titanium, 3),
                                                        Requirement(Polymer, 1),
                                                        Requirement(Aluminum, 3)])

    # SuperMagnet
        Item(ItemTypeEnum.Component, "SuperMagnet", [Requirement(Tantalum, 1),
                                                        Requirement(Titanium, 2),
                                                        Requirement(Gold, 2),
                                                        Requirement(Aluminum, 3),
                                                        Requirement(Polymer, 1)])

    # Cryocooler
        Item(ItemTypeEnum.Component, "Cryocooler", [Requirement(Silicon, 10),
                                                        Requirement(Copper, 3),
                                                        Requirement(Polymer, 1),
                                                        Requirement(Aluminum, 3)])
        

    # Thermocouple
        Item(ItemTypeEnum.Component, "Thermocouple", [Requirement(Silicon, 10),
                                                        Requirement(Copper, 3),
                                                        Requirement(Polymer, 1),
                                                        Requirement(Aluminum, 3)])

    # ArmorGlass
        Item(ItemTypeEnum.Component, "ArmorGlass", [Requirement(Aluminum, 3)])


    #Cupper Wire
        Item(ItemTypeEnum.Component, "Cupper", [Requirement(Copper, 1)])

    # GoldWire
        Item(ItemTypeEnum.Component, "GoldWire", [Requirement(Gold, 0.6)])


    # Rubber
        Item(ItemTypeEnum.Component, "Rubber", [Requirement(Polymer, 2)])
    # Plastic
        Item(ItemTypeEnum.Component, "Plastic", [Requirement(Polymer, 1.5)])
    # AdvancedComputer
        Item(ItemTypeEnum.Component, "AdvancedComputer", [Requirement(Gold, 3),
                                                        Requirement(Silicon, 2),
                                                        Requirement(Polymer, 2)])


    # QuantumComputer
        Item(ItemTypeEnum.Component, "QuantumComputer", [Requirement(Polymer, 1.5),
                                                        Requirement(Gold, 1),
                                                        Requirement(Aluminum, 3),
                                                        Requirement(Platinum, 0.2),
                                                        Requirement(Tantalum, 0.1),
                                                        Requirement(Silicon, 2)])
    #Concrete
        Item(ItemTypeEnum.Component, "Concrete", 0)


    # TitaniumPlate
        Item(ItemTypeEnum.Component, "TitaniumPlate", [Requirement(Titanium, 3)])

    # ArmoredPlate
        Item(ItemTypeEnum.Component, "ArmoredPlate", [Requirement(Titanium, 1)])

    # Electromagnet
        Item(ItemTypeEnum.Component, "Electromagnet", 0)
    # Lightbulb
        Item(ItemTypeEnum.Component, "Lightbulb", 0)

    # AcidPowerCell
        Item(ItemTypeEnum.Component, "AcidPowerCell", [Requirement(Iron, 1.5),
                                                        Requirement(Copper, 5),
                                                        Requirement(Sulfur, 53),])

    # AlkalinePowerCell
        Item(ItemTypeEnum.Component, "AlkalinePowerCell", 0)
    # Ceramic
        Item(ItemTypeEnum.Component, "Ceramic", [Requirement(Silicon, 4),
                                                        Requirement(Carbon, 4)])
    # LaserEmitter
        Item(ItemTypeEnum.Component, "LaserEmitter", [Requirement(Silicon, 10),
                                                        Requirement(Silver, 1),
                                                        Requirement(Lithium, 1),
                                                        Requirement(Aluminum, 3)])

    # HeatingElement
        Item(ItemTypeEnum.Component, "HeatingElement", 0)

        #Strategic Resources
        Item(ItemTypeEnum.Component, "ReinforcedDrillbit", [Requirement(DoriumIngot, 100)])
        Item(ItemTypeEnum.Component, "ReinforcedPlate", [Requirement(DoriumIngot, 100)])
        Item(ItemTypeEnum.Component, "LaserAmplifier", [Requirement(CrystalPrism, 100)])

        Item(ItemTypeEnum.Ammo, "NATO_25x184mm", [Requirement(Iron, 40),
                                Requirement(Niter, 3),
                                Requirement(Nickel, 5)])                       

        Item(ItemTypeEnum.Ammo, "Missile200mm", [Requirement(Iron, 55),
                                Requirement(Niter, 0.5),
                                Requirement(Platinum, 0.04),
                                Requirement(Nickel, 7),                   
                                Requirement(Uranium, 0.1)])

        Item(ItemTypeEnum.Ammo, "MediumCalibreAmmo", [Requirement(Iron, 15),            
                                Requirement(Nickel, 2),     
                                Requirement(Niter, 0.5)])

        Item(ItemTypeEnum.Ammo, "LargeCalibreAmmo", [Requirement(Iron, 15),            
                                Requirement(Nickel, 2),   
                                Requirement(Uranium, 2),       
                                Requirement(Niter, 5)])
        Item(ItemTypeEnum.Ammo, "LargeRailgunAmmo", [Requirement(Iron, 20),            
                                Requirement(Nickel, 3),   
                                Requirement(Uranium, 1),       
                                Requirement(Silicon, 30)])

        Item(ItemTypeEnum.Ammo, "SmallRailgunAmmo", [Requirement(Iron, 4),            
                                Requirement(Nickel, 0.5),   
                                Requirement(Uranium, 0.2),       
                                Requirement(Silicon, 5)])

        Item(ItemTypeEnum.Ammo, "AutocannonClip", [Requirement(Iron, 25),            
                                Requirement(Nickel, 3),   
                                Requirement(Niter, 2),       
                                Requirement(Silicon, 5)])

        Item(ItemTypeEnum.Tool, "HandDrill2Item", 0)
        Item(ItemTypeEnum.Tool, "HandDrill3Item", 0)
        Item(ItemTypeEnum.Tool, "AngleGrinder2Item", 0)
        Item(ItemTypeEnum.Tool, "AngleGrinder3Item", 0)
        Item(ItemTypeEnum.Tool, "Welder2Item", 0)
        Item(ItemTypeEnum.Tool, "Welder3Item", 0)
        Item(ItemTypeEnum.Tool, "SemiAutoPistolItem", 0)
        Item(ItemTypeEnum.Tool, "FullAutoPistolItem", 0)
        Item(ItemTypeEnum.Tool, "AutomaticRifleItem", 0)
        Item(ItemTypeEnum.Tool, "PreciseAutomaticRifleItem", 0)
        Item(ItemTypeEnum.Tool, "RapidFireAutomaticRifleItem", 0)

        Item(ItemTypeEnum.Consumable, "ClangCola", 0)
        Item(ItemTypeEnum.Consumable, "CosmicCoffee", 0)
        Item(ItemTypeEnum.Consumable, "Medkit", 0)
        Item(ItemTypeEnum.Consumable, "Powerkit", 0)
        Item(ItemTypeEnum.Consumable, "SpiceInhaler", [Requirement(PurifiedSpice, 100)])
        Item(ItemTypeEnum.Consumable, "FAFSquadron", 0)
    #VANILLA
    else:

        Iron = Ore("Iron",0.7).AddScore(row['Iron'])
        Nickel = Ore("Nickel",4).AddScore(row['Nickel'])
        Silicon = Ore("Silicon",0.7).AddScore(row['Silicon'])
        Cobalt = Ore("Cobalt",0.3).AddScore(row['Cobalt'])
        Magnesium = Ore("Magnesium",0.007).AddScore(row['Magnesium'])
        Silver = Ore("Silver", 0.1).AddScore(row['Silver'])
        Gold = Ore("Gold",0.01).AddScore(row['Gold'])
        Platinum = Ore("Platinum",0.005).AddScore(row['Platinum'])
        Uranium = Ore("Uranium", 0.01).AddScore(row['Uranium'])
        Lithium = Ore("Lithium", 0.2).AddScore(row['Lithium'])
        Sulfur = Ore("Sulfur", 0.3).AddScore(row['Sulfur'])    

        CrystalPrism = Ore("CrystalPrism", 1).AddScore(row['CrystalPrism'])
        DoriumIngot = Ore("DoriumIngot", 1).AddScore(row['DoriumIngot'])
        StabilizedCronyx = Ore("StabilizedCronyx", 1).AddScore(row['StabilizedCronyx'])
        PurifiedSpice = Ore("PurifiedSpice", 1).AddScore(row['PurifiedSpice'])

        Item(ItemTypeEnum.Component, "SteelPlate", 0)
        Item(ItemTypeEnum.Component, "Construction", 0)
        Item(ItemTypeEnum.Component, "Girder", 0)
        Item(ItemTypeEnum.Component, "InteriorPlate", 0)
        Item(ItemTypeEnum.Component, "Construction", 0)
        Item(ItemTypeEnum.Component, "SmallTube", 0)
        Item(ItemTypeEnum.Component, "LargeTube", 0)
        Item(ItemTypeEnum.Component, "BulletproofGlass", 0)
        Item(ItemTypeEnum.Component, "Motor", 0)
        Item(ItemTypeEnum.Component, "Display", 0)
        Item(ItemTypeEnum.Component, "Computer", 0)
        Item(ItemTypeEnum.Component, "RadioCommunicationComponent", 0)
        Item(ItemTypeEnum.Component, "Detector", 0)
        Item(ItemTypeEnum.Component, "Canvas", 0)
        Item(ItemTypeEnum.Component, "SolarCell", 0)
        Item(ItemTypeEnum.Component, "PowerCell", 0)

        Item(ItemTypeEnum.Component, "Explosives", [Requirement(Magnesium, 2),
                                Requirement(Silicon, 0.5)])
        Item(ItemTypeEnum.Component, "MedicalComponent", [Requirement(Iron, 60),
                                Requirement(Silver, 20),
                                Requirement(Nickel, 70)])
        Item(ItemTypeEnum.Component, "ReactorComponent", [Requirement(Iron, 15),
                                Requirement(Silver, 5)])
        Item(ItemTypeEnum.Component, "MetalGrid", [Requirement(Iron, 12),
                                Requirement(Nickel, 5),
                                Requirement(Cobalt, 3)])
        Item(ItemTypeEnum.Component, "ThrustComponent", [Requirement(Iron, 12),
                                Requirement(Platinum, 0.4),
                                Requirement(Gold, 1),                       
                                Requirement(Cobalt, 10)])
        Item(ItemTypeEnum.Component, "GravityGeneratorComponent", [Requirement(Iron, 600),
                                Requirement(Silver, 5),
                                Requirement(Gold, 10),                       
                                Requirement(Cobalt, 220)])
        Item(ItemTypeEnum.Component, "Superconductor", [Requirement(Iron, 10), Requirement(Gold, 2)])

        #Strategic Resources
        Item(ItemTypeEnum.Component, "ReinforcedDrillbit", [Requirement(DoriumIngot, 100)])
        Item(ItemTypeEnum.Component, "ReinforcedPlate", [Requirement(DoriumIngot, 100)])
        Item(ItemTypeEnum.Component, "LaserAmplifier", [Requirement(CrystalPrism, 100)])

        Item(ItemTypeEnum.Ammo, "NATO_25x184mm", [Requirement(Iron, 40),
                                Requirement(Magnesium, 3),
                                Requirement(Nickel, 5)])                       

        Item(ItemTypeEnum.Ammo, "missile", [Requirement(Iron, 55),
                                Requirement(Magnesium, 0.5),
                                Requirement(Platinum, 0.04),
                                Requirement(Nickel, 7),                   
                                Requirement(Uranium, 0.1)])

        Item(ItemTypeEnum.Ammo, "MediumCalibreAmmo", [Requirement(Iron, 15),            
                                Requirement(Nickel, 2),     
                                Requirement(Magnesium, 0.5)])

        Item(ItemTypeEnum.Ammo, "LargeCalibreAmmo", [Requirement(Iron, 15),            
                                Requirement(Nickel, 2),   
                                Requirement(Uranium, 2),       
                                Requirement(Magnesium, 5)])
        Item(ItemTypeEnum.Ammo, "LargeRailgunAmmo", [Requirement(Iron, 20),            
                                Requirement(Nickel, 3),   
                                Requirement(Uranium, 1),       
                                Requirement(Silicon, 30)])

        Item(ItemTypeEnum.Ammo, "SmallRailgunAmmo", [Requirement(Iron, 4),            
                                Requirement(Nickel, 0.5),   
                                Requirement(Uranium, 0.2),       
                                Requirement(Silicon, 5)])

        Item(ItemTypeEnum.Ammo, "AutocannonClip", [Requirement(Iron, 25),            
                                Requirement(Nickel, 3),   
                                Requirement(Magnesium, 2),       
                                Requirement(Silicon, 5)])

        Item(ItemTypeEnum.Tool, "HandDrill2Item", 0)
        Item(ItemTypeEnum.Tool, "HandDrill3Item", 0)
        Item(ItemTypeEnum.Tool, "AngleGrinder2Item", 0)
        Item(ItemTypeEnum.Tool, "AngleGrinder3Item", 0)
        Item(ItemTypeEnum.Tool, "Welder2Item", 0)
        Item(ItemTypeEnum.Tool, "Welder3Item", 0)
        Item(ItemTypeEnum.Tool, "SemiAutoPistolItem", 0)
        Item(ItemTypeEnum.Tool, "FullAutoPistolItem", 0)
        Item(ItemTypeEnum.Tool, "AutomaticRifleItem", 0)
        Item(ItemTypeEnum.Tool, "PreciseAutomaticRifleItem", 0)
        Item(ItemTypeEnum.Tool, "RapidFireAutomaticRifleItem", 0)

        Item(ItemTypeEnum.Consumable, "ClangCola", 0)
        Item(ItemTypeEnum.Consumable, "CosmicCoffee", 0)
        Item(ItemTypeEnum.Consumable, "Medkit", 0)
        Item(ItemTypeEnum.Consumable, "Powerkit", 0)
        Item(ItemTypeEnum.Consumable, "SpiceInhaler", [Requirement(PurifiedSpice, 100)])
        Item(ItemTypeEnum.Consumable, "FAFSquadron", 0)



    XB.CreateXml(XML_Name,
                XB.GenerateXMLIngot(Ingot1),
                XB.GenerateXMLIngot(Ingot2),
                XB.GenerateXMLIngot(Ingot3),
                XB.GenerateXMLIngot(Ingot4),
                XB.GenerateXMLIngot(Ingot5),
                XB.GenerateXMLComponent(Component1),
                XB.GenerateXMLComponent(Component2),
                XB.GenerateXMLComponent(Component3),
                XB.GenerateXMLAmmo(Ammo1),
                XB.GenerateXMLAmmo(Ammo2),
                XB.GenerateXMLAmmo(Ammo3),
                XB.GenerateXMLTool(Tool1),
                XB.GenerateXMLTool(Tool2),
                XB.GenerateXMLTool(Tool3),
                XB.GenerateXMLConsumable(Consumable1),
                XB.GenerateXMLConsumable(Consumable2),
                XB.GenerateXMLConsumable(Consumable3))

    df = pd.read_csv('Encounters.csv', header=0)

    for index, row in df.iterrows():

        if row["FileName"] != XML_ID:
            continue
        if row["IO"] != IO:
            continue

        Encounter_Id = row['Name']       
        Faction = row['Faction']
        
        if IO:
            Encounter_Id = Encounter_Id + "IO"


        mission_ids = row['MissionIds'].split(',')
        PB.CreateStoreItems(Faction,Encounter_Id,XML_Name,row['StoreProfiles'],row['TradeIngots'],Ingots,mission_ids)


        
        PB.CreateTriggers(Faction,Encounter_Id, IO)

input("Done")
