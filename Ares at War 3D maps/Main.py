

import PIL
import os
import sys
from PIL import ImageTk,Image
from collections import Counter


import time
from tkinter import *
from tkinter import filedialog, Text

def run(Agaris, file):

    if(Agaris):
        upcoords = (2048,0,4096,2048)
        downcoords = (6144,4096,8192,6144)
        backcoords = (6144,2048,8192,4096)
        leftcoords = (0,2048,2048,4096)
        frontcoords = (2048,2048,4096,4096)
        rightcoords = (4096,2048,6144,4096)

        Cubeupcoords = (2048,0,4096,2048)
        Cubedowncoords = (2048,4096,4096,6144)
        Cubeleftcoords = (0,2048,2048,4096)
        Cubefrontcoords = (2048,2048,4096,4096)
        Cuberightcoords = (4096,2048,6144,4096)
        Cubebackcoords = (6144,2048,8192,4096)
    else:
        upcoords = (0,0,2048,2048)
        downcoords = (4096,4096,6144,6144)
        frontcoords = (0,2048,2048,4096)
        rightcoords = (2048,2048,4096,4096)
        backcoords = (4096,2048,6144,4096)
        leftcoords = (6144,2048,8192,4096)

        #CubeMap Coords
        Cubeupcoords = (2048,0,4096,2048)
        Cubedowncoords = (2048,4096,4096,6144)
        Cubefrontcoords = (0,2048,2048,4096)
        Cuberightcoords = (2048,2048,4096,4096)
        Cubebackcoords = (4096,2048,6144,4096)
        Cubeleftcoords = (6144,2048,8192,4096)


    img = PIL.Image.open(f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War 3D maps/input/{file}")
    img.load()

    front = img.crop(Cubefrontcoords)
    right = img.crop(Cuberightcoords)
    back = img.crop(Cubebackcoords)
    left = img.crop(Cubeleftcoords)
    up = img.crop(Cubeupcoords)
    down = img.crop(Cubedowncoords)
    if(Agaris):
        down = down.rotate(180)
        print("Rotating down")
    else:
        up = up.rotate(90)
        down = down.rotate(90)

    newimg = PIL.Image.new('RGBA', (8192, 6144), (0,0,0,0))
    newimg.paste(front, frontcoords)
    newimg.paste(right, rightcoords)
    newimg.paste(back, backcoords)
    newimg.paste(left, leftcoords)
    newimg.paste(up, upcoords)
    newimg.paste(down, downcoords)
    newimg.save(f"D:/SEMods-Github/Ares-at-War-part-1/Ares at War 3D maps/output/{file}")






#Normal







currentDir = "D:/SEMods-Github/Ares-at-War-part-1/Ares at War 3D maps/input"
files = os.listdir(currentDir)



for file in files:
  if("agaris" in file):
    run(True,file)
  else:
    run(False,file)


input()

