import PIL
import os
import sys
from PIL import ImageTk,Image
from collections import Counter
import os

# get the path of the current script
current_script_path = os.path.realpath(__file__)

# get the directory containing the script
script_directory = os.path.dirname(current_script_path)

# change the current working directory to the script directory
os.chdir(script_directory)

# print the new current working directory
print("New current working directory:", os.getcwd())

import time
from tkinter import *
from tkinter import filedialog, Text
Mloc = filedialog.askopenfilename(filetypes=(("png files", "*.png"),("all files", "*.*")))

img = PIL.Image.open(Mloc)
img.load()


front = img.crop((0,0,2048,2048))
right = img.crop((2048,0,4096,2048))
back = img.crop((4096,0,6144,2048))
left = img.crop((6144,0,8192,2048))

front.save('front.png')
right.save('right.png')
back.save('back.png')
left.save('left.png')
