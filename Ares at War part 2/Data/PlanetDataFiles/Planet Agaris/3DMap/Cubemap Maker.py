import PIL
import os
import sys
from PIL import ImageTk,Image
from collections import Counter


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










root = Tk()
root.title('Matboy')
root.configure(background='grey')

def autoload():

    global Bloc
    Bloc = 'back.png'
    back_btn.configure(text = os.path.split(Bloc)[1])
    print('Bloc')

    global Dloc
    Dloc = 'down.png'
    down_btn.configure(text= os.path.split(Dloc)[1])

    global Floc
    Floc = 'front.png'
    front_btn.configure(text= os.path.split(Floc)[1])

    global Lloc
    Lloc = 'left.png'
    left_btn.configure(text = os.path.split(Lloc)[1])

    global Rloc
    Rloc = 'right.png'
    right_btn.configure(text= os.path.split(Rloc)[1])

    global Uloc
    Uloc = 'up.png'
    up_btn.configure(text= os.path.split(Uloc)[1])

#Back
def Bopen():
    global Bloc
    Bloc = filedialog.askopenfilename(filetypes=(("png files", "*.png"),("all files", "*.*")))
    if Bloc:
        back_btn.configure(text = os.path.split(Bloc)[1])
        print(Bloc)
    else:
        print("Nothing Chosen")

#Down
def Dopen():
    global Dloc
    Dloc = filedialog.askopenfilename(filetypes=(("png files", "*.png"),("all files", "*.*")))
    down_btn.configure(text= os.path.split(Dloc)[1])
    print(gloc)

#Front
def Fopen():
    global Floc
    Floc = filedialog.askopenfilename(filetypes=(("png files", "*.png"),("all files", "*.*")))
    print(Floc)
    front_btn.configure(text= os.path.split(Floc)[1])


#left
def Lopen():
    global Lloc
    Lloc = filedialog.askopenfilename(filetypes=(("png files", "*.png"),("all files", "*.*")))
    if Lloc:
        left_btn.configure(text = os.path.split(Lloc)[1])
        print(Lloc)
    else:
        print("Nothing Chosen")

#right
def Ropen():
    global Rloc
    Rloc = filedialog.askopenfilename(filetypes=(("png files", "*.png"),("all files", "*.*")))
    right_btn.configure(text= os.path.split(Rloc)[1])
    print(Rloc)

#up
def Uopen():
    global Uloc
    Uloc = filedialog.askopenfilename(filetypes=(("png files", "*.png"),("all files", "*.*")))
    print(Uloc)
    up_btn.configure(text= os.path.split(Uloc)[1])




def Clear():
    global rloc
    global gloc
    global bloc
    rloc = 2
    gloc = 2
    bloc = 2
    mat_btn.configure(text = 'Mat file')
def show():
    checklabelB = Label(root, text=varB.get())
    checklabelB.grid(row=1, column=4)
    checklabelD = Label(root, text=varD.get())
    checklabelD.grid(row=2, column=4)
    checklabelF = Label(root, text=varF.get())
    checklabelF.grid(row=3, column=4)

    checklabelL = Label(root, text=varL.get())
    checklabelL.grid(row=4, column=4)
    checklabelR = Label(root, text=varR.get())
    checklabelR.grid(row=5, column=4)
    checklabelU = Label(root, text=varU.get())
    checklabelU.grid(row=6, column=4)


def run():

#Back
    if varB.get() == 'on':
       print('fixing the corners on back')
       im = PIL.Image.open(Bloc)
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

       im.save(Bloc)

#Down
    if varD.get() == 'on':
       print('fixing the corners on down')
       im = PIL.Image.open(Dloc)
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

       im.save(Dloc)

#Front
    if varF.get() == 'on':
       print('fixing the corners on front')
       im = PIL.Image.open(Floc)
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

       im.save(Floc)

#Left
    if varL.get() == 'on':
       print('fixing the corners on left')
       im = PIL.Image.open(Lloc)
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

       im.save(Lloc)

#Right
    if varR.get() == 'on':
       print('fixing the corners on right')
       im = PIL.Image.open(Rloc)
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

       im.save(Rloc)

#Up
    if varU.get() == 'on':
       print('fixing the corners on up')
       im = PIL.Image.open(Uloc)
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

       im.save(Uloc)


    cubemap = PIL.Image.new('RGB',(8192,6144))
    back = PIL.Image.open(Bloc)
    down = PIL.Image.open(Dloc)
    front = PIL.Image.open(Floc)
    left = PIL.Image.open(Lloc)
    right = PIL.Image.open(Rloc)
    up = PIL.Image.open(Uloc)

    cubemap.paste(left,box=(0,2048))
    cubemap.paste(front,box=(2048,2048))
    cubemap.paste(right,box=(4096,2048))
    cubemap.paste(back,box=(6144,2048))
    cubemap.paste(up,box=(2048,0))
    cubemap.paste(down,box=(2048,4096))
    cubemap.show()


mat_btn = Button(root, text='autoload', command=autoload)
mat_btn.grid(row=0, column=0)

back_btn = Button(root, text='back', command=Bopen)
back_btn.grid(row=1, column=0)

down_btn = Button(root, text='down', command=Dopen)
down_btn.grid(row=2, column=0)

front_btn = Button(root, text='front', command=Fopen)
front_btn.grid(row=3, column=0)

left_btn = Button(root, text='left', command=Lopen)
left_btn.grid(row=4, column=0)

right_btn = Button(root, text='right', command=Ropen)
right_btn.grid(row=5, column=0)

up_btn = Button(root, text='up', command=Uopen)
up_btn.grid(row=6, column=0)


clear_btn = Button(root, text='Clear', command=Clear)
clear_btn.grid(row=0, column=1)

varB = StringVar()
varD = StringVar()
varF = StringVar()
varL = StringVar()
varR = StringVar()
varU = StringVar()

check_back = Checkbutton(root, text='corner fix', variable=varB, onvalue='on', offvalue='off', command=show)
check_back.grid(row=1, column=1)
check_back.deselect()


check_down = Checkbutton(root, text='corner fix', variable=varD, onvalue='on', offvalue='off', command=show)
check_down.grid(row=2, column=1)
check_down.deselect()

check_front = Checkbutton(root, text='corner fix', variable=varF, onvalue='on', offvalue='off', command=show)
check_front.grid(row=3, column=1)
check_front.deselect()

check_left = Checkbutton(root, text='corner fix', variable=varL, onvalue='on', offvalue='off', command=show)
check_left.grid(row=4, column=1)
check_left.deselect()

check_right = Checkbutton(root, text='corner fix', variable=varR, onvalue='on', offvalue='off', command=show)
check_right.grid(row=5, column=1)
check_right.deselect()

check_up = Checkbutton(root, text='corner fix', variable=varU, onvalue='on', offvalue='off', command=show)
check_up.grid(row=6, column=1)
check_up.deselect()



run_btn = Button(root, text='Run', command=run)
run_btn.grid(row=7, column=1)
#Credits_btn = Button(root, text='Credits', command=open).pack()

root.mainloop()
