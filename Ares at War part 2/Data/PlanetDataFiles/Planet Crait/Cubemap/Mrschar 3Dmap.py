import PIL
import os
import sys
from PIL import ImageTk,Image
from collections import Counter


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
