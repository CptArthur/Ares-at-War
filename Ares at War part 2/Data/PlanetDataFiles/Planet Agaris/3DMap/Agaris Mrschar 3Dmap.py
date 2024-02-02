import PIL
import os
import sys
from PIL import ImageTk,Image
from collections import Counter
import os


import time
from tkinter import *
from tkinter import filedialog, Text

import os

# get the path of the current script
current_script_path = os.path.realpath(__file__)

# get the directory containing the script
script_directory = os.path.dirname(current_script_path)

# change the current working directory to the script directory
os.chdir(script_directory)

# print the new current working directory
print("New current working directory:", os.getcwd())



Mloc = filedialog.askopenfilename(filetypes=(("png files", "*.png"),("all files", "*.*")))

img = PIL.Image.open(Mloc)
img.load()


left = img.crop((0,2048,2048,4096))
up = img.crop((2048,0,4096,2048))
front = img.crop((2048,2048,4096,4096))
right = img.crop((4096,2048,6144,4096))
back = img.crop((6144,2048,8192,4096))

front.save('front.png')
right.save('right.png')
back.save('back.png')
left.save('left.png')
up.save("up.png")

print("done")

