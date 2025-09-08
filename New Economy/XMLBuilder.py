

def GenerateXMLIngot(Ingots:list):
    if len(Ingots) == 0:
        return ""
    
    string = "\n      <ItemSubtypeIds> \n"
    for ingot in Ingots:
        string += f"        <ItemSubtypeId>{ingot}</ItemSubtypeId> \n"
    string += "      </ItemSubtypeIds>"

    return string

def GenerateXMLTool(Tools:list):
    if len(Tools) == 0:
        return ""    
    string = "\n      <ItemSubtypeIds> \n"
    for Tool in Tools:
        string += f"        <ItemSubtypeId>{Tool}</ItemSubtypeId>  \n"
    string += "      </ItemSubtypeIds>"
    return string

def GenerateXMLAmmo(Ammos:list):
    if len(Ammos) == 0:
        return ""
    string = "\n      <ItemSubtypeIds> \n"
    for Ammo in Ammos:
        string += f"        <ItemSubtypeId>{Ammo}</ItemSubtypeId> \n"
    string += "      </ItemSubtypeIds>"
    return string

def GenerateXMLConsumable(Consumables:list):
    if len(Consumables) == 0:
        return ""
    string = "\n      <ItemSubtypeIds> \n"
    for Consumable in Consumables:
        string += f"        <ItemSubtypeId>{Consumable}</ItemSubtypeId>  \n"
    string += "      </ItemSubtypeIds>"    
    return string
def GenerateXMLComponent(Components:list):
    if len(Components) == 0:
        return ""
    string = "\n      <ItemSubtypeIds> \n"
    for Component in Components:
        string += f"        <ItemSubtypeId>{Component}</ItemSubtypeId>  \n"
    string += "      </ItemSubtypeIds>"
    return string






