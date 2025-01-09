from pickle import TRUE
import PIL
import os
import sys
from PIL import ImageTk,Image
from collections import Counter
import Setup


def Matboy(Mloc,rloc,gloc,bloc,Red,Green,Blue,side):
    MAT = PIL.Image.open(Mloc)
    rgb_MAT = MAT.convert('RGB')
#red channel
    if (MrhoekR):
       print('fixing the corners on the red map')
       im = PIL.Image.open(rloc)
       im.load()

       x = 0
       y = 0
       for z in range(0, 2048*2048):
           if x == 0:
               colode = im.getpixel((x+1, y))
               im.putpixel((x,y),(colode))
           if x == 2047:
               colode = im.getpixel((x-1, y))
               im.putpixel((x,y),(colode))
           if y == 0:
               colode = im.getpixel((x, y+1))
               im.putpixel((x,y),(colode))
           if y == 2047:
               colode = im.getpixel((x, y-1))
               im.putpixel((x,y),(colode))

           x = x + 1
           if x == 2048:
               y = y + 1
               x = 0

       im.putpixel((0,0),(im.getpixel((1, 1))))
       im.putpixel((0,2047),(im.getpixel((1, 2046))))
       im.putpixel((2047,0),(im.getpixel((2046, 1))))
       im.putpixel((2047,2047),(im.getpixel((2046, 2046))))

       im.save(rloc)
#Green channel
    if (MrhoekG):
       print('fixing the corners on the green map')
       im = PIL.Image.open(gloc)
       im.load()

       x = 0
       y = 0
       for z in range(0, 2048*2048):
           if x == 0:
               colode = im.getpixel((x+1, y))
               im.putpixel((x,y),(colode))
           if x == 2047:
               colode = im.getpixel((x-1, y))
               im.putpixel((x,y),(colode))
           if y == 0:
               colode = im.getpixel((x, y+1))
               im.putpixel((x,y),(colode))
           if y == 2047:
               colode = im.getpixel((x, y-1))
               im.putpixel((x,y),(colode))

           x = x + 1
           if x == 2048:
               y = y + 1
               x = 0

       im.putpixel((0,0),(im.getpixel((1, 1))))
       im.putpixel((0,2047),(im.getpixel((1, 2046))))
       im.putpixel((2047,0),(im.getpixel((2046, 1))))
       im.putpixel((2047,2047),(im.getpixel((2046, 2046))))

       im.save(gloc)

#Blue channel
    if MrhoekB:
       print('fixing the corners on the blue map')
       im = PIL.Image.open(bloc)
       im.load()

       x = 0
       y = 0
       for z in range(0, 2048*2048):
           if x == 0:
               colode = im.getpixel((x+1, y))
               im.putpixel((x,y),(colode))
           if x == 2047:
               colode = im.getpixel((x-1, y))
               im.putpixel((x,y),(colode))
           if y == 0:
               colode = im.getpixel((x, y+1))
               im.putpixel((x,y),(colode))
           if y == 2047:
               colode = im.getpixel((x, y-1))
               im.putpixel((x,y),(colode))

           x = x + 1
           if x == 2048:
               y = y + 1
               x = 0

       im.putpixel((0,0),(im.getpixel((1, 1))))
       im.putpixel((0,2047),(im.getpixel((1, 2046))))
       im.putpixel((2047,0),(im.getpixel((2046, 1))))
       im.putpixel((2047,2047),(im.getpixel((2046, 2046))))

       im.save(bloc)

    if Red:
        VOX = PIL.Image.open(rloc)
        rgb_im = VOX.convert('RGB')
    else:
        print('Voxel file not included')

    if Green:
        BIOM = PIL.Image.open(gloc)
        rgb_BIOM = BIOM.convert('RGB')
    else:
        print("Biome File not inlcuded")

    if usebasicoremap:
        BasicOremap = PIL.Image.open("D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/Python/basicoremap.png")
        rgb_BasicOremap = BasicOremap.convert('RGB')
    else:
        print("usebasicoremap not inlcuded")       

    if Blue:
        ore = PIL.Image.open(bloc)
        rgb_ore = ore.convert('RGB')
    else:
        print("ore File not inlcuded")

    MAT.load()
    r, g, b = rgb_MAT.split()
    x = 0
    y = 0
    for z in range(0, 2048*2048):
    # Red Channel

        if Red:
            Colode = rgb_im.getpixel((x, y))
            Vkeys = Setup.redlist.keys()
            if Colode in Vkeys:
                c = Setup.redlist[Colode]
                r.putpixel((x, y), (c))

    # Green Channel
        if Green:
            Bilode = rgb_BIOM.getpixel((x, y))
            Bkeys = Setup.greenlist.keys()
            if Bilode in Bkeys:
                d = Setup.greenlist[Bilode]
                g.putpixel((x, y), (d))

        if usebasicoremap:
            orecode = rgb_BasicOremap.getpixel((x, y))
            Okeys = Setup.bluelist.keys()
            if orecode in Okeys:
                d = Setup.bluelist[orecode]
                b.putpixel((x, y), (d))           
    # Blue Channel
        if Blue:
            orecode = rgb_ore.getpixel((x, y))
            Bkeys = Setup.bluelist.keys()
            if orecode in Bkeys:
                d = Setup.bluelist[orecode]
                b.putpixel((x, y), (d))

        x = x + 1
        if x == 2048:
            y = y + 1
            x = 0

    MAT_nieuw = PIL.Image.merge( 'RGB', (r, g, b))
    MAT_nieuw.save(Mloc)
    print('Klaar met' + side)






MrhoekR = False
MrhoekG = False
MrhoekB = False




Red = False
Green = False
Blue = True




sides = ["back","down","front","left","right","up"]
PlanetName = "Planet Crait" #Als Agaris of Thora4 dan moet (208,208,208) aan
usebasicoremap = False

location = f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{PlanetName}/"
Importlocation = f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{PlanetName}/Import/"
locationIO = f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War part 2/Data/PlanetDataFiles/{PlanetName} - IO/"

for side in sides:
    Mloc = location + side + "_mat.png"
    rloc = Importlocation + side + "vox.png"
    gloc = Importlocation + side + "biom.png"
    bloc = Importlocation + side + "ore.png"
    Matboy(Mloc,rloc,gloc,bloc,Red,Green,Blue,side)

for side in sides:
    Mloc = locationIO + side + "_mat.png"
    rloc = Importlocation + side + "vox.png"
    gloc = Importlocation + side + "biom.png"
    bloc = Importlocation + side + "oreIO.png"
    Matboy(Mloc,rloc,gloc,bloc,Red,Green,Blue,side)



print("Klaar")
input()