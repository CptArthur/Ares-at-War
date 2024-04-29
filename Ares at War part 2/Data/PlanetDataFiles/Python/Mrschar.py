
import PIL
import os
import sys
from PIL import ImageTk,Image
from collections import Counter


import time
from tkinter import *
from tkinter import filedialog, Text






def run():

    #Height
    if(height == True):

        print("Starting MrHeight")
        img = PIL.Image.open(f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/{filename}_high-res-heightmap.png")
        img.load()


        back = img.crop(backcoords)
        left = img.crop(leftcoords)
        front = img.crop(frontcoords)
        right = img.crop(rightcoords)
        up = img.crop(upcoords)
        down = img.crop(downcoords)



        front.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/front.png')
        right.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/right.png')
        back.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/back.png')
        left.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/left.png')
        up.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/up.png')
        down.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/down.png')

        front.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet} - IO/front.png')
        right.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet} - IO/right.png')
        back.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet} - IO/back.png')
        left.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet} - IO/left.png')
        up.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet} - IO/up.png')
        down.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet} - IO/down.png')


    #Biome
    if(Biome):
        print("Starting MrBiome")
        img = PIL.Image.open(f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/allbiome.png")
        img.load()


        back = img.crop(backcoords)
        left = img.crop(leftcoords)
        front = img.crop(frontcoords)
        right = img.crop(rightcoords)
        up = img.crop(upcoords)
        down = img.crop(downcoords)

        front.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/frontbiom.png')
        right.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/rightbiom.png')
        back.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/backbiom.png')
        left.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/leftbiom.png')
        up.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/upbiom.png')
        down.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/downbiom.png')


    #Vox
    if(Vox):
        print("Starting MrVox")
        img = PIL.Image.open(f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/allvox.png")
        img.load()

        back = img.crop(backcoords)
        left = img.crop(leftcoords)
        front = img.crop(frontcoords)
        right = img.crop(rightcoords)
        up = img.crop(upcoords)
        down = img.crop(downcoords)

        front.save(f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/frontvox.png")
        right.save(f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/rightvox.png")
        back.save(f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/backvox.png")
        left.save(f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/leftvox.png")
        up.save(f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/upvox.png")
        down.save(f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/downvox.png")

    if(Ore):
        #Ore vanilla
        print("Starting MrOre")
        img = PIL.Image.open(f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/allore.png")
        img.load()


        back = img.crop(backcoords)
        left = img.crop(leftcoords)
        front = img.crop(frontcoords)
        right = img.crop(rightcoords)
        up = img.crop(upcoords)
        down = img.crop(downcoords)

        front.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/frontore.png')
        right.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/rightore.png')
        back.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/backore.png')
        left.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/leftore.png')
        up.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/upore.png')
        down.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/downore.png')


        #Ore IO
        img = PIL.Image.open(f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/alloreIO.png")
        img.load()


        back = img.crop(backcoords)
        left = img.crop(leftcoords)
        front = img.crop(frontcoords)
        right = img.crop(rightcoords)
        up = img.crop(upcoords)
        down = img.crop(downcoords)

        front.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/frontoreIO.png')
        right.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/rightoreIO.png')
        back.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/backoreIO.png')
        left.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/leftoreIO.png')
        up.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/uporeIO.png')
        down.save(f'D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{Planet}/Import/downoreIO.png')





#Setup
# Planet = "Planet Thora 4"
# filename = "thora4" 

Planet = "Planet Lezuno"
filename = "lezuno" 

#Planet = "Planet Lorus"
#filename = "lorus" 

#Planet = "Planet Agaris"
#filename = "agaris" 


#Planet = "Planet Crait"
#filename = "crait" 

height = True
Vox = True
Biome = False
Ore = False

#Normal
upcoords = (0,0,2048,2048)
downcoords = (4096,4096,6144,6144)
frontcoords = (0,2048,2048,4096)
rightcoords = (2048,2048,4096,4096)
backcoords = (4096,2048,6144,4096)
leftcoords = (6144,2048,8192,4096)



if(Planet == "Planet Agaris"):
    #Agaris
    upcoords = (4096,0,6144,2048)
    downcoords = (0,4096,2048,6144)
    backcoords = (0,2048,2048,4096)
    leftcoords = (2048,2048,4096,4096)
    frontcoords = (4096,2048,6144,4096)
    rightcoords = (6144,2048,8192,4096)


run()
print("Done")



