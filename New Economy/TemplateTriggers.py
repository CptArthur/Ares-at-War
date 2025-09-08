#Store Restaurant-Agaris
#Store Restaurant-Thora4
#Store Restaurant-Space

ITC = "{ITC}"
SHIVAN = "{SHIVAN}"
SECURITY = "{SECURITY}"
AGURO = "{AGURO}"  	
UNION = "{UNION}"  
SOLCOOP = "{SOLCOOP}" 
ZENOVA = "{ZENOVA}" 
AHE = "{AHE}" 




#Special
def GetStoreSettlement(Faction:str, Name:str)->str:  
    string = f"""[StoreBlocks:Store Settlement]
[StoreProfiles:{Faction}_StoreProfile_{Name}_Settlement]"""

    return string
    
#Special
def GetStoreMilitary(Faction:str, Name:str)->str:  
    string = f"""[StoreBlocks:Store Military]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Military]"""
    return string    
    
#Special
def GetStoreTradestation(Faction:str, Name:str)->str:  
    string = f"""[StoreBlocks:Store Tradestation]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Tradestation]"""
    return string


def GetStoreUniversity(Faction:str, Name:str)->str:  
    string = f"""[StoreBlocks:Store University]
      [StoreProfiles:AaW_StoreProfile_{Name}_University]"""
    return string

def GetStoreITC(Faction:str, Name:str)->str:  
    string = f"""[StoreBlocks:Store {ITC}]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_ITC]	
"""       
    return string


def GetStoreZENOVA(Faction:str, Name:str)->str:  
    string = f"""[StoreBlocks:Store {ZENOVA}]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_ZENOVA]"""       
    return string

def GetStoreSECURITY(Faction:str, Name:str)->str:  
    string = f"""[StoreBlocks:Store {SECURITY}]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_SECURITY]"""       
    
    return string

def GetStoreAGURO(Faction:str, Name:str)->str:  
    string = f"""[StoreBlocks:Store {AGURO}]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_AGURO]"""       
    return string


def GetStoreUNION(Faction:str, Name:str)->str:  
    string = f"""[StoreBlocks:Store {UNION}]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_UNION]	""" 

    return string

def GetStoreSOLCOOP(Faction:str, Name:str)->str:  
    string = f"""[StoreBlocks:Store {SOLCOOP}]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_SOLCOOP]	"""       

    return string

def GetStoreSHIVAN(Faction:str, Name:str)->str:  

    string = f"""[StoreBlocks:Store {SHIVAN}]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_SHIVAN]	"""   

    return string

def GetStoreAHE(Faction:str, Name:str)->str:  

    string = f"""[StoreBlocks:Store {AHE}]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_AHE]	"""   

    return string


def GetStoreVendingMachine(Faction:str, Name:str)->str:  
    string = f"""[StoreBlocks:Vending Machine]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_VendingMachine]
"""       
    return string


#Outpost
def GetStoreOutpost(Faction:str, Name:str)->str:  
    string = f"""[StoreBlocks:Store Outpost]
      [StoreProfiles:{Faction}_StoreProfile_{Name}_Outpost]"""       

    return string



#Special
def GetStoreIngot(Faction:str, Name:str)->str:  
    string = f"""
    """

    return string
#Static from here
def GetStoreCiv(Faction:str, Name:str)->str:  
    string = f"""
 """
    return string


def GetStoreScrap(Faction:str, Name:str)->str:  
    string = f"""
"""       

    return string


#Static from here
def GetStoreAmmo(Faction:str, Name:str)->str:  
    string = f"""
"""
    return string

### Fuck
def GetContractZENOVA(Faction:str, Name:str)->str:  
    string = f"""[ContractBlocks:Contracts {ZENOVA}]
      [ContractBlockProfiles:ZENOVA]	
"""       
    return string


def GetContractITC(Faction:str, Name:str)->str:  
    string = f"""[ContractBlocks:Contracts {ITC}]
      [ContractBlockProfiles:{Faction}_ContractBlockProfile_{Name}_ITC]	
"""       
    return string

def GetContractSECURITY(Faction:str, Name:str)->str:  
    string = f"""[ContractBlocks:Contracts {SECURITY}]
      [ContractBlockProfiles:{Faction}_ContractBlockProfile_{Name}_SECURITY]"""       
    
    return string

def GetContractAGURO(Faction:str, Name:str)->str:  
    string = f"""[ContractBlocks:Contracts {AGURO}]
      [ContractBlockProfiles:{Faction}_ContractBlockProfile_{Name}_AGURO]"""       
    return string


def GetContractUNION(Faction:str, Name:str)->str:  
    string = f"""[ContractBlocks:Contracts {UNION}]
      [ContractBlockProfiles:{Faction}_ContractBlockProfile_{Name}_UNION]	""" 

    return string

def GetContractSOLCOOP(Faction:str, Name:str)->str:  
    string = f"""[ContractBlocks:Contracts {SOLCOOP}]
      [ContractBlockProfiles:{Faction}_ContractBlockProfile_{Name}_SOLCOOP]	"""       

    return string

def GetContractSHIVAN(Faction:str, Name:str)->str:  

    string = f"""[ContractBlocks:Contracts {SHIVAN}]
      [ContractBlockProfiles:{Faction}_ContractBlockProfile_{Name}_SHIVAN]	"""   

    return string

def GetContractZENOVA(Faction:str, Name:str)->str:  

    string = f"""[ContractBlocks:Contracts {SHIVAN}]
      [ContractBlockProfiles:{Faction}_ContractBlockProfile_{Name}_ZENOVA]	"""   

    return string
