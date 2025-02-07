

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
      <StoreItemId>CivilianProduct</StoreItemId>
      <ItemType>Component</ItemType>
      <ItemSubtypeIds> 
        <ItemSubtypeId>CivilianProductI</ItemSubtypeId>  
        <ItemSubtypeId>CivilianProductII</ItemSubtypeId>  
        <ItemSubtypeId>CivilianProductIII</ItemSubtypeId>  
      </ItemSubtypeIds> 

      <Offer>
        <MinPriceMultiplier>110</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>100</MinAmount>
        <MaxAmount>200</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>90</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>100</MinAmount>
        <MaxAmount>200</MaxAmount>
      </Order>
    </StoreItem>
    <StoreItem>
      <StoreItemId>Ingot1</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot1}
      <Offer>
        <MinPriceMultiplier>50</MinPriceMultiplier>
        <MaxPriceMultiplier>60</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>50000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>10</MinPriceMultiplier>
        <MaxPriceMultiplier>20</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>50000</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Ingot2</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot2}
      <Offer>
        <MinPriceMultiplier>80</MinPriceMultiplier>
        <MaxPriceMultiplier>90</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>15000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>65</MinPriceMultiplier>
        <MaxPriceMultiplier>70</MaxPriceMultiplier>
        <MinAmount>5000</MinAmount>
        <MaxAmount>15000</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ingot3</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot3}
      <Offer>
        <MinPriceMultiplier>120</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
        <MinAmount>1500</MinAmount>
        <MaxAmount>5000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>108</MinPriceMultiplier>
        <MaxPriceMultiplier>100</MaxPriceMultiplier>
        <MinAmount>1500</MinAmount>
        <MaxAmount>5000</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ingot4</StoreItemId>
      <ItemType>Ingot</ItemType>{Ingot4}
      <Offer>
        <MinPriceMultiplier>145</MinPriceMultiplier>
        <MaxPriceMultiplier>160</MaxPriceMultiplier>
        <MinAmount>500</MinAmount>
        <MaxAmount>2000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>125</MinPriceMultiplier>
        <MaxPriceMultiplier>135</MaxPriceMultiplier>
        <MinAmount>500</MinAmount>
        <MaxAmount>2000</MaxAmount>
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
        <MinAmount>50000</MinAmount>
        <MaxAmount>200000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>100</MinPriceMultiplier>
        <MaxPriceMultiplier>200</MaxPriceMultiplier>
        <MinAmount>50000</MinAmount>
        <MaxAmount>200000</MaxAmount>
      </Order>
    </StoreItem>
	
    <StoreItem>
      <StoreItemId>Ore2</StoreItemId>
      <ItemType>Ore</ItemType>{Ingot2}
      <Offer>
        <MinPriceMultiplier>800</MinPriceMultiplier>
        <MaxPriceMultiplier>900</MaxPriceMultiplier>
        <MinAmount>30000</MinAmount>
        <MaxAmount>80000</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>650</MinPriceMultiplier>
        <MaxPriceMultiplier>700</MaxPriceMultiplier>
        <MinAmount>30000</MinAmount>
        <MaxAmount>80000</MaxAmount>
      </Order>
    </StoreItem>

    <StoreItem>
      <StoreItemId>Ore3</StoreItemId>
      <ItemType>Ore</ItemType>{Ingot3}
      <Offer>
        <MinPriceMultiplier>1200</MinPriceMultiplier>
        <MaxPriceMultiplier>1300</MaxPriceMultiplier>
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
        <MinPriceMultiplier>1450</MinPriceMultiplier>
        <MaxPriceMultiplier>1600</MaxPriceMultiplier>
        <MinAmount>50</MinAmount>
        <MaxAmount>500</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>1250</MinPriceMultiplier>
        <MaxPriceMultiplier>1350</MaxPriceMultiplier>
        <MinAmount>500</MinAmount>
        <MaxAmount>5000</MaxAmount>
      </Order>
    </StoreItem>	

    
    <StoreItem>
      <StoreItemId>Ore5</StoreItemId>
      <ItemType>Ore</ItemType>{Ingot5}
      <Offer>
        <MinPriceMultiplier>1900</MinPriceMultiplier>
        <MaxPriceMultiplier>2100</MaxPriceMultiplier>
        <MinAmount>10</MinAmount>
        <MaxAmount>90</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>1700</MinPriceMultiplier>
        <MaxPriceMultiplier>1800</MaxPriceMultiplier>
        <MinAmount>10</MinAmount>
        <MaxAmount>90</MaxAmount>
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
        <MinPriceMultiplier>60</MinPriceMultiplier>
        <MaxPriceMultiplier>70</MaxPriceMultiplier>
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
        <MinPriceMultiplier>120</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
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


    <!-- Tool -->
    <StoreItem>
      <StoreItemId>Tool1</StoreItemId>
      <ItemType>Tool</ItemType>{Tool1}
      <Offer>
        <MinPriceMultiplier>60</MinPriceMultiplier>
        <MaxPriceMultiplier>70</MaxPriceMultiplier>
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
      <StoreItemId>Tool2</StoreItemId>
      <ItemType>Tool</ItemType>{Tool2}
      <Offer>
        <MinPriceMultiplier>120</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
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
      <StoreItemId>Tool3</StoreItemId>
      <ItemType>Tool</ItemType>{Tool3}
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

    <!-- Consumable -->
    <StoreItem>
      <StoreItemId>Consumable1</StoreItemId>
      <ItemType>Consumable</ItemType>{Consumable1}
      <Offer>
        <MinPriceMultiplier>60</MinPriceMultiplier>
        <MaxPriceMultiplier>70</MaxPriceMultiplier>
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
      <StoreItemId>Consumable2</StoreItemId>
      <ItemType>Consumable</ItemType>{Consumable2}
      <Offer>
        <MinPriceMultiplier>120</MinPriceMultiplier>
        <MaxPriceMultiplier>130</MaxPriceMultiplier>
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
      <StoreItemId>Consumable3</StoreItemId>
      <ItemType>Consumable</ItemType>{Consumable3}
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

    
       <StoreItem>
      <StoreItemId>RequestBeacons</StoreItemId>
      <ItemType>Consumable</ItemType>
      <ItemSubtypeIds> 
        <ItemSubtypeId>FAFSquadron</ItemSubtypeId>  
      </ItemSubtypeIds>
      <Offer>
        <MinPriceMultiplier>190</MinPriceMultiplier>
        <MaxPriceMultiplier>210</MaxPriceMultiplier>
        <MinAmount>1</MinAmount>
        <MaxAmount>4</MaxAmount>
      </Offer>
      <Order>
        <MinPriceMultiplier>150</MinPriceMultiplier>
        <MaxPriceMultiplier>160</MaxPriceMultiplier>
        <MinAmount>1</MinAmount>
        <MaxAmount>4</MaxAmount>
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
      <StoreItemId>Oxygen</StoreItemId>
      <ItemType>Oxygen</ItemType>
      <ItemSubtypeId>Oxygen</ItemSubtypeId>
      <Offer>
        <MinPriceMultiplier>65</MinPriceMultiplier>
        <MaxPriceMultiplier>115</MaxPriceMultiplier>
        <MinAmount>10000</MinAmount>
        <MaxAmount>20000</MaxAmount>
      </Offer>
    </StoreItem>

  </StoreItems>
</StoreItemsContainer>
"""
    with open(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 1/Data/StoreItems/AaW_StoreItems_{Name}.xml', 'w') as f:
        f.write(StoreItems)