def CreateXml(Name:str,Ingot1, Ingot2,Ingot3,Ingot4,Ingot5,Component1,Component2,Component3,Ammo1,Ammo2,Ammo3,Tool1,Tool2,Tool3,Consumable1,Consumable2,Consumable3):
    StoreItems = f"""<?xml version="1.0" encoding="utf-16"?>
<StoreItemsContainer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <StoreItems>


    <StoreItem>
      <StoreItemId>Ingot1</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot1}
      <Offer>
        <MinPriceMultiplier>50</MinPriceMultiplier>
        <MaxPriceMultiplier>60</MaxPriceMultiplier>
        <MinAmount>1000</MinAmount>
        <MaxAmount>10000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>10</MinPriceMultiplier>
        <MaxPriceMultiplier>20</MaxPriceMultiplier>
        <MinAmount>1000</MinAmount>
        <MaxAmount>10000</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Ingot2</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot2}
      <Offer>
        <MinPriceMultiplier>80</MinPriceMultiplier>
        <MaxPriceMultiplier>90</MaxPriceMultiplier>
        <MinAmount>1000</MinAmount>
        <MaxAmount>3000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>65</MinPriceMultiplier>
        <MaxPriceMultiplier>70</MaxPriceMultiplier>
        <MinAmount>1000</MinAmount>
        <MaxAmount>3000</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ingot3</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot3}
      <Offer>
        <MinPriceMultiplier>120</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>300</MinAmount>
        <MaxAmount>2000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>108</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>300</MinAmount>
        <MaxAmount>2000</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ingot4</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot4}
      <Offer>
        <MinPriceMultiplier>145</MinPriceMultiplier>
        <MaxPriceMultiplier>160</MaxPriceMultiplier>
        <MinAmount>100</MinAmount>
        <MaxAmount>800</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>125</MinPriceMultiplier>
        <MaxPriceMultiplier>135</MaxPriceMultiplier>
        <MinAmount>100</MinAmount>
        <MaxAmount>800</MaxAmount>
      </Order>
    </StoreItem>	

    
    <StoreItem>
      <StoreItemId>Ingot5</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot5}
      <Offer>
        <MinPriceMultiplier>190</MinPriceMultiplier>
        <MaxPriceMultiplier>210</MaxPriceMultiplier>
        <MinAmount>100</MinAmount>
        <MaxAmount>900</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>170</MinPriceMultiplier>
        <MaxPriceMultiplier>180</MaxPriceMultiplier>
        <MinAmount>100</MinAmount>
        <MaxAmount>900</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ore1</StoreItemId>
      <ItemType>Ore</ItemType>{Ingot1}
      <Offer>
        <MinPriceMultiplier>500</MinPriceMultiplier>
        <MaxPriceMultiplier>600</MaxPriceMultiplier>
        <MinAmount>10000</MinAmount>
        <MaxAmount>50000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>100</MinPriceMultiplier>
        <MaxPriceMultiplier>200</MaxPriceMultiplier>
        <MinAmount>10000</MinAmount>
        <MaxAmount>50000</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Ore2</StoreItemId>
      <ItemType>Ore</ItemType>{Ingot2}
      <Offer>
        <MinPriceMultiplier>600</MinPriceMultiplier>
        <MaxPriceMultiplier>700</MaxPriceMultiplier>
        <MinAmount>6000</MinAmount>
        <MaxAmount>40000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>650</MinPriceMultiplier>
        <MaxPriceMultiplier>700</MaxPriceMultiplier>
        <MinAmount>6000</MinAmount>
        <MaxAmount>40000</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ore3</StoreItemId>
      <ItemType>Ore</ItemType>{Ingot3}
      <Offer>
        <MinPriceMultiplier>800</MinPriceMultiplier>
        <MaxPriceMultiplier>900</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>1080</MinPriceMultiplier>
        <MaxPriceMultiplier>1000</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ore4</StoreItemId>
      <ItemType>Ore</ItemType>{Ingot4}
      <Offer>
        <MinPriceMultiplier>900</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>1250</MinPriceMultiplier>
        <MaxPriceMultiplier>1350</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Order>
    </StoreItem>	

    
    <StoreItem>
      <StoreItemId>Ore5</StoreItemId>
      <ItemType>Ore</ItemType>{Ingot5}
      <Offer>
        <MinPriceMultiplier>1000</MinPriceMultiplier>
        <MaxPriceMultiplier>1100</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>1700</MinPriceMultiplier>
        <MaxPriceMultiplier>1800</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Order>
    </StoreItem>



    <!-- Component -->
    <StoreItem>
      <StoreItemId>Component1</StoreItemId>
      <ItemType>Component</ItemType>{Component1}
      <Offer>
        <MinPriceMultiplier>60</MinPriceMultiplier>
        <MaxPriceMultiplier>70</MaxPriceMultiplier>
        <MinAmount>500</MinAmount>
        <MaxAmount>15000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>20</MinPriceMultiplier>
        <MaxPriceMultiplier>40</MaxPriceMultiplier>
        <MinAmount>500</MinAmount>
        <MaxAmount>15000</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Component2</StoreItemId>
      <ItemType>Component</ItemType>{Component2}
      <Offer>
        <MinPriceMultiplier>120</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>200</MinAmount>
        <MaxAmount>5000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>90</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>200</MinAmount>
        <MaxAmount>5000</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Component3</StoreItemId>
      <ItemType>Component</ItemType>{Component3}
      <Offer>
        <MinPriceMultiplier>190</MinPriceMultiplier>
        <MaxPriceMultiplier>210</MaxPriceMultiplier>
        <MinAmount>50</MinAmount>
        <MaxAmount>200</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>150</MinPriceMultiplier>
        <MaxPriceMultiplier>160</MaxPriceMultiplier>
        <MinAmount>50</MinAmount>
        <MaxAmount>200</MaxAmount>
      </Order>
    </StoreItem>

    <!-- Ammo -->
    <StoreItem>
      <StoreItemId>Ammo1</StoreItemId>
      <ItemType>Ammo</ItemType>{Ammo1}
      <Offer>
        <MinPriceMultiplier>50</MinPriceMultiplier>
        <MaxPriceMultiplier>60</MaxPriceMultiplier>
        <MinAmount>500</MinAmount>
        <MaxAmount>5000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>20</MinPriceMultiplier>
        <MaxPriceMultiplier>40</MaxPriceMultiplier>
        <MinAmount>500</MinAmount>
        <MaxAmount>5000</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ammo2</StoreItemId>
      <ItemType>Ammo</ItemType>{Ammo2}
      <Offer>
        <MinPriceMultiplier>110</MinPriceMultiplier>
        <MaxPriceMultiplier>120</MaxPriceMultiplier>
        <MinAmount>200</MinAmount>
        <MaxAmount>1000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>90</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>200</MinAmount>
        <MaxAmount>1000</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Ammo3</StoreItemId>
      <ItemType>Ammo</ItemType>{Ammo3}
      <Offer>
        <MinPriceMultiplier>140</MinPriceMultiplier>
        <MaxPriceMultiplier>150</MaxPriceMultiplier>
        <MinAmount>50</MinAmount>
        <MaxAmount>200</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>130</MinPriceMultiplier>
        <MaxPriceMultiplier>140</MaxPriceMultiplier>
        <MinAmount>50</MinAmount>
        <MaxAmount>200</MaxAmount>
      </Order>
    </StoreItem>    


    <!-- Tool -->
    <StoreItem>
      <StoreItemId>Tool1</StoreItemId>
      <ItemType>Tool</ItemType>{Tool1}
      <Offer>
        <MinPriceMultiplier>60</MinPriceMultiplier>
        <MaxPriceMultiplier>70</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>10</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>20</MinPriceMultiplier>
        <MaxPriceMultiplier>40</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>10</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Tool2</StoreItemId>
      <ItemType>Tool</ItemType>{Tool2}
      <Offer>
        <MinPriceMultiplier>120</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>10</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>90</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>10</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Tool3</StoreItemId>
      <ItemType>Tool</ItemType>{Tool3}
      <Offer>
        <MinPriceMultiplier>190</MinPriceMultiplier>
        <MaxPriceMultiplier>210</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>10</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>150</MinPriceMultiplier>
        <MaxPriceMultiplier>160</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>10</MaxAmount>
      </Order>
    </StoreItem>

    <!-- Consumable -->
    <StoreItem>
      <StoreItemId>Consumable1</StoreItemId>
      <ItemType>Consumable</ItemType>{Consumable1}
      <Offer>
        <MinPriceMultiplier>60</MinPriceMultiplier>
        <MaxPriceMultiplier>70</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>50</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>20</MinPriceMultiplier>
        <MaxPriceMultiplier>40</MaxPriceMultiplier>
        <MinAmount>50</MinAmount>
        <MaxAmount>50</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Consumable2</StoreItemId>
      <ItemType>Consumable</ItemType>{Consumable2}
      <Offer>
        <MinPriceMultiplier>120</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>20</MinAmount>
        <MaxAmount>100</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>90</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>20</MinAmount>
        <MaxAmount>100</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Consumable3</StoreItemId>
      <ItemType>Consumable</ItemType>{Consumable3}
      <Offer>
        <MinPriceMultiplier>190</MinPriceMultiplier>
        <MaxPriceMultiplier>210</MaxPriceMultiplier>
        <MinAmount>20</MinAmount>
        <MaxAmount>100</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>150</MinPriceMultiplier>
        <MaxPriceMultiplier>160</MaxPriceMultiplier>
        <MinAmount>20</MinAmount>
        <MaxAmount>100</MaxAmount>
      </Order>
    </StoreItem>

    



    <StoreItem>
      <StoreItemId>HydrogenOffer</StoreItemId>
      <ItemType>Hydrogen</ItemType>
      <ItemSubtypeId>Hydrogen</ItemSubtypeId>
      <Offer>
        <MinPriceMultiplier>75</MinPriceMultiplier>
        <MaxPriceMultiplier>125</MaxPriceMultiplier>
        <MinAmount>10000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Offer>
    </StoreItem>

    
    <StoreItem>
      <StoreItemId>OxygenOffer</StoreItemId>
      <ItemType>Oxygen</ItemType>
      <ItemSubtypeId>Oxygen</ItemSubtypeId>
      <Offer>
        <MinPriceMultiplier>65</MinPriceMultiplier>
        <MaxPriceMultiplier>115</MaxPriceMultiplier>
        <MinAmount>10000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Offer>
    </StoreItem>

    <StoreItem>
      <StoreItemId>IceOffer</StoreItemId>
      <ItemType>Ore</ItemType>
      <ItemSubtypeIds> 
        <ItemSubtypeId>Ice</ItemSubtypeId>  
      </ItemSubtypeIds> 

      <Offer>
        <MinPriceMultiplier>110</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>100000</MinAmount>
        <MaxAmount>200000</MaxAmount>
      </Offer>

    </StoreItem>


  </StoreItems>
</StoreItemsContainer>
"""
    with open(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 1/Data/StoreItems/AaW_StoreItems_{Name}.xml', 'w') as f:
        f.write(StoreItems)


def CreateXml_Dynamic(Name:str,Ingot1, Ingot2,Ingot3,Ingot4,Ingot5,Component1,Component2,Component3,Ammo1,Ammo2,Ammo3,Tool1,Tool2,Tool3,Consumable1,Consumable2,Consumable3):
    StoreItems = f"""<?xml version="1.0" encoding="utf-16"?>
<StoreItemsContainer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <StoreItems>
    <StoreItem>
      <StoreItemId>Schematics</StoreItemId>
      <ItemType>Component</ItemType>
      <ItemSubtypeIds> 
        <ItemSubtypeId>AaW_Schematic_CeramicsFurnace</ItemSubtypeId>  
        <ItemSubtypeId>AaW_Schematic_MicroelectronicsFactory</ItemSubtypeId>  
        <ItemSubtypeId>AaW_Schematic_WireDrawer</ItemSubtypeId>  
        <ItemSubtypeId>AaW_Schematic_AutoLoom</ItemSubtypeId>  
        <ItemSubtypeId>AaW_Schematic_PlateStamp</ItemSubtypeId>  
        <ItemSubtypeId>AaW_Schematic_Extruder</ItemSubtypeId>   
        <ItemSubtypeId>AaW_Schematic_MunitionsFactory</ItemSubtypeId>  
        <ItemSubtypeId>AaW_Schematic_Fabricator</ItemSubtypeId> 
        <ItemSubtypeId>AaW_Schematic_Reactor</ItemSubtypeId> 
        <ItemSubtypeId>AaW_Schematic_JumpDrive</ItemSubtypeId>  
      </ItemSubtypeIds> 
      <Offer>
        <MinPriceMultiplier>110</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>3</MinAmount>
        <MaxAmount>3</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>90</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>3</MinAmount>
        <MaxAmount>3</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ingot1</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot1}
      <Offer>
        <MinPriceMultiplier>50</MinPriceMultiplier>
        <MaxPriceMultiplier>60</MaxPriceMultiplier>
        <MinAmount>1000</MinAmount>
        <MaxAmount>10000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>10</MinPriceMultiplier>
        <MaxPriceMultiplier>20</MaxPriceMultiplier>
        <MinAmount>1000</MinAmount>
        <MaxAmount>10000</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Ingot2</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot2}
      <Offer>
        <MinPriceMultiplier>80</MinPriceMultiplier>
        <MaxPriceMultiplier>90</MaxPriceMultiplier>
        <MinAmount>1000</MinAmount>
        <MaxAmount>3000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>65</MinPriceMultiplier>
        <MaxPriceMultiplier>70</MaxPriceMultiplier>
        <MinAmount>1000</MinAmount>
        <MaxAmount>3000</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ingot3</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot3}
      <Offer>
        <MinPriceMultiplier>120</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>300</MinAmount>
        <MaxAmount>2000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>108</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>300</MinAmount>
        <MaxAmount>2000</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ingot4</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot4}
      <Offer>
        <MinPriceMultiplier>145</MinPriceMultiplier>
        <MaxPriceMultiplier>160</MaxPriceMultiplier>
        <MinAmount>100</MinAmount>
        <MaxAmount>800</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>125</MinPriceMultiplier>
        <MaxPriceMultiplier>135</MaxPriceMultiplier>
        <MinAmount>100</MinAmount>
        <MaxAmount>800</MaxAmount>
      </Order>
    </StoreItem>	

    
    <StoreItem>
      <StoreItemId>Ingot5</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot5}
      <Offer>
        <MinPriceMultiplier>190</MinPriceMultiplier>
        <MaxPriceMultiplier>210</MaxPriceMultiplier>
        <MinAmount>100</MinAmount>
        <MaxAmount>900</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>170</MinPriceMultiplier>
        <MaxPriceMultiplier>180</MaxPriceMultiplier>
        <MinAmount>100</MinAmount>
        <MaxAmount>900</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ore1</StoreItemId>
      <ItemType>Ore</ItemType>{Ingot1}
      <Offer>
        <MinPriceMultiplier>500</MinPriceMultiplier>
        <MaxPriceMultiplier>600</MaxPriceMultiplier>
        <MinAmount>10000</MinAmount>
        <MaxAmount>50000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>100</MinPriceMultiplier>
        <MaxPriceMultiplier>200</MaxPriceMultiplier>
        <MinAmount>10000</MinAmount>
        <MaxAmount>50000</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Ore2</StoreItemId>
      <ItemType>Ore</ItemType>{Ingot2}
      <Offer>
        <MinPriceMultiplier>600</MinPriceMultiplier>
        <MaxPriceMultiplier>700</MaxPriceMultiplier>
        <MinAmount>6000</MinAmount>
        <MaxAmount>40000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>650</MinPriceMultiplier>
        <MaxPriceMultiplier>700</MaxPriceMultiplier>
        <MinAmount>6000</MinAmount>
        <MaxAmount>40000</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ore3</StoreItemId>
      <ItemType>Ore</ItemType>{Ingot3}
      <Offer>
        <MinPriceMultiplier>800</MinPriceMultiplier>
        <MaxPriceMultiplier>900</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>1080</MinPriceMultiplier>
        <MaxPriceMultiplier>1000</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ore4</StoreItemId>
      <ItemType>Ore</ItemType>{Ingot4}
      <Offer>
        <MinPriceMultiplier>900</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>1250</MinPriceMultiplier>
        <MaxPriceMultiplier>1350</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Order>
    </StoreItem>	

    
    <StoreItem>
      <StoreItemId>Ore5</StoreItemId>
      <ItemType>Ore</ItemType>{Ingot5}
      <Offer>
        <MinPriceMultiplier>1000</MinPriceMultiplier>
        <MaxPriceMultiplier>1100</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>1700</MinPriceMultiplier>
        <MaxPriceMultiplier>1800</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Order>
    </StoreItem>



    <!-- Component -->
    <StoreItem>
      <StoreItemId>Component1</StoreItemId>
      <ItemType>Component</ItemType>{Component1}
      <Offer>
        <MinPriceMultiplier>60</MinPriceMultiplier>
        <MaxPriceMultiplier>70</MaxPriceMultiplier>
        <MinAmount>50</MinAmount>
        <MaxAmount>1500</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>20</MinPriceMultiplier>
        <MaxPriceMultiplier>40</MaxPriceMultiplier>
        <MinAmount>50</MinAmount>
        <MaxAmount>1500</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Component2</StoreItemId>
      <ItemType>Component</ItemType>{Component2}
      <Offer>
        <MinPriceMultiplier>120</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>20</MinAmount>
        <MaxAmount>500</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>90</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>20</MinAmount>
        <MaxAmount>500</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Component3</StoreItemId>
      <ItemType>Component</ItemType>{Component3}
      <Offer>
        <MinPriceMultiplier>190</MinPriceMultiplier>
        <MaxPriceMultiplier>210</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>20</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>150</MinPriceMultiplier>
        <MaxPriceMultiplier>160</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>20</MaxAmount>
      </Order>
    </StoreItem>


    <!-- Ammo -->
    <StoreItem>
      <StoreItemId>Ammo1</StoreItemId>
      <ItemType>Ammo</ItemType>{Ammo1}
      <Offer>
        <MinPriceMultiplier>50</MinPriceMultiplier>
        <MaxPriceMultiplier>60</MaxPriceMultiplier>
        <MinAmount>50</MinAmount>
        <MaxAmount>500</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>20</MinPriceMultiplier>
        <MaxPriceMultiplier>40</MaxPriceMultiplier>
        <MinAmount>50</MinAmount>
        <MaxAmount>500</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ammo2</StoreItemId>
      <ItemType>Ammo</ItemType>{Ammo2}
      <Offer>
        <MinPriceMultiplier>110</MinPriceMultiplier>
        <MaxPriceMultiplier>120</MaxPriceMultiplier>
        <MinAmount>30</MinAmount>
        <MaxAmount>100</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>90</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>30</MinAmount>
        <MaxAmount>100</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Ammo3</StoreItemId>
      <ItemType>Ammo</ItemType>{Ammo3}
      <Offer>
        <MinPriceMultiplier>140</MinPriceMultiplier>
        <MaxPriceMultiplier>150</MaxPriceMultiplier>
        <MinAmount>30</MinAmount>
        <MaxAmount>100</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>130</MinPriceMultiplier>
        <MaxPriceMultiplier>140</MaxPriceMultiplier>
        <MinAmount>30</MinAmount>
        <MaxAmount>100</MaxAmount>
      </Order>
    </StoreItem>   

  


    <!-- Tool -->
    <StoreItem>
      <StoreItemId>Tool1</StoreItemId>
      <ItemType>Tool</ItemType>{Tool1}
      <Offer>
        <MinPriceMultiplier>60</MinPriceMultiplier>
        <MaxPriceMultiplier>70</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>10</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>20</MinPriceMultiplier>
        <MaxPriceMultiplier>40</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>10</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Tool2</StoreItemId>
      <ItemType>Tool</ItemType>{Tool2}
      <Offer>
        <MinPriceMultiplier>120</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>10</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>90</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>10</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Tool3</StoreItemId>
      <ItemType>Tool</ItemType>{Tool3}
      <Offer>
        <MinPriceMultiplier>190</MinPriceMultiplier>
        <MaxPriceMultiplier>210</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>10</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>150</MinPriceMultiplier>
        <MaxPriceMultiplier>160</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>10</MaxAmount>
      </Order>
    </StoreItem>

    <!-- Consumable -->
    <StoreItem>
      <StoreItemId>Consumable1</StoreItemId>
      <ItemType>Consumable</ItemType>{Consumable1}
      <Offer>
        <MinPriceMultiplier>60</MinPriceMultiplier>
        <MaxPriceMultiplier>70</MaxPriceMultiplier>
        <MinAmount>5</MinAmount>
        <MaxAmount>50</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>20</MinPriceMultiplier>
        <MaxPriceMultiplier>40</MaxPriceMultiplier>
        <MinAmount>50</MinAmount>
        <MaxAmount>50</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Consumable2</StoreItemId>
      <ItemType>Consumable</ItemType>{Consumable2}
      <Offer>
        <MinPriceMultiplier>120</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>20</MinAmount>
        <MaxAmount>100</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>90</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>20</MinAmount>
        <MaxAmount>100</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Consumable3</StoreItemId>
      <ItemType>Consumable</ItemType>{Consumable3}
      <Offer>
        <MinPriceMultiplier>190</MinPriceMultiplier>
        <MaxPriceMultiplier>210</MaxPriceMultiplier>
        <MinAmount>20</MinAmount>
        <MaxAmount>100</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>150</MinPriceMultiplier>
        <MaxPriceMultiplier>160</MaxPriceMultiplier>
        <MinAmount>20</MinAmount>
        <MaxAmount>100</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>HydrogenOffer</StoreItemId>
      <ItemType>Hydrogen</ItemType>
      <ItemSubtypeId>Hydrogen</ItemSubtypeId>
      <Offer>
        <MinPriceMultiplier>75</MinPriceMultiplier>
        <MaxPriceMultiplier>125</MaxPriceMultiplier>
        <MinAmount>10000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Offer>
    </StoreItem>

    
    <StoreItem>
      <StoreItemId>OxygenOffer</StoreItemId>
      <ItemType>Oxygen</ItemType>
      <ItemSubtypeId>Oxygen</ItemSubtypeId>
      <Offer>
        <MinPriceMultiplier>65</MinPriceMultiplier>
        <MaxPriceMultiplier>115</MaxPriceMultiplier>
        <MinAmount>10000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Offer>
    </StoreItem>

    <StoreItem>
      <StoreItemId>IceOffer</StoreItemId>
      <ItemType>Ore</ItemType>
      <ItemSubtypeIds> 
        <ItemSubtypeId>Ice</ItemSubtypeId>  
      </ItemSubtypeIds> 

      <Offer>
        <MinPriceMultiplier>110</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>100000</MinAmount>
        <MaxAmount>200000</MaxAmount>
      </Offer>

    </StoreItem>


  </StoreItems>
</StoreItemsContainer>
"""
    with open(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 1/Data/StoreItems/AaW_StoreItems_{Name}.xml', 'w') as f:
        f.write(StoreItems)