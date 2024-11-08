import numpy as np
from PIL import Image
import py360convert
import tkinter as tk
from tkinter import filedialog
import os

# Create a tkinter root window (it won't be shown)
root = tk.Tk()
root.withdraw()  # Hide the root window

# Open a file dialog to select the image
file_path = filedialog.askopenfilename(filetypes=[("PNG Files", "*.png")])

# Check if a file was selected
if file_path:
    # Load the selected PNG image
    cube_dice = np.array(Image.open(file_path))

    # Convert the cube map image to equirectangular format
    cube_h = py360convert.c2e(cube_dice, 4096, 8192, cube_format='dice')

    # Save the resulting equirectangular image as PNG
    equirec_image = np.uint8(np.clip(cube_h, 0, 255))  # Convert to uint8 type for image saving
    img = Image.fromarray(equirec_image)  # Convert NumPy array to PIL Image

    # Get the folder and filename of the selected image
    folder = os.path.dirname(file_path)
    filename = os.path.basename(file_path)
    output_path = os.path.join(folder, f"{filename}")  # Save the output image with a modified name

    # Save the image
    img.save(output_path)

    print(f"Image saved as: {output_path}")
else:
    print("No file selected.")
