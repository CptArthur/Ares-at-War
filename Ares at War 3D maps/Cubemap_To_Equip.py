import numpy as np
from PIL import Image
import py360convert
import tkinter as tk
from tkinter import filedialog
import os

# Create a tkinter root window (it won't be shown)
root = tk.Tk()
root.withdraw()

# Open a file dialog to select the image
file_path = filedialog.askopenfilename(filetypes=[("PNG Files", "*.png")])

if file_path:
    # Load image with forced RGB conversion
    img = Image.open(file_path).convert('RGB')  # <- Critical fix here
    cube_dice = np.array(img)
    
    # Debug check
    print("Image shape:", cube_dice.shape)  # Should show (H, W, 3)
    
    try:
        cube_h = py360convert.c2e(cube_dice, 4096, 8192, cube_format='dice')
        
        # Save result
        equirec_image = np.uint8(np.clip(cube_h, 0, 255))
        output_path = os.path.join(os.path.dirname(file_path), 
                                 f"EQUI_{os.path.basename(file_path)}")
        Image.fromarray(equirec_image).save(output_path)
        print(f"Saved: {output_path}")
        
    except Exception as e:
        print(f"Conversion failed: {str(e)}")
        print("Possible causes: Invalid cube map layout, wrong dimensions, or unsupported color mode")
else:
    print("No file selected.")